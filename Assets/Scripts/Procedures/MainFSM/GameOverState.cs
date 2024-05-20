using Constants;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        public override void OnUpdate() {
            if (Game.Input(0).GetButtonDown(InputNames.UISubmit) || Game.Input(1).GetButtonDown(InputNames.UISubmit)) {
                Game.Scene.LoadSceneAsync(SceneNames.Menu, LoadSceneMode.Single, true, (scene) => { ChangeState(GameLoopStatesNames.Menu); });
            }
        }

        public override void OnExit() {
            Game.Blackboard.RemoveData(BlackboardDataNames.WinnerName);
            Game.Blackboard.RemoveData(BlackboardDataNames.PlayerCharacterType(0));
            Game.Blackboard.RemoveData(BlackboardDataNames.PlayerCharacterType(1));
            Game.UI.UnactivateUI(UINames.GameOver);
        }
    }
}