using Constants;
using PlayerEntities;
using UnityEngine;
using XiheFramework.Core.Config.Entry;
using XiheFramework.Core.FSM;
using XiheFramework.Runtime;

namespace Procedures.MainFSM {
    public class TennisGameState : State<GameObject> {
        private int m_CurrentScoreOffset;
        private int m_WinScore;
        private uint m_LeftPlayerEntityId;
        private uint m_RightPlayerEntityId;


        public TennisGameState(StateMachine parentStateMachine, GameObject owner) : base(parentStateMachine, owner) { }

        public override void OnEnter() {
            var leftPlayerAnimalType = Game.Blackboard.GetData<string>(BlackboardDataNames.PlayerCharacterType(0));
            leftPlayerAnimalType = $"{nameof(TennisPlayerEntity)}_{leftPlayerAnimalType}";
            var rightPlayerAnimalType = Game.Blackboard.GetData<string>(BlackboardDataNames.PlayerCharacterType(1));
            rightPlayerAnimalType = $"{nameof(TennisPlayerEntity)}_{rightPlayerAnimalType}";

            var leftEntity = Game.Entity.InstantiateEntity<TennisPlayerEntity>(leftPlayerAnimalType, 0, false, 0);
            m_LeftPlayerEntityId = leftEntity.EntityId;
            var spawnPosLeft = Game.Config.FetchConfig<Vector3ConfigEntry>(ConfigNames.PlayerSpawnPositionLeft).value;

            var rightEntity = Game.Entity.InstantiateEntity<TennisPlayerEntity>(rightPlayerAnimalType, 0, false, 0);
            m_RightPlayerEntityId = rightEntity.EntityId;
            var spawnPosRight = Game.Config.FetchConfig<Vector3ConfigEntry>(ConfigNames.PlayerSpawnPositionRight).value;

            leftEntity.transform.position = spawnPosLeft;
            rightEntity.transform.position = spawnPosRight;

            leftEntity.rigidBody.velocity = Vector3.zero;
            rightEntity.rigidBody.velocity = Vector3.zero;

            m_WinScore = Game.Config.FetchConfig<IntConfigEntry>(ConfigNames.GameWinScore).value;
        }

        public override void OnUpdate() {
            if (m_CurrentScoreOffset <= -m_WinScore) {
                //left win
            }

            if (m_CurrentScoreOffset >= m_WinScore) {
                //right win
            }
        }

        public override void OnExit() {
            Game.Entity.DestroyEntity(m_LeftPlayerEntityId);
            Game.Entity.DestroyEntity(m_RightPlayerEntityId);
        }
    }
}