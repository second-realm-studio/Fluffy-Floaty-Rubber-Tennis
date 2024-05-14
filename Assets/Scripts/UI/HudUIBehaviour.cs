using Constants;
using TMPro;
using UnityEngine.UI;
using XiheFramework.Core.UI;
using XiheFramework.Runtime;

namespace UI {
    public class HudUIBehaviour : UIBehaviour {
        public Slider scoreSlider;
        public Slider hitBallSlider;
        public Slider crossNetSlider;

        private string m_OnDataChangeEventHandlerId;

        protected override void OnActive() {
            UpdateScoreSlider();

            m_OnDataChangeEventHandlerId = Game.Event.Subscribe(Game.Blackboard.onDataChangeEventName, OnDataChange);
        }

        private void OnDataChange(object sender, object e) {
            if (sender is not string dataName) {
                return;
            }

            if (dataName == BlackboardDataNames.ScoreOffset || dataName == BlackboardDataNames.HitCount) {
                UpdateScoreSlider();
            }
        }

        void UpdateScoreSlider() {
            var score = Game.Blackboard.GetData<int>(BlackboardDataNames.ScoreOffset);
            scoreSlider.value = score;

            var hitCount = Game.Blackboard.GetData<int>(BlackboardDataNames.HitCount);
            switch (hitCount) {
                case 0:
                    hitBallSlider.value = score;
                    crossNetSlider.value = score;
                    break;
                case 1:
                    hitBallSlider.value = score + 1;
                    crossNetSlider.value = score;
                    break;
                case -1:
                    hitBallSlider.value = score - 1;
                    crossNetSlider.value = score;
                    break;
                case 2:
                    hitBallSlider.value = score + 1;
                    crossNetSlider.value = score + 1;
                    break;
                case -2:
                    hitBallSlider.value = score - 1;
                    crossNetSlider.value = score - 1;
                    break;
            }
        }

        protected override void OnUnActive() {
            Game.Event.Unsubscribe(Game.Blackboard.onDataChangeEventName, m_OnDataChangeEventHandlerId);
        }
    }
}