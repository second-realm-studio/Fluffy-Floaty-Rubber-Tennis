using Constants;
using UnityEngine;
using XiheFramework.Core.FSM;
using XiheFramework.Runtime;

namespace Procedures.MainFSM {
    public class GameOverState : State<GameObject> {
        public GameOverState(StateMachine parentStateMachine, GameObject owner) : base(parentStateMachine, owner) { }

        public override void OnEnter() {
            AkSoundEngine.PostEvent("Stop_BGM", Camera.main.gameObject);
            AkSoundEngine.PostEvent("Play_GameOver", Camera.main.gameObject);
            Game.UI.ActivateUI(UINames.GameOver);
        }

        public override void OnUpdate() { }

        public override void OnExit() {
            Game.UI.UnactivateUI(UINames.GameOver);
        }
    }
}