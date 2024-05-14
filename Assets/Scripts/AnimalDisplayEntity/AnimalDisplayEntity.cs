using Constants;
using UnityEngine;
using UnityEngine.Serialization;
using XiheFramework.Core.Entity;

namespace AnimalDisplayEntity {
    public class AnimalDisplayEntity : GameEntity {
        public override string EntityGroupName => "AnimalDisplayEntity";
        public override string EntityAddressName => animalType.ToString();

        public AnimalType animalType;
        public Animator animator;

        public override void OnInitCallback() {
            base.OnInitCallback();

            transform.position = new Vector3((float)(int)animalType, -0.5f, 58);
            animator = GetComponentInChildren<Animator>();
        }

        public void OnSelected() {
            if (animator != null) {
                animator.Play("Selected");
            }
        }
    }
}