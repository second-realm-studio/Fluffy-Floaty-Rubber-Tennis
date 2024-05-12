using Constants;
using UnityEngine;
using XiheFramework.Core.FSM;
using XiheFramework.Runtime;

namespace Procedures.MainFSM {
    public class SelectionState : State<GameObject> {
        private string m_OnBothSideReadyEventHandlerId;
        public SelectionState(StateMachine parentStateMachine, GameObject owner) : base(parentStateMachine, owner) { }

        public override void OnEnter() {
            Game.UI.ActivateUI(UINames.Selection);
            m_OnBothSideReadyEventHandlerId = Game.Event.Subscribe(EventNames.OnBothSideReady, OnBothSideReady);
        }

        public override void OnUpdate() { }

        public override void OnExit() {
            Game.Event.Unsubscribe(EventNames.OnBothSideReady, m_OnBothSideReadyEventHandlerId);
            Game.UI.UnactivateUI(UINames.Selection);
        }

        private void OnBothSideReady(object sender, object e) {
            ChangeState(GameLoopStatesNames.Game);
        }
    }
}