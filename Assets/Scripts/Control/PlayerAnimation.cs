using System;
using UnityEngine;
using UniRx;

namespace Control
{
    internal enum PlayerAnimationState
    {
        Idle = 0,
        Walk = 1,
        Jump = 2,
        Attack = 3,
        Skill = 4,
        Death = 5
    } 

    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private PlayerControllerBase _controller;
        [SerializeField] private Animator _animator;
        [SerializeField] public double AttackDuration = 0.5F;
        [SerializeField] private float _deathDuration = 1F;

        private ReactiveProperty<PlayerAnimationState> _state = new ReactiveProperty<PlayerAnimationState>(PlayerAnimationState.Idle);

        private void Start()
        {
            _controller.IsMoving.DistinctUntilChanged().Subscribe(UpdateWalking);

            _state.Subscribe(state =>
            {
                Debug.Log("New state: " + state);
                _animator.SetInteger("State", (int) state);
            });
        }

        private void UpdateWalking(bool walking)
        {
            if (_state.Value == PlayerAnimationState.Death) return;
            if (_state.Value == PlayerAnimationState.Attack) return;

            if (walking && _state.Value == PlayerAnimationState.Idle)
                _state.Value = PlayerAnimationState.Walk;
            if(!walking && _state.Value == PlayerAnimationState.Walk)
                _state.Value = PlayerAnimationState.Idle;
        }

        public void Attack()
        {
            if (_state.Value == PlayerAnimationState.Death) return;
            _state.Value = PlayerAnimationState.Attack;
            Observable.Timer(TimeSpan.FromSeconds(AttackDuration))
                .Subscribe(_ => _state.Value = PlayerAnimationState.Idle);
        }

        public void Jump()
        {
            if (_state.Value == PlayerAnimationState.Death) return;
            if (_state.Value == PlayerAnimationState.Attack) return;
            if (_state.Value == PlayerAnimationState.Jump) return;

            _state.Value = PlayerAnimationState.Jump;
        }

        public void HitGround()
        {
            if (_state.Value == PlayerAnimationState.Death) return;
            if (_state.Value == PlayerAnimationState.Jump)
                _state.Value = PlayerAnimationState.Idle;
        }

        public IObservable<Unit> Die()
        {
            _state.Value = PlayerAnimationState.Death;
            return Observable.Timer(TimeSpan.FromSeconds(_deathDuration))
                .Select(_ => _state.Value = PlayerAnimationState.Idle).AsUnitObservable();
        }

    }
}