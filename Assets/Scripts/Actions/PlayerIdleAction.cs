using Constants;
using PlayerEntities;
using UnityEngine;
using XiheFramework.Combat.Action;
using XiheFramework.Runtime;

namespace Actions {
    public class PlayerIdleAction : TennisPlayerActionEntityBase {
        public override string EntityAddressName => PlayerActionNames.PlayerIdle;

        protected override void OnActionUpdate() {
            if (Game.Input(owner.inputId).GetButtonDown(InputNames.SwingHold)) {
                ChangeAction(PlayerActionNames.PlayerSwingHold);
            }
        }

        protected override void OnActionExit() { }
    }
}