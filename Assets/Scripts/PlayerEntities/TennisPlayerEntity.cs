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
        public override string EntityName => entityName;

        public string entityName;
        public int inputPlayerId;
        public float weight;
        public float airDrag;
        public float swingRadius;
        public float speed;

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

            rigidBody.mass = weight;
            rigidBody.drag = airDrag;
            Game.Action.ChangeAction(EntityId, PlayerActionNames.PlayerIdle);
        }
    }
}