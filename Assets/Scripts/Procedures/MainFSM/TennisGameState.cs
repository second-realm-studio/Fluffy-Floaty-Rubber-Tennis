using Balls;
using Constants;
using PlayerEntities;
using UnityEngine;
using XiheFramework.Core.FSM;
using XiheFramework.Runtime;

namespace Procedures.MainFSM {
    public class TennisGameState : State<GameObject> {
        private int m_WinScore = 6;
        private uint m_LeftPlayerEntityId;
        private uint m_RightPlayerEntityId;
        private uint m_BallEntityId;

        private string m_OnScoreChangedEventHandlerId;
        private string m_OnBallAddHitCountEventHandlerId;

        public TennisGameState(StateMachine parentStateMachine, GameObject owner) : base(parentStateMachine, owner) { }

        public override void OnEnter() {
            Game.Blackboard.SetData(BlackboardDataNames.HitCountP1, 0);
            Game.Blackboard.SetData(BlackboardDataNames.HitCountP2, 0);

            Game.UI.ActivateUI(UINames.GameHud);

            m_OnScoreChangedEventHandlerId = Game.Event.Subscribe(Game.Blackboard.onDataChangeEventName, OnScoreChanged);
            m_OnBallAddHitCountEventHandlerId = Game.Event.Subscribe(EventNames.OnBallHit, OnBallHit);

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
            left.transform.position = new Vector3(-10, -0, 0);
            left.inputId = 0;
            left.isRightSide = false;

            var right = Game.Entity.InstantiateEntity<TennisPlayerEntity>($"TennisPlayerEntity_{rightPlayerAnimalType.ToString()}");
            m_RightPlayerEntityId = right.EntityId;
            right.transform.position = new Vector3(10, 0, 0);
            right.inputId = 1;
            right.isRightSide = true;

            var ball = Game.Entity.InstantiateEntity<GeneralBallEntity>("BallEntity_GeneralBall");
            ball.transform.position = new Vector3(0, 0, 0);
            ball.rigidBody.velocity = Vector3.zero;
            m_BallEntityId = ball.EntityId;

            // m_WinScore = Game.Config.FetchConfig<int>(ConfigNames.GameWinScore);
        }

        public override void OnUpdate() { }

        public override void OnExit() {
            Game.Event.Unsubscribe(Game.Blackboard.onDataChangeEventName, m_OnScoreChangedEventHandlerId);
            Game.Event.Unsubscribe(EventNames.OnBallHit, m_OnBallAddHitCountEventHandlerId);
            Game.UI.UnactivateUI(UINames.GameHud);
            Game.Entity.DestroyEntity(m_LeftPlayerEntityId);
            Game.Entity.DestroyEntity(m_RightPlayerEntityId);
            Game.Entity.DestroyEntity(m_BallEntityId);
            Game.Blackboard.RemoveData(BlackboardDataNames.HitCountP1);
            Game.Blackboard.RemoveData(BlackboardDataNames.HitCountP2);
        }

        private void OnBallHit(object sender, object e) {
            if (sender is not uint hitterId) {
                return;
            }


            var entity = Game.Entity.GetEntity<TennisPlayerEntity>(hitterId);
            if (entity.isRightSide) {
                var hitCountR = Game.Blackboard.GetData<int>(BlackboardDataNames.HitCountP2);
                Game.Blackboard.SetData(BlackboardDataNames.HitCountP2, hitCountR + 1);

                //reset p1 to last save point
                var hitCountL = Game.Blackboard.GetData<int>(BlackboardDataNames.HitCountP1);
                var lastSavePoint = GetLastSavePoint(hitCountL);
                Game.Blackboard.SetData(BlackboardDataNames.HitCountP1, lastSavePoint);
            }
            else {
                var hitCountL = Game.Blackboard.GetData<int>(BlackboardDataNames.HitCountP1);
                Game.Blackboard.SetData(BlackboardDataNames.HitCountP1, hitCountL + 1);

                //reset p2 to last save point
                var hitCountR = Game.Blackboard.GetData<int>(BlackboardDataNames.HitCountP2);
                var lastSavePoint = GetLastSavePoint(hitCountR);
                Game.Blackboard.SetData(BlackboardDataNames.HitCountP2, lastSavePoint);
            }
        }

        int GetLastSavePoint(int currentHitCount) {
            switch (currentHitCount) {
                case 0: return 0;
                case 1: return 1;
                case 2: return 1;
                case 3: return 3;
                case 4: return 3;
                case 5: return 3;
                case 6: return 6;
                default: return 0;
            }
        }

        private void OnScoreChanged(object sender, object e) {
            if (sender is not string dataName) {
                return;
            }

            if (dataName == BlackboardDataNames.HitCountP1) {
                var countL = (int)e;

                if (countL >= m_WinScore) {
                    //left win
                    Game.LogicTime.SetGlobalTimeScaleInSecond(0.1f, 4f, true);
                    Game.Blackboard.SetData(BlackboardDataNames.WinnerName, "Left");
                    ChangeState(GameLoopStatesNames.GameOver);
                }
            }

            if (dataName == BlackboardDataNames.HitCountP2) {
                var countR = (int)e;

                if (countR >= m_WinScore) {
                    //right win
                    Game.LogicTime.SetGlobalTimeScaleInSecond(0.1f, 4f, true);
                    Game.Blackboard.SetData(BlackboardDataNames.WinnerName, "Right");
                    ChangeState(GameLoopStatesNames.GameOver);
                }
            }
        }
    }
}