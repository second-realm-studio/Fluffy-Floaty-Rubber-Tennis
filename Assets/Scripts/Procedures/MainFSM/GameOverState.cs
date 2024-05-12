using UnityEngine;
using XiheFramework.Core.FSM;

namespace Procedures.MainFSM {
    public class GameOverState : State<GameObject> {
        public GameOverState(StateMachine parentStateMachine, GameObject owner) : base(parentStateMachine, owner) { }
        public override void OnEnter() { }

        public override void OnUpdate() { }

        public override void OnExit() {
            AkSoundEngine.PostEvent("Stop_BGM", Camera.main.gameObject);
        }
    }
}