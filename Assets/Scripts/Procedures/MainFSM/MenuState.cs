using UnityEngine;
using XiheFramework.Core.FSM;
using XiheFramework.Runtime;

namespace Procedures.MainFSM {
    public class MenuState : State<GameObject> {
        public MenuState(StateMachine parentStateMachine, GameObject owner) : base(parentStateMachine, owner) { }

        public override void OnEnter() {
            
        }

        public override void OnUpdate() {
            throw new System.NotImplementedException();
        }

        public override void OnExit() {
            throw new System.NotImplementedException();
        }
    }
}