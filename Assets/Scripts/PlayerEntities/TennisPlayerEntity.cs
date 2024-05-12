using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using UnityEngine.Serialization;
using XiheFramework.Combat.Base;
using XiheFramework.Core.LogicTime;
using XiheFramework.Core.Utility.Extension;
using XiheFramework.Runtime;

namespace PlayerEntities {
    public class TennisPlayerEntity : TimeBasedGameEntity {
        public override string EntityGroupName => "TennisPlayerEntity";
        public override string EntityAddressName => animalType.ToString();

        public AnimalType animalType;
        public bool isRightSide;
        public int inputId; //rewired player id
        public float power;
        public float mass;
        public float airDrag;
        public float swingRadius;

        public Animator animator;
        public CapsuleCollider capsuleCollider;
        public Rigidbody rigidBody;

#if UNITY_EDITOR
        private void OnValidate() {
            if (animator == null) {
                animator = GetComponentInChildren<Animator>();
            }
        }
#endif

        public override void OnInitCallback() {
            base.OnInitCallback();

            rigidBody.mass = mass;
            rigidBody.drag = airDrag;
            rigidBody.angularDrag = airDrag * 5f;
            Game.Action.ChangeAction(EntityId, PlayerActionNames.PlayerIdle);
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            var aimDir = new Vector2(Game.Input(inputId).GetAxis(InputNames.AimHorizontal), Game.Input(inputId).GetAxis(InputNames.AimVertical));
            Gizmos.DrawLine(transform.position, transform.position + aimDir.ToVector3(V2ToV3Type.XY) * 2f);
        }
    }
}