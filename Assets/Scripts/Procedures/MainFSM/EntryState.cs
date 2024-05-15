using Constants;
using UnityEngine;
using UnityEngine.SceneManagement;
using XiheFramework.Core.FSM;
using XiheFramework.Runtime;

namespace Procedures.MainFSM {
    public class EntryState : State<GameObject> {
        public EntryState(StateMachine parentStateMachine, GameObject owner) : base(parentStateMachine, owner) { }

        public override void OnEnter() {
            Game.Scene.LoadSceneAsync(SceneNames.Menu, LoadSceneMode.Single, true, (handle) => { ChangeState(GameLoopStatesNames.Menu); });
            AkSoundEngine.SetState("BGM", "None");
        }

        public override void OnUpdate() { }

        public override void OnExit() { }
    }
}