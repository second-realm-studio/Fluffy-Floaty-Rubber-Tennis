using System.Collections.Generic;
using Balls;
using Constants;
using PlayerEntities;
using TMPro;
using UnityEngine;
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

        public RectTransform p1Text;
        public RectTransform p2Text;
        public RectTransform ballText;
        public float ballTextDisplaySpeed = 2f;
        public float yOffset = 10f;
        public float ballYOffset = 5f;

        private uint m_EntityIdP1 = 0;
        private uint m_EntityIdP2 = 0;
        private TennisPlayerEntity m_PlayerEntityP1 = null;
        private TennisPlayerEntity m_PlayerEntityP2 = null;

        private string m_OnDataChangeEventHandlerId;

        protected override void OnActive() {
            UpdateScoreSlider();

            m_OnDataChangeEventHandlerId = Game.Event.Subscribe(Game.Blackboard.onDataChangeEventName, OnDataChange);
        }

        private void Update() {
            UpdatePlayerLabelPositions();
        }

        private void UpdatePlayerLabelPositions() {
            if (m_EntityIdP1 == 0) {
                if (Game.Blackboard.IsDataExisted(BlackboardDataNames.EntityIdP1)) {
                    m_EntityIdP1 = Game.Blackboard.GetData<uint>(BlackboardDataNames.EntityIdP1);
                    m_PlayerEntityP1 = Game.Entity.GetEntity<TennisPlayerEntity>(m_EntityIdP1);
                }
            }
            else {
                if (Game.Entity.IsEntityExisted(m_EntityIdP1)) {
                    p1Text.anchoredPosition = Camera.main.WorldToScreenPoint(m_PlayerEntityP1.transform.position + Vector3.up * yOffset);
                }
            }

            if (m_EntityIdP2 == 0) {
                if (Game.Blackboard.IsDataExisted(BlackboardDataNames.EntityIdP2)) {
                    m_EntityIdP2 = Game.Blackboard.GetData<uint>(BlackboardDataNames.EntityIdP2);
                    m_PlayerEntityP2 = Game.Entity.GetEntity<TennisPlayerEntity>(m_EntityIdP2);
                }
            }
            else {
                if (Game.Entity.IsEntityExisted(m_EntityIdP2)) {
                    p2Text.anchoredPosition = Camera.main.WorldToScreenPoint(m_PlayerEntityP2.transform.position + Vector3.up * yOffset);
                }
            }

            if (Game.Blackboard.IsDataExisted(BlackboardDataNames.EntityIdBall)) {
                if (Game.Blackboard.IsDataExisted(BlackboardDataNames.BallVelocity)) {
                    var ballPos = Game.Entity.GetEntity<GeneralBallEntity>(Game.Blackboard.GetData<uint>(BlackboardDataNames.EntityIdBall)).transform.position;
                    var ballVelocity = Game.Blackboard.GetData<Vector3>(BlackboardDataNames.BallVelocity);
                    if (Vector3.Distance(ballVelocity, Vector3.zero) < ballTextDisplaySpeed) {
                        ballText.gameObject.SetActive(true);
                        ballText.anchoredPosition = Camera.main.WorldToScreenPoint(ballPos + Vector3.up * ballYOffset);
                    }
                    else {
                        ballText.gameObject.SetActive(false);
                    }
                }
                else {
                    ballText.gameObject.SetActive(false);
                }
            }
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