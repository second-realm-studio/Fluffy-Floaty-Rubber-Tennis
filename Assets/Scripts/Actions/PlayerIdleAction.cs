using Constants;
using XiheFramework.Combat.Action;
using XiheFramework.Runtime;

namespace Actions {
    public class PlayerIdleAction : ActionEntity {
        public override string EntityName => ActionNames.PlayerIdle;

        protected override void OnActionInit() {
            
        }

        protected override void OnActionUpdate() {
            // if (Game.Input(0)) {
            //     
            // }
        }

        protected override void OnActionExit() { }
    }
}