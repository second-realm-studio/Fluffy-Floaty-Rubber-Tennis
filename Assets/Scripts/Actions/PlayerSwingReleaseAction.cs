using System;
using Balls;
using Constants;
using PlayerEntities;
using Unity.Mathematics;
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
        public float closeSwingDistance = 10f;
        public float swingCoverAngle = 270f;
        public int rayCount = 20;

        [SerializeField]
        public SmoothFanCastHelper.SortingMethod hitSortingMethod = SmoothFanCastHelper.SortingMethod.AngleMin;


        public AK.Wwise.Event swingSound;
        public AK.Wwise.Event hitBallSound;
        public AK.Wwise.Event hitObstacleSound;

        private LayerMask m_HitLayerMask;
        private Vector3 m_SwingDir;
        private float m_AfterSwingTimer;

        protected override void OnActionInit() {
            base.OnActionInit();
            swingSound.Post(owner.gameObject);

            m_SwingDir = FetchArgument<Vector2>(ActionArgumentNames.SwingDirection).ToVector3(V2ToV3Type.XY).normalized;
            var swingCharge01 = FetchArgument<float>(ActionArgumentNames.SwingPower01);

            var origin = owner.transform.position;
            var radius = closeSwingDistance;
            var direction = m_SwingDir;
            var distance = owner.swingRadius;

            if (owner.isRightSide) {
                m_HitLayerMask = LayerMask.GetMask("Ball", "DynamicObstacle", "FixedObstacle", "SolidWall", "Player1");
            }
            else {
                m_HitLayerMask = LayerMask.GetMask("Ball", "DynamicObstacle", "FixedObstacle", "SolidWall", "Player2");
            }

            // var hits = Physics.SphereCastAll(origin, radius, direction, distance, hitLayerMask, QueryTriggerInteraction.Ignore);
            var hits = SmoothFanCastHelper.SmoothFanCastAllXY(origin, radius, distance, swingCoverAngle, direction, rayCount, m_HitLayerMask, hitSortingMethod,
                QueryTriggerInteraction.Ignore);

            LayerMask hitLayers = 0;
            foreach (var sphereHit in hits) {
                //exclude hit to owner
                var possiblePlayerEntity = sphereHit.collider.GetComponentInParent<TennisPlayerEntity>();
                if (possiblePlayerEntity != null && possiblePlayerEntity.EntityId == OwnerId) {
                    continue;
                }

                if (FilterCachedLayer(sphereHit, hitLayers, out hitLayers)) {
                    continue;
                }

                //debug
                if (Vector3.Distance(Vector3.zero, sphereHit.point) < 0.1f) {
                    return;
                }

                Game.Particle.PlayParticle(OwnerId, ParticleNames.HitObstacle, sphereHit.point + Vector3.back * 10f, quaternion.identity, Vector3.one * 3f, false, false);

                // var deltaDir = sphereHit.point - owner.transform.position;
                var force = m_SwingDir * (owner.power * swingCharge01);
                ApplyForce(sphereHit, force);
                ApplyReactingForce(force);
            }

            if (hits.Length == 0) {
                //push air
                owner.rigidBody.AddForceAtPosition(-m_SwingDir * (2f + swingCharge01), owner.transform.position, ForceMode.Impulse);
            }

            owner.armRTransform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.up, m_SwingDir, Vector3.forward));
        }

        private bool FilterCachedLayer(RaycastHit sphereHit, LayerMask currentCachedLayers, out LayerMask newCachedLayers) {
            var currentLayer = sphereHit.collider.gameObject.layer;
            if (currentCachedLayers.Includes(currentLayer)) {
                newCachedLayers = currentCachedLayers;
                return true;
            }

            currentCachedLayers |= (1 << currentLayer);
            newCachedLayers = currentCachedLayers;
            return false;
        }

        private void ApplyForce(RaycastHit hitInfo, Vector3 force) {
            if (IsRbHit(hitInfo, out var hitRb, out bool isBall)) {
                if (isBall) {
                    var dirDotBall = Vector3.Dot(m_SwingDir, hitRb.velocity.normalized);
                    dirDotBall = dirDotBall / 2 + 0.5f;
                    hitRb.velocity *= dirDotBall;
                    hitRb.AddForceAtPosition(force, hitInfo.point, ForceMode.Impulse);
                    Game.Event.Invoke(EventNames.OnBallHit, owner.EntityId);
                    hitBallSound.Post(owner.gameObject);
                    Game.LogicTime.SetGlobalTimeScaleInSecond(0.02f, 0.8f, true);
                }
                else {
                    hitRb.AddForceAtPosition(force, hitInfo.point, ForceMode.Impulse);
                    hitObstacleSound.Post(owner.gameObject);
                }
            }
            else {
                hitObstacleSound.Post(owner.gameObject);
            }
        }

        private void ApplyReactingForce(Vector3 force) {
            var dirDot = Vector3.Dot(m_SwingDir, -owner.rigidBody.velocity.normalized);
            dirDot = dirDot / 2 + 0.5f;
            owner.rigidBody.velocity *= dirDot;

            owner.rigidBody.AddForceAtPosition(-force, owner.transform.position, ForceMode.Impulse);
        }

        bool IsRbHit(RaycastHit hit, out Rigidbody hitRb, out bool isBall) {
            hitRb = hit.collider.attachedRigidbody;
            if (hitRb == null) {
                isBall = false;
                return false;
            }

            var ballEntity = hitRb.GetComponentInParent<GeneralBallEntity>();
            isBall = ballEntity != null;
            return true;
        }


        protected override void OnActionUpdate() {
            m_AfterSwingTimer += ScaledDeltaTime;
            if (m_AfterSwingTimer >= afterSwingDuration) {
                ChangeAction(PlayerActionNames.PlayerIdle);
            }
        }

        public override void OnLateUpdateCallback() {
            base.OnLateUpdateCallback();
            if (owner != null) {
                owner.armRTransform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.up, m_SwingDir, Vector3.forward));
            }
        }

        protected override void OnActionExit() { }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            // Gizmos.DrawWireSphere(dorigin, dradius);
            // for (int i = 0; i < ddistance; i++) {
            //     Gizmos.DrawWireSphere(dorigin + ddirection * i, dradius);
            // }
        }
#endif
    }
}