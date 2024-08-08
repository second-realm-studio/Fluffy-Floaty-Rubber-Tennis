using Constants;
using UnityEngine;
using UnityEngine.SceneManagement;
using XiheFramework.Core.FSM;
using XiheFramework.Runtime;

namespace Procedures.MainFSM {
    public class MenuState : State<GameObject> {
        private string m_OnStartBtnClickedEventHandlerId;
        private string m_OnExitBtnClickedEventHandlerId;
        public MenuState(StateMachine parentStateMachine, GameObject owner) : base(parentStateMachine, owner) { }

        public override void OnEnter() {
            Game.UI.ActivateUI(UINames.Menu);

            AkSoundEngine.PostEvent("Play_BGM", Camera.main.gameObject);
            AkSoundEngine.SetState("BGM", "Menu");

            m_OnStartBtnClickedEventHandlerId = Game.Event.Subscribe(EventNames.OnStartBtnClicked, OnStartBtnClicked);
            m_OnExitBtnClickedEventHandlerId = Game.Event.Subscribe(EventNames.OnExitBtnClicked, OnExitBtnClicked);
        }


        public override void OnUpdate() { }

        public override void OnExit() {
            Game.Event.Unsubscribe(EventNames.OnStartBtnClicked, m_OnStartBtnClickedEventHandlerId);
            Game.Event.Unsubscribe(EventNames.OnExitBtnClicked, m_OnExitBtnClickedEventHandlerId);
        }

        private void OnStartBtnClicked(object sender, object e) {
            Game.UI.UnactivateUI(UINames.Menu);
            Game.Scene.LoadSceneAsync(SceneNames.Tutorial, LoadSceneMode.Single, true, (scene) => { ChangeState(GameLoopStatesNames.Tutorial); });
        }

        private void OnExitBtnClicked(object sender, object e) {
            Application.Quit();
        }
    }
}