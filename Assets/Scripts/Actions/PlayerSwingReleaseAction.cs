using Constants;
using PlayerEntities;
using UnityEngine;
using XiheFramework.Combat.Action;
using XiheFramework.Runtime;

namespace Actions {
    public class PlayerSwingReleaseAction : TennisPlayerActionEntityBase {
        public override string EntityName => PlayerActionNames.PlayerSwingRelease;

        protected override void OnActionInit() {
            base.OnActionInit();
            //play swing animation

            //shoot raycast to detect if anything is hit
            var colliders = Physics.OverlapSphere(Vector3.zero, 1f);
            foreach (var col in colliders) {
                var deltaDir = col.transform.position - owner.transform.position;

                Physics.Raycast(owner.transform.TransformPoint(owner.capsuleCollider.center), deltaDir, out var hit, owner.swingRadius);
                var rb = col.GetComponent<Rigidbody>();
                if (rb != null) {
                    var swingDir = FetchArgument<Vector2>(ActionArgumentNames.SwingDirection);
                    rb.AddForceAtPosition(hit.point, swingDir, ForceMode.Impulse);
                }

                //if hit, apply force to owner
                Physics.Raycast(hit.point, -deltaDir, out var hitOnOwner, owner.swingRadius);
                owner.rigidBody.AddForceAtPosition(hitOnOwner.point, -deltaDir, ForceMode.Impulse);

                Game.LogicTime.SetGlobalTimeScaleInSecond(0.1f, 1f, true);
            }
        }

        protected override void OnActionUpdate() { }

        protected override void OnActionExit() { }
    }
}