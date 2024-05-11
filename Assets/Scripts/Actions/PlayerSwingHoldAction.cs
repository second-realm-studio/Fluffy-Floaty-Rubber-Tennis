using System.Collections.Generic;
using Constants;
using PlayerEntities;
using UnityEngine;
using XiheFramework.Runtime;

namespace Actions {
    public class PlayerSwingHoldAction : TennisPlayerActionEntityBase {
        public override string EntityAddressName => PlayerActionNames.PlayerSwingHold;

        private float m_SwingPower;

        protected override void OnActionUpdate() {
            var chargeSpeed = Game.Config.FetchConfig<float>(ConfigNames.PlayerSwingChargeSpeed);
            m_SwingPower += ScaledDeltaTime * chargeSpeed;
            //rotate player hand to face opposite direction of the left joystick input
            var aimDirH = Game.Input(owner.inputPlayerId).GetAxis(InputNames.AimHorizontal);
            var aimDirV = Game.Input(owner.inputPlayerId).GetAxis(InputNames.AimVertical);
            var aimDir = new Vector2(aimDirH, aimDirV);

            if (Game.Input(owner.inputPlayerId).GetButtonDown(InputNames.SwingRelease)) {
                Game.Action.ChangeAction(EntityId, PlayerActionNames.PlayerSwingRelease,
                    new KeyValuePair<string, object>(ActionArgumentNames.SwingDirection, aimDir),
                    new KeyValuePair<string, object>(ActionArgumentNames.SwingPower, m_SwingPower));
            }
        }

        protected override void OnActionExit() { }
    }
}