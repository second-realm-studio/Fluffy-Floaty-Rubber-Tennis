using System;
using System.Collections.Generic;
using UnityEngine;

public static class SmoothFanCastHelper {
    public enum SortingMethod {
        FirstHit = 0,
        LastHit,
        DistanceMin,
        DistanceMax,
        AngleMin,
        AngleMax
    }

    public static RaycastHit[] SmoothFanCastAllXY(Vector3 origin, float originRadius, float distance, float angle, Vector3 direction, int rayCount, LayerMask layerMask,
        SortingMethod sortingMethod, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) {
        //preprocess
        angle = Mathf.Clamp(angle, 0, 360f);
        var midAngle = Vector3.SignedAngle(direction, Vector3.right, Vector3.back);
        var startAngle = midAngle - angle / 2;
        var finishAngle = midAngle + angle / 2;

        //(Collider, distance, angle, RaycastHit)
        List<RaycastHit> existingHits = new List<RaycastHit>();

        for (int i = 0; i < rayCount; i++) {
            var destAngle = startAngle + (angle / (rayCount - 1)) * i;
            var destX = origin.x + distance * Mathf.Cos(destAngle * Mathf.Deg2Rad);
            var destY = origin.y + distance * Mathf.Sin(destAngle * Mathf.Deg2Rad);
            var destPoint = new Vector3(destX, destY, origin.z);

            var originAngle = finishAngle - (angle / (rayCount - 1)) * i;
            var originX = origin.x + originRadius * Mathf.Cos(originAngle * Mathf.Deg2Rad);
            var originY = origin.y + originRadius * Mathf.Sin(originAngle * Mathf.Deg2Rad);
            var originPoint = new Vector3(originX, originY, origin.z);
            originPoint = origin + (origin - originPoint).normalized * originRadius;

            Debug.DrawLine(originPoint, destPoint, new Color(i / (float)rayCount, 0, 0), 0.5f);

            var rayDir = destPoint - originPoint;

            if (Physics.Raycast(originPoint, rayDir, out var currentHit, rayDir.magnitude, layerMask.value, queryTriggerInteraction)) {
                Debug.DrawLine(originPoint, currentHit.point, new Color(0, i / (float)rayCount, 0), 0.5f);
                bool untouched = true;
                for (var j = 0; j < existingHits.Count; j++) {
                    var existingHit = existingHits[j];
                    if (existingHit.collider == currentHit.collider) {
                        switch (sortingMethod) {
                            case SortingMethod.FirstHit:
                                break;
                            case SortingMethod.LastHit:
                                existingHits[j] = currentHit;
                                untouched = false;
                                break;
                            case SortingMethod.DistanceMin:
                                if (Vector3.Distance(originPoint, existingHit.point) > Vector3.Distance(originPoint, currentHit.point)) {
                                    existingHits[j] = currentHit;
                                    untouched = false;
                                }

                                break;
                            case SortingMethod.DistanceMax:
                                if (Vector3.Distance(originPoint, existingHit.point) < Vector3.Distance(originPoint, currentHit.point)) {
                                    existingHits[j] = currentHit;
                                    untouched = false;
                                }

                                break;
                            case SortingMethod.AngleMin:
                                if (Vector3.Angle(existingHit.point - origin, direction) > Vector3.Angle(currentHit.point - origin, direction)) {
                                    existingHits[j] = currentHit;
                                    untouched = false;
                                }

                                break;
                            case SortingMethod.AngleMax:
                                if (Vector3.Angle(existingHit.point - origin, direction) < Vector3.Angle(currentHit.point - origin, direction)) {
                                    existingHits[j] = currentHit;
                                    untouched = false;
                                }

                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(sortingMethod), sortingMethod, null);
                        }
                    }
                }

                if (untouched) {
                    existingHits.Add(currentHit);
                }
            }
        }

        return existingHits.ToArray();
    }
}