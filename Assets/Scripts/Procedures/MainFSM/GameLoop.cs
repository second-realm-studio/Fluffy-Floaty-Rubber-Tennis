using System;
using Constants;
using PlayerEntities;
using UnityEngine;
using XiheFramework.Runtime;

namespace Procedures.MainFSM {
    public class GameLoop :MonoBehaviour{
        private void Start() {
            var leftPlayerAnimalType = Game.Blackboard.GetData<string>(BlackboardDataNames.PlayerCharacterType(0));
            leftPlayerAnimalType = $"{nameof(TennisPlayerEntity)}_{leftPlayerAnimalType}";
            var rightPlayerAnimalType = Game.Blackboard.GetData<string>(BlackboardDataNames.PlayerCharacterType(1));
            rightPlayerAnimalType = $"{nameof(TennisPlayerEntity)}_{rightPlayerAnimalType}";

            var leftEntity = Game.Entity.InstantiateEntity<TennisPlayerEntity>(leftPlayerAnimalType, 0, false, 0);
            var spawnPosLeft = Game.Config.FetchConfig<Vector3ConfigEntry>(ConfigNames.PlayerSpawnPositionLeft).value;

            var rightEntity = Game.Entity.InstantiateEntity<TennisPlayerEntity>(rightPlayerAnimalType, 0, false, 0);
            var spawnPosRight = Game.Config.FetchConfig<Vector3ConfigEntry>(ConfigNames.PlayerSpawnPositionRight).value;

            leftEntity.transform.position = spawnPosLeft;
            rightEntity.transform.position = spawnPosRight;

            leftEntity.rigidBody.velocity = Vector3.zero;
            rightEntity.rigidBody.velocity = Vector3.zero;
        }
    }
}