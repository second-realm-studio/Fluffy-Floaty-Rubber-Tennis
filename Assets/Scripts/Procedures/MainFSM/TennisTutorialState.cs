using Balls;
using Constants;
using PlayerEntities;
using UnityEngine;
using UnityEngine.SceneManagement;
using XiheFramework.Core.FSM;
using XiheFramework.Runtime;

namespace Procedures.MainFSM {
    public class TennisTutorialState : State<GameObject> {
        private uint m_LeftPlayerEntityId;
        private uint m_RightPlayerEntityId;
        private uint m_BallEntityId;

        private string m_OnScoreChangedEventHandlerId;
        private string m_OnBallAddHitCountEventHandlerId;

        public TennisTutorialState(StateMachine parentStateMachine, GameObject owner) : base(parentStateMachine, owner) { }

        public override void OnEnter() {
            Game.UI.ActivateUI(UINames.GameHud);

            //input
            Game.Input(0).controllers.maps.SetMapsEnabled(true, InputNames.CategoryGame);
            Game.Input(1).controllers.maps.SetMapsEnabled(true, InputNames.CategoryGame);

            //bgm
            AkSoundEngine.SetState("BGM", "Combat");
            AkSoundEngine.SetState("BGM_Combat", "Enter");
            AkSoundEngine.SetState("BGM_Combat", "Main");

            var left = Game.Entity.InstantiateEntity<TennisPlayerEntity>($"TennisPlayerEntity_{AnimalType.Sheep.ToString()}");
            m_LeftPlayerEntityId = left.EntityId;
            Game.Blackboard.SetData(BlackboardDataNames.EntityIdP1, m_LeftPlayerEntityId);
            left.gameObject.layer = LayerMask.NameToLayer("Player1");
            left.transform.position = Game.Config.FetchConfig<Vector3>(ConfigNames.TutorialPlayerSpawnPositionLeft);
            left.inputId = 0;
            left.isRightSide = false;
            left.racketRenderer.material.color = new Color(0.9686275f, 0.6196079f, 0.6392157f);
            foreach (Transform child in left.transform) {
                child.gameObject.layer = LayerMask.NameToLayer("Player1");
            }

            var right = Game.Entity.InstantiateEntity<TennisPlayerEntity>($"TennisPlayerEntity_{AnimalType.Rabbit.ToString()}");
            m_RightPlayerEntityId = right.EntityId;
            Game.Blackboard.SetData(BlackboardDataNames.EntityIdP2, m_RightPlayerEntityId);
            right.gameObject.layer = LayerMask.NameToLayer("Player2");
            right.transform.position = Game.Config.FetchConfig<Vector3>(ConfigNames.TutorialPlayerSpawnPositionRight);
            right.inputId = 1;
            right.isRightSide = true;
            right.racketRenderer.material.color = new Color(1f, 0.854902f, 0.5764706f);
            foreach (Transform child in right.transform) {
                child.gameObject.layer = LayerMask.NameToLayer("Player2");
            }

            var ball = Game.Entity.InstantiateEntity<GeneralBallEntity>("BallEntity_GeneralBall");
            Game.Blackboard.SetData(BlackboardDataNames.EntityIdBall, ball.EntityId);
            ball.transform.position = new Vector3(0, -50f, 0);
            ball.rigidBody.velocity = Vector3.zero;
            m_BallEntityId = ball.EntityId;
        }

        public override void OnUpdate() { }

        public override void OnExit() {
            Game.UI.UnactivateUI(UINames.GameHud);
            Game.Entity.DestroyEntity(m_LeftPlayerEntityId);
            Game.Entity.DestroyEntity(m_RightPlayerEntityId);
            Game.Entity.DestroyEntity(m_BallEntityId);
            Game.Blackboard.RemoveData(BlackboardDataNames.EntityIdP1);
            Game.Blackboard.RemoveData(BlackboardDataNames.EntityIdP2);
            Game.Blackboard.RemoveData(BlackboardDataNames.EntityIdBall);
        }

        private void OnBallHit(object sender, object e) {
            if (sender is not uint hitterId) {
                return;
            }

            Game.Scene.LoadSceneAsync(SceneNames.Tutorial, LoadSceneMode.Single, true, (scene) => { ChangeState(GameLoopStatesNames.Tutorial); });
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
    }
}