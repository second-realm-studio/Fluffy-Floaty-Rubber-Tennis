using Constants;
using PlayerEntities;
using XiheFramework.Combat.Action;
using XiheFramework.Runtime;

namespace Actions {
    public class PlayerIdleAction : TennisPlayerActionEntityBase {
        public override string EntityName => PlayerActionNames.PlayerIdle;

        protected override void OnActionUpdate() {
            if (Game.Input(owner.inputPlayerId).GetButtonDown(InputNames.SwingHold)) {
                Game.Action.ChangeAction(EntityId, PlayerActionNames.PlayerSwingHold);
            }
        }

        protected override void OnActionExit() { }
    }
}