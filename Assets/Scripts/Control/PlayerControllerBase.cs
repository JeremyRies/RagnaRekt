using UnityEngine;

namespace Control
{
    [RequireComponent(typeof (CollisionController))]
    public abstract class PlayerControllerBase : MonoBehaviour
    {
        [SerializeField] public int PlayerId;
        [SerializeField] public float 
            MaxJumpHeight = 4, 
            MinJumpHeight = 1,
            MoveSpeed = 6,
            TimeToJumpApex = .4f;

        private const float AccelerationTimeAirborne = .2f;
        private const float AccelerationTimeGrounded = .1f;
        private float _velocityXSmoothing;
        private float _gravity;

        protected IInputProvider InputProvider { get; private set; }
        protected CollisionController Controller { get; private set; }
        protected float MaxJumpVelocity { get; private set; }
        protected float MinJumpVelocity { get; private set; }
        protected Vector3 Velocity;
        
        protected virtual void Start()
        {
            InputProvider = GetInputProvider();
            Controller = GetComponent<CollisionController>();

            _gravity = -(2*MaxJumpHeight)/Mathf.Pow(TimeToJumpApex, 2);
            MaxJumpVelocity = Mathf.Abs(_gravity)*TimeToJumpApex;
            MinJumpVelocity = Mathf.Sqrt(2*Mathf.Abs(_gravity)*MinJumpHeight);
        }

        protected abstract IInputProvider GetInputProvider();

        protected virtual Vector2 GetHorizontalInput()
        {
            return new Vector2(InputProvider.GetAxis("Horizontal"), 0f);
        }

        private void Update()
        {
            var horizontalInput = GetHorizontalInput();
            
            UpdateHorizontalVelocity(horizontalInput);

            HandleJump();

            ApplyGravity();

            MovePlayer(Velocity*Time.deltaTime);

            if (IsHittingCeiling || IsOnGround)
            {
                Velocity.y = 0;
            }
        }

        protected bool IsHittingCeiling
        {
            get { return Controller.Collisions.Above; }
        }

        protected bool IsOnGround
        {
            get { return Controller.Collisions.Below; }
        }

        private void UpdateHorizontalVelocity(Vector2 input)
        {
            var targetVelocityX = input.x*MoveSpeed;

            Velocity.x = Mathf.SmoothDamp(Velocity.x, targetVelocityX, ref _velocityXSmoothing,
                Controller.Collisions.Below ? AccelerationTimeGrounded : AccelerationTimeAirborne);
        }

        protected virtual void MovePlayer(Vector2 amount)
        {
            Controller.Move(amount);
        }

        protected virtual void ApplyGravity()
        {
            Velocity.y += _gravity*Time.deltaTime;
        }

        protected virtual void HandleJump()
        {
            if (InputProvider.GetButtonDown("Jump"))
            {
                if (IsOnGround)
                {
                    Velocity.y = MaxJumpVelocity;
                }
            }

            if (InputProvider.GetButtonUp("Jump"))
            {
                if (Velocity.y > MinJumpVelocity)
                {
                    Velocity.y = MinJumpVelocity;
                }
            }
        }

        public virtual void ArrestMovement()
        {
            Velocity = Vector3.zero;
            _velocityXSmoothing = 0;
        }

        public float VelocityX
        {
            get { return this.Velocity.x; }
        }

        public float VelocityY
        {
            get { return this.Velocity.y; }
        }

        public Vector2 Size
        {
            get { return Controller.Collider.size; }
        }
    }
}