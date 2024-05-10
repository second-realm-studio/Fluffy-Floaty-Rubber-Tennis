using System;
using XiheFramework.Core;
using XiheFramework.Runtime;

namespace DevConsole.Commands {
    public class HUDCommand : IDevConsoleCommand {
        public bool Execute(string[] args) {
            if (args.Length == 0) {
                return false;
            }

            var arg0 = args[0].ToLower();
            if (arg0 == "on") {
                Game.UI.ActivateUI("HUD");
                return true;
            }
            else if (arg0 == "off") {
                Game.UI.UnactivateUI("HUD");
                return true;
            }
            else {
                return false;
            }
        }
    }
}