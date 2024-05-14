using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XiheFramework.Core.UI;
using XiheFramework.Runtime;

namespace UI {
    public class SelectionUIBehaviour : UIBehaviour {
        // public TextMeshProUGUI p1SelectedAnimal;
        // public TextMeshProUGUI p2SelectedAnimal;
        public Image countdownImageP1;
        public Image countdownImageP2;
        public Sprite countdown3;
        public Sprite countdown2;
        public Sprite countdown1;
        public Sprite countdownReady;

        public Slider p1PowerSlider;
        public Slider p1RangeSlider;
        public Slider p1WeightSlider;

        public Slider p2PowerSlider;
        public Slider p2RangeSlider;
        public Slider p2WeightSlider;

        public float scrollSpeed = 1f;
        private float m_P1SelectedAnimalId;
        private float m_P2SelectedAnimalId;
        private AnimalType m_P1SelectedAnimalType;
        private AnimalType m_P2SelectedAnimalType;

        private bool m_P1Ready;
        private bool m_P2Ready;

        private Camera m_CameraP1;
        private Camera m_CameraP2;

        private bool m_BothSideReady = false;

        private List<uint> m_DisplayEntityIds = new List<uint>();

        protected override void OnActive() {
            m_P1SelectedAnimalId = 0;
            m_P2SelectedAnimalId = 0;
            m_P1Ready = false;
            m_P2Ready = false;
            m_BothSideReady = false;

            m_CameraP1 = GameObject.Find("SelectionCameraP1").GetComponent<Camera>();
            m_CameraP2 = GameObject.Find("SelectionCameraP2").GetComponent<Camera>();

            var entity = Game.Entity.InstantiateEntity<AnimalDisplayEntity.AnimalDisplayEntity>($"AnimalDisplayEntity_{AnimalType.Sheep}");
            m_DisplayEntityIds.Add(entity.EntityId);

            entity = Game.Entity.InstantiateEntity<AnimalDisplayEntity.AnimalDisplayEntity>($"AnimalDisplayEntity_{AnimalType.Cat}");
            m_DisplayEntityIds.Add(entity.EntityId);

            entity = Game.Entity.InstantiateEntity<AnimalDisplayEntity.AnimalDisplayEntity>($"AnimalDisplayEntity_{AnimalType.Goat}");
            m_DisplayEntityIds.Add(entity.EntityId);

            entity = Game.Entity.InstantiateEntity<AnimalDisplayEntity.AnimalDisplayEntity>($"AnimalDisplayEntity_{AnimalType.Dog}");
            m_DisplayEntityIds.Add(entity.EntityId);

            entity = Game.Entity.InstantiateEntity<AnimalDisplayEntity.AnimalDisplayEntity>($"AnimalDisplayEntity_{AnimalType.Rabbit}");
            m_DisplayEntityIds.Add(entity.EntityId);

            entity = Game.Entity.InstantiateEntity<AnimalDisplayEntity.AnimalDisplayEntity>($"AnimalDisplayEntity_{AnimalType.Mouse}");
            m_DisplayEntityIds.Add(entity.EntityId);
        }

        private void Update() {
            UpdateSelectedTypes();

            UpdateCamera();

            UpdateStats();

            if (!m_P1Ready) {
                if (Game.Input(0).GetButtonDown(InputNames.UISubmit)) {
                    Game.Blackboard.SetData(BlackboardDataNames.PlayerCharacterType(0), m_P1SelectedAnimalType);
                    m_P1Ready = true;
                    countdownImageP1.enabled = true;
                    countdownImageP1.sprite = countdownReady;
                }
            }

            if (!m_P2Ready) {
                if (Game.Input(1).GetButtonDown(InputNames.UISubmit)) {
                    Game.Blackboard.SetData(BlackboardDataNames.PlayerCharacterType(1), m_P2SelectedAnimalType);
                    m_P2Ready = true;
                    countdownImageP2.enabled = true;
                    countdownImageP2.sprite = countdownReady;
                }
            }

            if (m_P1Ready) {
                if (Game.Input(0).GetButtonDown(InputNames.UICancel)) {
                    m_P1Ready = false;
                    countdownImageP1.enabled = false;
                    countdownImageP1.sprite = null;
                }
            }

            if (m_P2Ready) {
                if (Game.Input(1).GetButtonDown(InputNames.UICancel)) {
                    m_P2Ready = false;
                    countdownImageP2.enabled = false;
                    countdownImageP2.sprite = null;
                }
            }

            if (!m_BothSideReady) {
                if (m_P1Ready && m_P2Ready) {
                    StartCoroutine(CountdownCo());
                    m_BothSideReady = true;
                }
            }
        }

        private void UpdateStats() {
            switch (m_P1SelectedAnimalType) {
                case AnimalType.Sheep:
                    p1PowerSlider.value = 3;
                    p1RangeSlider.value = 3;
                    p1WeightSlider.value = 1;
                    break;
                case AnimalType.Cat:
                    p1PowerSlider.value = 4;
                    p1RangeSlider.value = 2;
                    p1WeightSlider.value = 2;
                    break;
                case AnimalType.Goat:
                    p1PowerSlider.value = 5;
                    p1RangeSlider.value = 2;
                    p1WeightSlider.value = 4;
                    p1WeightSlider.value = 5;
                    break;
                case AnimalType.Dog:
                    p1PowerSlider.value = 4;
                    p1RangeSlider.value = 3;
                    p1WeightSlider.value = 3;
                    break;
                case AnimalType.Rabbit:
                    p1PowerSlider.value = 3;
                    p1RangeSlider.value = 3;
                    p1WeightSlider.value = 1;
                    break;
                case AnimalType.Mouse:
                    p1PowerSlider.value = 2;
                    p1RangeSlider.value = 5;
                    p1WeightSlider.value = 0;
                    break;
                default:
                    p1PowerSlider.value = 0;
                    p1RangeSlider.value = 0;
                    p1WeightSlider.value = 0;
                    break;
            }

            switch (m_P2SelectedAnimalType) {
                case AnimalType.Sheep:
                    p2PowerSlider.value = 3;
                    p2RangeSlider.value = 3;
                    p2WeightSlider.value = 1;
                    break;
                case AnimalType.Cat:
                    p2PowerSlider.value = 4;
                    p2RangeSlider.value = 2;
                    p2WeightSlider.value = 2;
                    break;
                case AnimalType.Goat:
                    p2PowerSlider.value = 5;
                    p2RangeSlider.value = 2;
                    p2WeightSlider.value = 4;
                    break;
                case AnimalType.Dog:
                    p2PowerSlider.value = 4;
                    p2RangeSlider.value = 3;
                    p2WeightSlider.value = 3;
                    break;
                case AnimalType.Rabbit:
                    p2PowerSlider.value = 3;
                    p2RangeSlider.value = 3;
                    p2WeightSlider.value = 1;
                    break;
                case AnimalType.Mouse:
                    p2PowerSlider.value = 2;
                    p2RangeSlider.value = 5;
                    p2WeightSlider.value = 0;
                    break;
                default:
                    p2PowerSlider.value = 0;
                    p2RangeSlider.value = 0;
                    p2WeightSlider.value = 0;
                    break;
            }
        }

        private IEnumerator CountdownCo() {
            countdownImageP1.enabled = true;
            countdownImageP2.enabled = true;

            countdownImageP1.sprite = countdown3;
            countdownImageP2.sprite = countdown3;
            yield return new WaitForSeconds(1);
            countdownImageP1.sprite = countdown2;
            countdownImageP2.sprite = countdown2;
            yield return new WaitForSeconds(1);
            countdownImageP1.sprite = countdown1;
            countdownImageP2.sprite = countdown1;
            yield return new WaitForSeconds(1);
            Game.Event.Invoke(EventNames.OnBothSideReady);
        }

        private void UpdateCamera() {
            m_CameraP1.transform.position = Vector3.Lerp(m_CameraP1.transform.position,
                new Vector3((float)(int)m_P1SelectedAnimalType, m_CameraP2.transform.position.y, m_CameraP2.transform.position.z), Time.deltaTime * 5);
            m_CameraP2.transform.position = Vector3.Lerp(m_CameraP2.transform.position,
                new Vector3((float)(int)m_P2SelectedAnimalType, m_CameraP2.transform.position.y, m_CameraP2.transform.position.z), Time.deltaTime * 5);
        }

        protected override void OnUnActive() {
            foreach (var entityId in m_DisplayEntityIds) {
                Game.Entity.DestroyEntity(entityId);
            }

            m_CameraP1 = null;
            m_CameraP2 = null;

            m_DisplayEntityIds.Clear();
        }


        void UpdateSelectedTypes() {
            // Player 1 selection logic
            if (!m_P1Ready) {
                if (Game.Input(0).GetButtonDown(InputNames.UIRight)) {
                    Debug.Log("Right");
                    m_P1SelectedAnimalId++;
                }
                else if (Game.Input(0).GetButtonDown(InputNames.UILeft)) {
                    m_P1SelectedAnimalId--;
                }
                
                m_P1SelectedAnimalId = Mathf.Clamp(m_P1SelectedAnimalId, 0, 5);
                m_P1SelectedAnimalType = (AnimalType)Mathf.FloorToInt(m_P1SelectedAnimalId);
            }

            // Player 2 selection logic
            if (!m_P2Ready) {
                if (Game.Input(1).GetButtonDown(InputNames.UIRight)) {
                    m_P2SelectedAnimalId++;
                }
                else if (Game.Input(1).GetButtonDown(InputNames.UILeft)) {
                    m_P2SelectedAnimalId--;
                }

                m_P2SelectedAnimalId = Mathf.Clamp(m_P2SelectedAnimalId, 0, 5);
                m_P2SelectedAnimalType = (AnimalType)Mathf.FloorToInt(m_P2SelectedAnimalId);
            }
        }
    }
}