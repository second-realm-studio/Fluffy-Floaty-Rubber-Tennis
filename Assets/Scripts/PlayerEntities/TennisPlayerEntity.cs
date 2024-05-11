using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using UnityEngine.Serialization;
using XiheFramework.Combat.Base;
using XiheFramework.Core.LogicTime;
using XiheFramework.Runtime;

namespace PlayerEntities {
    public class TennisPlayerEntity : TimeBasedGameEntity {
        public override string EntityAddressName => entityName;

        public string entityName;
        public int inputPlayerId;
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
            Game.Action.ChangeAction(EntityId, PlayerActionNames.PlayerIdle);
        }
    }
}