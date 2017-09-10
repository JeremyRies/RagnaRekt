using UnityEngine;

namespace Control.Raycasts
{
    [RequireComponent(typeof (BoxCollider2D))]
    public abstract class RaycastControllerBase : MonoBehaviour
    {
        [SerializeField] public LayerMask CollisionMask;
        [SerializeField] public int HorizontalRayCount = 10;
        [SerializeField] public int VerticalRayCount = 20;

        [HideInInspector] public BoxCollider2D Collider;
        
        protected const float SkinWidth = .015f;
        protected float HorizontalRaySpacing;
        protected float VerticalRaySpacing;
        protected RaycastOrigins RaycastOrigins;

        protected virtual void Start()
        {
            Collider = GetComponent<BoxCollider2D>();
            CalculateRaySpacing();
        }

        private void AssureCollider()
        {
            if (Collider) return;
            Collider = GetComponent<BoxCollider2D>();
        }

        protected void UpdateRaycastOrigins()
        {
            AssureCollider();
            var bounds = Collider.bounds;
            bounds.Expand(SkinWidth*-2);

            RaycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            RaycastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
            RaycastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
            RaycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        private void CalculateRaySpacing()
        {
            var bounds = Collider.bounds;
            bounds.Expand(SkinWidth*-2);

            HorizontalRayCount = Mathf.Clamp(HorizontalRayCount, 2, int.MaxValue);
            VerticalRayCount = Mathf.Clamp(VerticalRayCount, 2, int.MaxValue);

            HorizontalRaySpacing = bounds.size.y/(HorizontalRayCount - 1);
            VerticalRaySpacing = bounds.size.x/(VerticalRayCount - 1);
        }

        public Bounds Bounds { get { return this.Collider.bounds; } }
    }
}