using Constants;
using UnityEngine;
using XiheFramework.Core.UI;
using XiheFramework.Runtime;

namespace UI {
    public class GameOverUIBehaviour : UIBehaviour {
        public GameObject leftWin;
        public GameObject rightWin;
        public GameObject leftLose;
        public GameObject rightLose;

        protected override void OnActive() {
            var winner = Game.Blackboard.GetData<string>(BlackboardDataNames.WinnerName);
            if (winner == "Left") {
                leftWin.SetActive(true);
                leftLose.SetActive(false);
                rightLose.SetActive(true);
                rightWin.SetActive(false);
            }

            if (winner == "Right") {
                rightWin.SetActive(true);
                rightLose.SetActive(false);
                leftLose.SetActive(true);
                leftWin.SetActive(false);
            }
        }
    }
}