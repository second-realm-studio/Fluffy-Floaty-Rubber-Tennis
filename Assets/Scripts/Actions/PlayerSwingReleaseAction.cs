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

        public float afterSwingDuration = 0.5f;

        public Material hitMaterial;
        public LayerMask hitLayerMask;

        public AK.Wwise.Event swingSound;
        public AK.Wwise.Event hitBallSound;
        public AK.Wwise.Event hitObstacleSound;

        private Vector2 m_SwingDir;
        private float m_AfterSwingTimer;


        protected override void OnActionInit() {
            base.OnActionInit();
            swingSound.Post(owner.gameObject);

            m_SwingDir = FetchArgument<Vector2>(ActionArgumentNames.SwingDirection);
            var swingCharge01 = FetchArgument<float>(ActionArgumentNames.SwingPower01);

            var isHit = Physics.SphereCast(owner.transform.position - m_SwingDir.ToVector3(V2ToV3Type.XY) * owner.swingRadius, owner.swingRadius,
                m_SwingDir.ToVector3(V2ToV3Type.XY), out var sphereHit, owner.swingRadius, hitLayerMask, QueryTriggerInteraction.Ignore);

            if (isHit) {
                var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.transform.position = sphereHit.point;
                go.transform.localScale = Vector3.one * 0.1f;
                go.GetComponent<Renderer>().material = hitMaterial;
                Destroy(go.gameObject, 1f);

                var deltaDir = sphereHit.point - owner.transform.position;
                var distance = Vector3.Distance(owner.transform.position, sphereHit.point);
                distance = Mathf.Clamp(distance, 0, owner.swingRadius);
                var force = m_SwingDir.ToVector3(V2ToV3Type.XY) * (owner.power * swingCharge01);

                var soundPlayed = false;
                var rb = sphereHit.collider.GetComponentInParent<Rigidbody>();
                if (rb != null) {
                    if (rb.GetComponent<GeneralBallEntity>() != null) {
                        //is a ball
                        Game.LogicTime.SetGlobalTimeScaleInSecond(0.05f, 0.75f, true);
                        Game.Event.Invoke(EventNames.OnBallHit, owner.EntityId);
                        hitBallSound.Post(owner.gameObject);
                        soundPlayed = true;
                        var dirDotBall = Vector3.Dot(m_SwingDir.ToVector3(V2ToV3Type.XY), rb.velocity.normalized);
                        dirDotBall = dirDotBall / 2 + 0.5f;
                        rb.velocity *= dirDotBall;
                        rb.AddForceAtPosition(force, sphereHit.point, ForceMode.Impulse);
                    }
                    else {
                        rb.AddForceAtPosition(force, sphereHit.point, ForceMode.Impulse);
                    }
                }

                if (!soundPlayed) {
                    hitObstacleSound.Post(owner.gameObject);
                }

                var dirDot = Vector3.Dot(m_SwingDir.ToVector3(V2ToV3Type.XY), -owner.rigidBody.velocity.normalized);
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
                //push air
                owner.rigidBody.AddForceAtPosition(-m_SwingDir * (1f + swingCharge01), owner.transform.position, ForceMode.Impulse);
            }

            owner.armRTransform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.up, m_SwingDir, Vector3.forward));
        }

        protected override void OnActionUpdate() {
            m_AfterSwingTimer += ScaledDeltaTime;
            if (m_AfterSwingTimer >= afterSwingDuration) {
                ChangeAction(PlayerActionNames.PlayerIdle);
            }
        }

        public override void OnLateUpdateCallback() {
            base.OnLateUpdateCallback();
            owner.armRTransform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.up, m_SwingDir, Vector3.forward));
        }

        protected override void OnActionExit() { }

        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(owner.transform.position, owner.swingRadius);
        }
    }
}