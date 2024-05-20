using Constants;
using UnityEngine;
using UnityEngine.Serialization;
using XiheFramework.Core.Entity;

namespace AnimalDisplayEntity {
    public class AnimalDisplayEntity : GameEntity {
        public override string EntityGroupName => "AnimalDisplayEntity";
        public override string EntityAddressName => animalType.ToString();

        public AnimalType animalType;
        
        private Animator m_Animator;

        public override void OnInitCallback() {
            base.OnInitCallback();

            transform.position = new Vector3((float)(int)animalType, -0.5f, 58);
            m_Animator = GetComponentInChildren<Animator>();
        }

        public void OnSelected() {
            if (m_Animator != null) {
                m_Animator.Play("Selected");
            }
        }
    }
}