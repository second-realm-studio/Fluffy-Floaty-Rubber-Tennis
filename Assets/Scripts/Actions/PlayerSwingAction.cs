using Constants;
using XiheFramework.Combat.Action;
using XiheFramework.Runtime;

namespace Actions {
    public class PlayerSwingAction : ActionEntity {
        public override string EntityName => ActionNames.PlayerSwing;

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