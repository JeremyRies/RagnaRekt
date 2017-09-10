using System;
using Control.Actions;
using Entities;
using UniRx;
using UnityEngine;
using Action = Control.Actions.Action;

namespace Control
{
    [RequireComponent(typeof (CollisionController))]
    public abstract class PlayerControllerBase : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private PlayerMovementConfig _conf;
        [SerializeField] private PlayerAnimation _animation;
        [SerializeField] private Player _player;

        [SerializeField] public Action Attack;
        [SerializeField] public Action Skill;

        private const float AccelerationTimeAirborne = .2f;
        private const float AccelerationTimeGrounded = .1f;

        private bool _inputEnabled = true;

        private float _velocityXSmoothing;
        private float _gravity;

        public IInputProvider InputProvider { get; private set; }
        public CollisionController Controller { get; private set; }
        protected float MaxJumpVelocity { get; private set; }
        protected float MinJumpVelocity { get; private set; }
        [NonSerialized] protected Vector3 Velocity;
        
        public ReactiveProperty<bool> IsMoving = new ReactiveProperty<bool>(false);
        private const float MinHorizontalMovement = 0.03F;

        protected virtual void Start()
        {
            InputProvider = GetInputProvider(_player.PlayerId);
            Controller = GetComponent<CollisionController>();

            _gravity = -(2* _conf.MaxJumpHeight) /Mathf.Pow(_conf.TimeToJumpApex, 2);
            MaxJumpVelocity = Mathf.Abs(_gravity)* _conf.TimeToJumpApex;
            MinJumpVelocity = Mathf.Sqrt(2*Mathf.Abs(_gravity)* _conf.MinJumpHeight);
        }

        protected abstract IInputProvider GetInputProvider(int playerPlayerId);

        protected virtual Vector2 GetHorizontalInput()
        {
            return new Vector2(InputProvider.GetAxis("Horizontal"), 0f);
        }

        private void Update()
        {
            Vector2 horizontalInput = Vector2.zero;
            if (_inputEnabled)
            {
                HandleJump();

                HandleSkill();
                HandleAttack();

                horizontalInput = GetHorizontalInput();
            }

            UpdateViewDirection(horizontalInput);
            UpdateHorizontalVelocity(horizontalInput);

            ApplyGravity();

            MovePlayer(Velocity*Time.deltaTime);

            if (Velocity.y < 0.1)
            {
                _animation.HitGround();
            }

            if (IsHittingCeiling || IsOnGround)
            {
                Velocity.y = 0;
                _animation.HitGround();
            }
        }

        private bool _looksLeft;

        public void DisableInput()
        {
            _inputEnabled = false;
        }

        public void EnableInput()
        {
            _inputEnabled = true;
        }

        public void UpdateViewDirection(Vector2 horizontalInput)
        {
            if (!_looksLeft && horizontalInput.x < 0)
            {
                _looksLeft = true;
                _sprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
            } else if (_looksLeft && horizontalInput.x > 0)
            {
                _looksLeft = false;
                _sprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        private void HandleSkill()
        {
            if (InputProvider.GetButtonDown("Skill"))
            {
                Skill.TryToActivate(GetDirection());
            }
        }

        private void HandleAttack()
        {
            if (InputProvider.GetButtonDown("Attack"))
            {
                Attack.TryToActivate(GetDirection());
            }
        }

        private Direction GetDirection()
        {
            var horizontal = InputProvider.GetAxis("Horizontal");
            return Mathf.Abs(horizontal) > MinHorizontalMovement
                ? horizontal > 0 ? Direction.RIGHT : Direction.LEFT
                : _looksLeft ? Direction.LEFT : Direction.RIGHT;
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
            var targetVelocityX = input.x* _conf.MoveSpeed;
            IsMoving.Value = Mathf.Abs(input.x) > MinHorizontalMovement;

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
                    _animation.Jump();
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

        public bool isLookingLeft
        {
            get { return this._looksLeft; }

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