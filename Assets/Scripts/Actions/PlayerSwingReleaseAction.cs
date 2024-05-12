using System;
using Constants;
using PlayerEntities;
using UnityEngine;
using XiheFramework.Combat.Action;
using XiheFramework.Core.Utility.Extension;
using XiheFramework.Runtime;

namespace Actions {
    public class PlayerSwingReleaseAction : TennisPlayerActionEntityBase {
        public override string EntityAddressName => PlayerActionNames.PlayerSwingRelease;

        public Material hitMaterial;
        public LayerMask hitLayerMask;

        public AK.Wwise.Event swingSound;
        public AK.Wwise.Event hitSound;

        protected override void OnActionInit() {
            base.OnActionInit();
            swingSound.Post(owner.gameObject);

            var swingDir = FetchArgument<Vector2>(ActionArgumentNames.SwingDirection);
            var swingPower = FetchArgument<float>(ActionArgumentNames.SwingPower);

            var isHit = Physics.SphereCast(owner.transform.position - swingDir.ToVector3(V2ToV3Type.XY) * owner.swingRadius / 2, owner.swingRadius / 2,
                swingDir.ToVector3(V2ToV3Type.XY), out var sphereHit,
                owner.swingRadius, hitLayerMask);

            // var isHit = Physics.Raycast(owner.transform.position, swingDir.ToVector3(V2ToV3Type.XY), out var sphereHit, owner.swingRadius, hitLayerMask);
            if (isHit) {
                var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.transform.position = sphereHit.point;
                go.transform.localScale = Vector3.one * 0.1f;
                go.GetComponent<Renderer>().material = hitMaterial;
                Destroy(go.gameObject, 1f);

                var deltaDir = sphereHit.point - owner.transform.position;
                var distance = Vector3.Distance(owner.transform.position, sphereHit.point);
                distance = Mathf.Clamp(distance, 0, owner.swingRadius);
                var force = swingDir.ToVector3(V2ToV3Type.XY) * owner.power;

                var rb = sphereHit.collider.GetComponent<Rigidbody>();
                if (rb != null) {
                    //replace 0 to distance
                    hitSound.Post(owner.gameObject);
                    var originForce = rb.velocity.magnitude * 5f;
                    rb.velocity = Vector3.zero;
                    rb.AddForceAtPosition((originForce + owner.power) * swingDir.ToVector3(V2ToV3Type.XY), sphereHit.point, ForceMode.Impulse);
                    Game.LogicTime.SetGlobalTimeScaleInSecond(0.05f, 1f, true);
                }

                if (Physics.Raycast(sphereHit.point, -deltaDir, out var hitOnOwner, owner.swingRadius)) {
                    var originForce = owner.rigidBody.velocity.magnitude;
                    owner.rigidBody.velocity = Vector3.zero;
                    owner.rigidBody.AddForceAtPosition(-(force + originForce * force.normalized), hitOnOwner.point, ForceMode.Impulse);
                }
                else {
                    var originForce = owner.rigidBody.velocity.magnitude;
                    owner.rigidBody.velocity = Vector3.zero;
                    owner.rigidBody.AddForceAtPosition(-(force + originForce * force.normalized), owner.transform.position, ForceMode.Impulse);
                }

                Game.Event.Invoke(EventNames.OnBallHit, owner.EntityId);
            }
            else {
                owner.rigidBody.AddForceAtPosition(-swingDir * 0.2f, owner.transform.position, ForceMode.Impulse);
            }
        }

        protected override void OnActionUpdate() {
            ChangeAction(PlayerActionNames.PlayerIdle);
        }

        protected override void OnActionExit() { }

        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(owner.transform.position, owner.swingRadius);
        }
    }
}