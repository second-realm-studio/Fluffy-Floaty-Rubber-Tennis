﻿using Constants;
using PlayerEntities;
using UnityEngine;
using XiheFramework.Core.FSM;
using XiheFramework.Runtime;

namespace Procedures.MainFSM {
    public class TennisGameState : State<GameObject> {
        private int m_WinScore;
        private uint m_LeftPlayerEntityId;
        private uint m_RightPlayerEntityId;

        private uint m_LastHitPlayerId;
        private int m_HitCount;

        public TennisGameState(StateMachine parentStateMachine, GameObject owner) : base(parentStateMachine, owner) { }

        public override void OnEnter() {
            Game.Event.Subscribe(EventNames.OnScoreChanged, OnScoreChanged);
            Game.Event.Subscribe(EventNames.OnBallAddHitCount, OnBallAddHitCount);

            //input
            Game.Input(0).controllers.maps.SetMapsEnabled(true, InputNames.CategoryGame);
            Game.Input(1).controllers.maps.SetMapsEnabled(true, InputNames.CategoryGame);

            //bgm
            AkSoundEngine.SetState("BGM", "Combat");
            AkSoundEngine.SetState("BGM_Combat", "Enter");
            AkSoundEngine.SetState("BGM_Combat", "Main");

            var leftPlayerAnimalType = Game.Blackboard.GetData<AnimalType>(BlackboardDataNames.PlayerCharacterType(0));
            var rightPlayerAnimalType = Game.Blackboard.GetData<AnimalType>(BlackboardDataNames.PlayerCharacterType(1));

            var left = Game.Entity.InstantiateEntity<TennisPlayerEntity>($"TennisPlayerEntity_{leftPlayerAnimalType.ToString()}");
            m_LeftPlayerEntityId = left.EntityId;
            left.transform.position = new Vector3(-10, -2, 0);
            left.inputId = 0;
            left.isRightSide = false;

            var right = Game.Entity.InstantiateEntity<TennisPlayerEntity>($"TennisPlayerEntity_{rightPlayerAnimalType.ToString()}");
            m_RightPlayerEntityId = right.EntityId;
            right.transform.position = new Vector3(10, -2, 0);
            right.inputId = 1;
            right.isRightSide = true;

            // m_WinScore = Game.Config.FetchConfig<int>(ConfigNames.GameWinScore);
            m_WinScore = 4;
        }

        public override void OnUpdate() { }

        public override void OnExit() {
            Game.Entity.DestroyEntity(m_LeftPlayerEntityId);
            Game.Entity.DestroyEntity(m_RightPlayerEntityId);
        }

        private void OnBallAddHitCount(object sender, object e) {
            if (sender is not uint hitterId) {
                return;
            }

            if (m_LastHitPlayerId == hitterId) {
                m_HitCount++;
            }
            else {
                m_HitCount = 1;
                m_LastHitPlayerId = hitterId;
            }

            if (m_HitCount >= 2) {
                var current = Game.Blackboard.GetData<int>(BlackboardDataNames.ScoreOffset);
                var entity = Game.Entity.GetEntity<TennisPlayerEntity>(hitterId);
                if (entity.isRightSide) {
                    Game.Blackboard.SetData(BlackboardDataNames.ScoreOffset, current + 1);
                }
                else {
                    Game.Blackboard.SetData(BlackboardDataNames.ScoreOffset, current - 1);
                }

                m_HitCount = 0;
            }
        }

        private void OnScoreChanged(object sender, object e) {
            if (sender is not uint scorerId) {
                return;
            }

            if (e is not int currentScoreOffset) {
                return;
            }

            if (currentScoreOffset <= -m_WinScore) {
                //left win
                Game.LogicTime.SetGlobalTimeScaleInSecond(0.2f, 4f, true);
            }

            if (currentScoreOffset >= m_WinScore) {
                //right win
                Game.LogicTime.SetGlobalTimeScaleInSecond(0.2f, 4f, true);
            }
        }
    }
}