using UnityEngine;
using XiheFramework.Core.Console;
using XiheFramework.Core.Entity;
using XiheFramework.Runtime;

namespace DevCommands {
    public class ReadyCommand : IDevConsoleCommand {
        public bool Execute(string[] args) {
            if (args.Length>0) {
                return false;
            }
            
            Game.Event.Invoke("Selection.BothReady", null);

            return true;
        }
    }
}