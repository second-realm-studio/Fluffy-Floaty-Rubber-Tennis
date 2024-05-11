using UnityEngine;
using XiheFramework.Core.Console;
using XiheFramework.Core.Entity;
using XiheFramework.Runtime;

namespace DevCommands {
    public class SpawnCommand : IDevConsoleCommand {
        public bool Execute(string[] args) {
            if (args.Length == 0) {
                Debug.LogError("Specify entity address");
                return false;
            }

            GameEntity entity = null;
            if (args.Length >= 1) {
                var address = args[0];
                entity = Game.Entity.InstantiateEntity<GameEntity>(address);
            }

            if (entity != null && args.Length >= 4) {
                entity.transform.position = new Vector3(float.Parse(args[1]), float.Parse(args[2]), float.Parse(args[3]));
            }

            return true;
        }
    }
}