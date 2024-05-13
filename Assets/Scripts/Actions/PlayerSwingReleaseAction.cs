using System;
using Balls;
using Constants;
using PlayerEntities;
using UnityEngine;
using UnityEngine.Serialization;
using XiheFramework.Combat.Action;
using XiheFramework.Core.Entity;
using XiheFramework.Core.Utility.Extension;
using XiheFramework.Runtime;

namespace Actions {
    public class PlayerSwingReleaseAction : TennisPlayerActionEntityBase {
        public override string EntityAddressName => PlayerActionNames.PlayerSwingRelease;

        public Material hitMaterial;
        public LayerMask hitLayerMask;

        public AK.Wwise.Event swingSound;
        public AK.Wwise.Event hitBallSound;
        public AK.Wwise.Event hitObstacleSound;

        protected override void OnActionInit() {
            base.OnActionInit();
            swingSound.Post(owner.gameObject);

            var swingDir = FetchArgument<Vector2>(ActionArgumentNames.SwingDirection);
            var swingPower = FetchArgument<float>(ActionArgumentNames.SwingPower);

            var isHit = Physics.SphereCast(owner.transform.position - swingDir.ToVector3(V2ToV3Type.XY) * owner.swingRadius, owner.swingRadius,
                swingDir.ToVector3(V2ToV3Type.XY), out var sphereHit, owner.swingRadius, hitLayerMask, QueryTriggerInteraction.Ignore);

            if (isHit) {
                var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.transform.position = sphereHit.point;
                go.transform.localScale = Vector3.one * 0.1f;
                go.GetComponent<Renderer>().material = hitMaterial;
                Destroy(go.gameObject, 1f);

                var deltaDir = sphereHit.point - owner.transform.position;
                var distance = Vector3.Distance(owner.transform.position, sphereHit.point);
                distance = Mathf.Clamp(distance, 0, owner.swingRadius);
                var force = swingDir.ToVector3(V2ToV3Type.XY) * (owner.power * swingPower);

                var soundPlayed = false;
                var rb = sphereHit.collider.GetComponentInParent<Rigidbody>();
                if (rb != null) {
                    if (rb.GetComponent<GeneralBallEntity>() != null) {
                        //is a ball
                        Game.LogicTime.SetGlobalTimeScaleInSecond(0.05f, 0.75f, true);
                        Game.Event.Invoke(EventNames.OnBallHit, owner.EntityId);
                        hitBallSound.Post(owner.gameObject);
                        soundPlayed = true;
                        rb.velocity = Vector3.zero;
                        rb.AddForceAtPosition(force, sphereHit.point, ForceMode.Impulse);
                    }
                    else {
                        rb.AddForceAtPosition(force, sphereHit.point, ForceMode.Impulse);
                    }
                }

                if (!soundPlayed) {
                    hitObstacleSound.Post(owner.gameObject);
                }

                var dirDot = Vector3.Dot(swingDir.ToVector3(V2ToV3Type.XY), -owner.rigidBody.velocity.normalized);
                dirDot = dirDot / 2 + 0.5f;
                if (Physics.Raycast(sphereHit.point, -deltaDir, out var hitOnOwner, owner.swingRadius)) {
                    owner.rigidBody.velocity *= dirDot;
                    owner.rigidBody.AddForceAtPosition(-(force), hitOnOwner.point, ForceMode.Impulse);
                }
                else {
                    owner.rigidBody.velocity *= dirDot;
                    owner.rigidBody.AddForceAtPosition(-(force), owner.transform.position, ForceMode.Impulse);
                }
            }
            else {
                owner.rigidBody.AddForceAtPosition(-swingDir * 0.5f, owner.transform.position, ForceMode.VelocityChange);
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