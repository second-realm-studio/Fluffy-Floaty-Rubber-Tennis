using DevConsole;
using XiheFramework.Runtime;

namespace Commands {
    public class PauseCommand : IDevConsoleCommand {
        public bool Execute(string[] args) {
            if (args.Length == 0 || args.Length > 1) {
                return false;
            }

            if (args[0] == "on") {
                Game.LogicTime.SetGlobalTimeScalePermanent(0f);
            }
            else if (args[0] == "off") {
                Game.LogicTime.SetGlobalTimeScalePermanent(Game.LogicTime.defaultTimeScale);
            }
            else {
                return false;
            }

            return true;
        }
    }
}