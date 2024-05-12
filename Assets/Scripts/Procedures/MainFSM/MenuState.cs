using UnityEngine;
using XiheFramework.Core.FSM;
using XiheFramework.Runtime;

namespace Procedures.MainFSM {
    public class MenuState : State<GameObject> {
        public MenuState(StateMachine parentStateMachine, GameObject owner) : base(parentStateMachine, owner) { }

        public override void OnEnter() {
            AkSoundEngine.SetState("BGM", "Menu");
        }

        public override void OnUpdate() { }

        public override void OnExit() { }
    }
}