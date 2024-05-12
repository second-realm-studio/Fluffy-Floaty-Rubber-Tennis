using Constants;
using TMPro;
using UnityEngine;
using XiheFramework.Core.UI;
using XiheFramework.Runtime;

namespace UI {
    public class SelectionUIBehaviour : UIBehaviour {
        // public TextMeshProUGUI p1SelectedAnimal;
        // public TextMeshProUGUI p2SelectedAnimal;
        public float scrollSpeed = 1f;
        private float m_P1SelectedAnimalId;
        private float m_P2SelectedAnimalId;
        private AnimalType m_P1SelectedAnimalType;
        private AnimalType m_P2SelectedAnimalType;

        private float m_P1HorizontalCached;
        private float m_P2HorizontalCached;


        private float m_HoldBufferP1;
        private float m_HoldBufferP2;
        protected override void OnActive() { }

        private void Update() {
            // UpdateHoldBuffer();

            var p1Horizontal = Game.Input(0).GetAxis(InputNames.UIHorizontal);
            var p2Horizontal = Game.Input(1).GetAxis(InputNames.UIHorizontal);

            if ((p1Horizontal - m_P1HorizontalCached) > 0.4f) {
                m_P1SelectedAnimalId += 1;
            }

            if ((p2Horizontal - m_P2HorizontalCached) > 0.4f) {
                m_P2SelectedAnimalId += 1;
            }

            if ((p1Horizontal - m_P1HorizontalCached) < -0.4f) {
                m_P1SelectedAnimalId -= 1;
            }

            if ((p2Horizontal - m_P2HorizontalCached) < -0.4f) {
                m_P2SelectedAnimalId -= 1;
            }

            m_P1SelectedAnimalId += Game.Input(0).GetAxis(InputNames.UIHorizontal) * Time.deltaTime * scrollSpeed;
            m_P2SelectedAnimalId += Game.Input(1).GetAxis(InputNames.UIHorizontal) * Time.deltaTime * scrollSpeed;

            m_P1SelectedAnimalId = Mathf.Clamp(m_P1SelectedAnimalId, 0, 5);
            m_P2SelectedAnimalId = Mathf.Clamp(m_P2SelectedAnimalId, 0, 5);

            m_P1SelectedAnimalType = (AnimalType)Mathf.FloorToInt(m_P1SelectedAnimalId);
            m_P2SelectedAnimalType = (AnimalType)Mathf.FloorToInt(m_P2SelectedAnimalId);
            //
            // p1SelectedAnimal.text = m_P1SelectedAnimalType.ToString() + "\n" + m_P1SelectedAnimalId;
            // p2SelectedAnimal.text = m_P2SelectedAnimalType.ToString() + "\n" + m_P2SelectedAnimalId;

            m_P1HorizontalCached = p1Horizontal;
            m_P2HorizontalCached = p2Horizontal;
        }

        protected override void OnUnActive() { }

        void UpdateHoldBuffer() {
            var leftUIHorizontal = Game.Input(0).GetAxis(InputNames.UIHorizontal);
            if (Mathf.Abs(leftUIHorizontal) > 0.5f) {
                m_HoldBufferP1 += Time.deltaTime;
            }
            else {
                m_HoldBufferP1 -= Time.deltaTime * 3f;
            }

            var rightUIHorizontal = Game.Input(1).GetAxis(InputNames.UIHorizontal);
            if (Mathf.Abs(rightUIHorizontal) > 0.5f) {
                m_HoldBufferP2 += Time.deltaTime;
            }
            else {
                m_HoldBufferP2 -= Time.deltaTime * 3f;
            }
        }
    }
}