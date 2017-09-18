using System;
using Control.Actions;
using Entities;
using UniRx;
using UnityEngine;
using Action = Control.Actions.Action;

namespace Control
{
    [RequireComponent(typeof (CollisionController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private PlayerMovementConfig _conf;
        [SerializeField] private PlayerAnimation _animation;

        [SerializeField] public Action Attack;
        [SerializeField] public Action Skill;

        private const float AccelerationTimeAirborne = .2f;
        private const float AccelerationTimeGrounded = .1f;

        private bool _inputEnabled = true;

        private float _velocityXSmoothing;
        private float _gravity;

        private IInputProvider InputProvider { get; set; }
        public CollisionController Controller { get; private set; }
        private float MaxJumpVelocity { get; set; }
        private float MinJumpVelocity { get; set; }

        private Vector3 _velocity;
        
        public ReactiveProperty<bool> IsMoving = new ReactiveProperty<bool>(false);
        private const float MinHorizontalMovement = 0.00001F;

        public void Initialize(IInputProvider inputProvider)
        {
            InputProvider = inputProvider;
        }

        protected void Start()
        {         
            Controller = GetComponent<CollisionController>();

            _gravity = -(2* _conf.MaxJumpHeight) /Mathf.Pow(_conf.TimeToJumpApex, 2);
            MaxJumpVelocity = Mathf.Abs(_gravity)* _conf.TimeToJumpApex;
            MinJumpVelocity = Mathf.Sqrt(2*Mathf.Abs(_gravity)* _conf.MinJumpHeight);

            IsMoving.DistinctUntilChanged().Subscribe(_animation.UpdateWalking);
        }

        private Vector2 GetHorizontalInput()
        {
            return new Vector2(InputProvider.GetAxis("Horizontal"), 0f);
        }

        private void Update()
        {
            Vector2 horizontalInput = Vector2.zero;
            if (_inputEnabled && InputProvider != null)
            {
                HandleJump();

                HandleSkill();
                HandleAttack();

                horizontalInput = GetHorizontalInput();
            }

            UpdateViewDirection(horizontalInput);
            UpdateHorizontalVelocity(horizontalInput);

            ApplyGravity();

            MovePlayer(_velocity*Time.deltaTime);

            if (_velocity.y < 0.1)
            {
                _animation.HitGround();
            }

            if (IsHittingCeiling || IsOnGround)
            {
                _velocity.y = 0;
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

        private bool IsHittingCeiling
        {
            get { return Controller.Collisions.Above; }
        }

        private bool IsOnGround
        {
            get { return Controller.Collisions.Below; }
        }

        private void UpdateHorizontalVelocity(Vector2 input)
        {
            var targetVelocityX = input.x* _conf.MoveSpeed;
            IsMoving.Value = Mathf.Abs(input.x) > MinHorizontalMovement;

            _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing,
                Controller.Collisions.Below ? AccelerationTimeGrounded : AccelerationTimeAirborne);
        }

        private void MovePlayer(Vector2 amount)
        {
            Controller.Move(amount);
        }

        private void ApplyGravity()
        {
            _velocity.y += _gravity*Time.deltaTime;
        }

        private void HandleJump()
        {
            if (InputProvider.GetButtonDown("Jump"))
            {
                if (IsOnGround)
                {
                    _velocity.y = MaxJumpVelocity;
                    _animation.Jump();
                }
            }

            if (InputProvider.GetButtonUp("Jump"))
            {
                if (_velocity.y > MinJumpVelocity)
                {
                    _velocity.y = MinJumpVelocity;
                }
            }
        }

        public void ArrestMovement()
        {
            _velocity = Vector3.zero;
            _velocityXSmoothing = 0;
        }

        public bool isLookingLeft
        {
            get { return this._looksLeft; }

        }
    }
}