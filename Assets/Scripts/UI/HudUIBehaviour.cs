using System.Collections.Generic;
using Constants;
using TMPro;
using UnityEngine.UI;
using XiheFramework.Core.UI;
using XiheFramework.Runtime;

namespace UI {
    public class HudUIBehaviour : UIBehaviour {
        // public Slider scoreSlider;
        // public Slider hitBallSlider;
        // public Slider crossNetSlider;

        public List<Image> hitCountLeftImages;
        public List<Image> hitCountRightImages;

        private string m_OnDataChangeEventHandlerId;

        protected override void OnActive() {
            UpdateScoreSlider();

            m_OnDataChangeEventHandlerId = Game.Event.Subscribe(Game.Blackboard.onDataChangeEventName, OnDataChange);
        }

        private void OnDataChange(object sender, object e) {
            if (sender is not string dataName) {
                return;
            }

            if (dataName == BlackboardDataNames.HitCountP1 || dataName == BlackboardDataNames.HitCountP2) {
                UpdateScoreSlider();
            }
        }

        void UpdateScoreSlider() {
            var hitCountLeft = Game.Blackboard.GetData<int>(BlackboardDataNames.HitCountP1);
            var hitCountRight = Game.Blackboard.GetData<int>(BlackboardDataNames.HitCountP2);

            for (var i = 0; i < hitCountLeftImages.Count; i++) {
                hitCountLeftImages[i].enabled = i < hitCountLeft;
            }

            for (var i = 0; i < hitCountRightImages.Count; i++) {
                hitCountRightImages[i].enabled = i < hitCountRight;
            }
        }

        protected override void OnUnActive() {
            Game.Event.Unsubscribe(Game.Blackboard.onDataChangeEventName, m_OnDataChangeEventHandlerId);
        }
    }
}