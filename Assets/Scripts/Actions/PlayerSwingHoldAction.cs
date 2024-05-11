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

        private float m_SwingPower;

        protected override void OnActionUpdate() {
            var chargeSpeed = Game.Config.FetchConfig<float>(ConfigNames.PlayerSwingChargeSpeed);
            m_SwingPower += ScaledDeltaTime * chargeSpeed;
            //rotate player hand to face opposite direction of the left joystick input
            var aimDirH = Game.Input(owner.inputId).GetAxis(InputNames.AimHorizontal);
            var aimDirV = Game.Input(owner.inputId).GetAxis(InputNames.AimVertical);
            var aimDir = new Vector2(aimDirH, aimDirV);

            if (Game.Input(owner.inputId).GetButtonDown(InputNames.SwingRelease)) {
                ChangeAction(PlayerActionNames.PlayerSwingRelease,
                    new KeyValuePair<string, object>(ActionArgumentNames.SwingDirection, aimDir.normalized),
                    new KeyValuePair<string, object>(ActionArgumentNames.SwingPower, m_SwingPower));
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