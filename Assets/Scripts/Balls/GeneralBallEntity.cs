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
        public bool crossedNetSinceLastHit;
        public Rigidbody rigidBody;

        public override void OnInitCallback() {
            base.OnInitCallback();
            crossedNetSinceLastHit = false;

            Game.Event.Subscribe(EventNames.OnBallHit, OnBallHit);
            Game.Event.Subscribe(EventNames.OnCourtChange, OnCourtChange);
        }

#if UNITY_EDITOR
        private void OnValidate() {
            rigidBody = GetComponent<Rigidbody>();
        }
#endif


        public override void OnFixedUpdateCallback() {
            rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, maxSpeedClamp);
        }

        private void OnCourtChange(object sender, object e) {
            if (sender is not bool rightSide) return;
            crossedNetSinceLastHit = true;
        }

        private void OnBallHit(object sender, object e) {
            if (sender is not uint hitterId) {
                return;
            }

            if (crossedNetSinceLastHit) {
                Game.Event.Invoke(EventNames.OnBallAddHitCount, hitterId, null);
                crossedNetSinceLastHit = false;
            }
        }
    }
}