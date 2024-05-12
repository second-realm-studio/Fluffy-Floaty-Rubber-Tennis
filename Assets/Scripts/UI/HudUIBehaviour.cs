using Constants;
using TMPro;
using UnityEngine.UI;
using XiheFramework.Core.UI;
using XiheFramework.Runtime;

namespace UI {
    public class HudUIBehaviour : UIBehaviour {
        public Slider scoreSlider;

        private string m_OnDataChangeEventHandlerId;

        protected override void OnActive() {
            UpdateScoreSlider();

            m_OnDataChangeEventHandlerId = Game.Event.Subscribe(Game.Blackboard.OnDataChangeEventName, OnDataChange);
        }

        private void OnDataChange(object sender, object e) {
            if (sender is not string dataName) {
                return;
            }

            if (dataName == BlackboardDataNames.ScoreOffset) {
                UpdateScoreSlider();
            }
        }

        void UpdateScoreSlider() {
            var score = Game.Blackboard.GetData<int>(BlackboardDataNames.ScoreOffset);
            scoreSlider.value = score;
        }

        protected override void OnUnActive() {
            Game.Event.Unsubscribe(Game.Blackboard.OnDataChangeEventName, m_OnDataChangeEventHandlerId);
        }
    }
}