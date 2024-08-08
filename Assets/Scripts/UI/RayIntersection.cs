using System;
using System.Linq;
using UnityEngine;

namespace UI {
    public class RayIntersection {
        public static Vector2 GetIntersection(Vector2 direction, float screenWidth, float screenHeight) {
            Vector2 screenCenter = new Vector2(screenWidth / 2, screenHeight / 2);
            direction = direction.normalized;

            Vector2 top = Vector2.positiveInfinity, bottom = Vector2.positiveInfinity, left = Vector2.positiveInfinity, right = Vector2.positiveInfinity;

            if (direction.x != 0 && direction.y > 0) {
                var rad = Mathf.Atan(direction.y / direction.x);
                top = new Vector2(Mathf.Tan(rad) * screenHeight / 2f + screenCenter.x, screenHeight);
            }

            if (direction.x != 0 && direction.y < 0) {
                var rad = Mathf.Atan(direction.y / direction.x);
                bottom = new Vector2(Mathf.Tan(rad) * screenHeight / 2f + screenCenter.x, 0);
            }

            if (direction.y != 0 && direction.x > 0) {
                var rad = Mathf.Atan(direction.x / direction.y);
                right = new Vector2(screenWidth, Mathf.Tan(rad) * screenWidth / 2f + screenCenter.y);
            }

            if (direction.y != 0 && direction.x < 0) {
                var rad = Mathf.Atan(direction.x / direction.y);
                left = new Vector2(0, Mathf.Tan(rad) * screenWidth / 2f + screenCenter.y);
            }

            var validIntersections = new Vector2[] { top, bottom, left, right };

            // 找出最近的交点
            Vector2 closestIntersection = validIntersections
                .OrderBy(point => Vector2.Distance(point, screenCenter))
                .FirstOrDefault();

            return closestIntersection;
        }
    }
}