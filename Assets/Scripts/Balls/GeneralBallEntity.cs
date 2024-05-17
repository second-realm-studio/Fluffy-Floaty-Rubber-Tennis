using System;
using Constants;
using UnityEngine;
using XiheFramework.Core.LogicTime;
using XiheFramework.Runtime;

namespace Balls {
    public class GeneralBallEntity : TimeBasedGameEntity {
        public override string EntityGroupName => "BallEntity";
        public override string EntityAddressName => "GeneralBall";

        public float maxSpeedClamp;
        public Rigidbody rigidBody;

#if UNITY_EDITOR
        private void OnValidate() {
            rigidBody = GetComponent<Rigidbody>();
        }
#endif


        public override void OnFixedUpdateCallback() {
            rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, maxSpeedClamp);
            Game.Blackboard.SetData(BlackboardDataNames.BallVelocity, rigidBody.velocity);
        }
    }
}