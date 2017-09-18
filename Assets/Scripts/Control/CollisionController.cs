using System;
using System.Collections.Generic;
using System.Linq;
using Control.Raycasts;
using UnityEngine;

namespace Control
{
    public class CollisionController : RaycastControllerBase
    {
        public CollisionInfo Collisions;

        protected override void Start()
        {
            base.Start();
            Collisions.FaceDir = 1;
        }
        
        public void Move(Vector3 velocity, bool standingOnPlatform = false)
        {
            UpdateRaycastOrigins();
            Collisions.Reset();

            if (Math.Abs(velocity.x) > 0 && !standingOnPlatform)
            {
                Collisions.FaceDir = (int) Mathf.Sign(velocity.x);
            }

            HorizontalCollisions(ref velocity);
            if (Math.Abs(velocity.y) > 0)
            {
                VerticalCollisions(ref velocity);
            }

            transform.Translate(velocity);

            if (standingOnPlatform)
            {
                Collisions.Below = true;
            }
        }

        private void HorizontalCollisions(ref Vector3 velocity)
        {
            var rayLength = Mathf.Abs(velocity.x) + SkinWidth;

            if (Mathf.Abs(velocity.x) < SkinWidth)
            {
                rayLength = 2*SkinWidth;
            }

            for (var i = 0; i < HorizontalRayCount; i ++)
            {
                var rayOrigin = velocity.x < 0 ? RaycastOrigins.BottomLeft : RaycastOrigins.BottomRight;

                rayOrigin += Vector2.up*(HorizontalRaySpacing*i);
                var hit = Physics2D.RaycastAll(rayOrigin, Vector2.right*Mathf.Sign(velocity.x), rayLength, CollisionMask);

                var dir = Vector2.right*Collisions.FaceDir*rayLength;
                Debug.DrawRay(rayOrigin, dir, Color.red);

                if (hit.Length % 2 == 0 && DoesNotGetStuckInTheWall(hit, dir)) continue;
                
                velocity.x = (hit[0].distance - SkinWidth) * Collisions.FaceDir;
                rayLength = hit[0].distance;
            }
        }

        private bool DoesNotGetStuckInTheWall(ICollection<RaycastHit2D> hit, Vector2 dir)
        {
            if (hit.Count == 0) return true;
            var lastHit = hit.Last().distance;
            return Mathf.Abs(lastHit - dir.magnitude) > Collider.size.x*1.7;
        }

        private void VerticalCollisions(ref Vector3 velocity)
        {
            var directionY = Mathf.Sign(velocity.y);
            var rayLength = Mathf.Abs(velocity.y) + SkinWidth;

            for (var i = 0; i < VerticalRayCount; i ++)
            {
                var rayOrigin = directionY < 0 ? RaycastOrigins.BottomLeft : RaycastOrigins.TopLeft;
                rayOrigin += Vector2.right*(VerticalRaySpacing*i + velocity.x);
                var hit = Physics2D.Raycast(rayOrigin, Vector2.up*directionY, rayLength, CollisionMask);

                Debug.DrawRay(rayOrigin, Vector2.up*directionY*rayLength, Color.red);

                if (!hit) continue;

                if (hit.collider.tag == "Through")
                {
                    if (directionY > 0 || hit.distance <= 0)
                    {
                        continue;
                    }
                }

                velocity.y = (hit.distance - SkinWidth)*directionY;
                rayLength = hit.distance;

                Collisions.Below = directionY < 0;
                Collisions.Above = directionY > 0;
            }
        }
    }
}