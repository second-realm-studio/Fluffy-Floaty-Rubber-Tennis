using System;
using System.Collections.Generic;
using Constants;
using PlayerEntities;
using UnityEngine;
using XiheFramework.Core.Utility.Extension;
using XiheFramework.Runtime;

namespace Actions {
    public class PlayerSwingHoldAction : TennisPlayerActionEntityBase {
        public override string EntityAddressName => PlayerActionNames.PlayerSwingHold;
        public float chargeSpeed = 0.5f;

        private float m_SwingPower;
        private static readonly int SwingHolding = Animator.StringToHash("SwingHolding");

        protected override void OnActionInit() {
            base.OnActionInit();

            owner.animator.SetBool(SwingHolding, true);
        }

        protected override void OnActionUpdate() {
            // var chargeSpeed = Game.Config.FetchConfig<float>(ConfigNames.PlayerSwingChargeSpeed);
            m_SwingPower += ScaledDeltaTime * chargeSpeed;
            m_SwingPower = Mathf.Clamp(m_SwingPower, 0, 1f);
            //rotate player hand to face opposite direction of the left joystick input
            var aimDirH = Game.Input(owner.inputId).GetAxis(InputNames.AimHorizontal);
            var aimDirV = Game.Input(owner.inputId).GetAxis(InputNames.AimVertical);
            var aimDir = new Vector2(aimDirH, aimDirV);
            if (aimDir.magnitude > 0.1f) {
                // owner.armRTransform.rotation = Quaternion.LookRotation(Vector3.back, -aimDir.ToVector3(V2ToV3Type.XY));
                owner.armRTransform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.up, -aimDir, Vector3.forward));
            }

            if (Game.Input(owner.inputId).GetButtonDown(InputNames.SwingRelease)) {
                ChangeAction(PlayerActionNames.PlayerSwingRelease,
                    new KeyValuePair<string, object>(ActionArgumentNames.SwingDirection, aimDir.normalized),
                    new KeyValuePair<string, object>(ActionArgumentNames.SwingPower01, m_SwingPower));
            }

            if (!Game.Input(owner.inputId).GetButton(InputNames.SwingHold)) {
                ChangeAction(PlayerActionNames.PlayerIdle);
            }
        }

        protected override void OnActionExit() { }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            //draw sphere cast
            var aimDir = new Vector2(Game.Input(owner.inputId).GetAxis(InputNames.AimHorizontal), Game.Input(owner.inputId).GetAxis(InputNames.AimVertical));
            aimDir.Normalize();
            Gizmos.DrawWireSphere(owner.transform.position - aimDir.ToVector3(V2ToV3Type.XY) * owner.swingRadius / 2, owner.swingRadius / 2);
            Gizmos.DrawLine(owner.transform.position - aimDir.ToVector3(V2ToV3Type.XY) * owner.swingRadius / 2,
                owner.transform.position + aimDir.ToVector3(V2ToV3Type.XY) * owner.swingRadius / 2);
            Gizmos.DrawWireSphere(owner.transform.position + aimDir.ToVector3(V2ToV3Type.XY) * owner.swingRadius / 2, owner.swingRadius / 2);
        }
    }
}