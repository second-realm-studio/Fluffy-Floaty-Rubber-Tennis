using UnityEngine;
using XiheFramework.Core.FSM;

namespace Procedures.MainFSM {
    public class SelectionState : State<GameObject> {
        public SelectionState(StateMachine parentStateMachine, GameObject owner) : base(parentStateMachine, owner) { }
        public override void OnEnter() { }

        public override void OnUpdate() { }

        public override void OnExit() { }
    }
}