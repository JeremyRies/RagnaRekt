using System;
using System.Collections.Generic;
using System.Linq;
using Animation;
using Control;
using UnityEngine;
using UniRx;
using UnityEditor;

namespace Control
{
    public enum PlayerAnimationState
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
        [SerializeField] private float _deathDuration = 1F;

        [SerializeField] private PlayerAnimatorConfig _config;
        [SerializeField] private SpriteRenderer _renderer;

        private readonly ReactiveProperty<PlayerAnimationState> _state = new ReactiveProperty<PlayerAnimationState>(PlayerAnimationState.Idle);
        private readonly Subject<PlayerAnimationState> _animationFinished = new Subject<PlayerAnimationState>();

        private bool _animating;

        private SpriteAnimator<PlayerAnimationState> _animator;

        private readonly PlayerAnimationState[] StatesWithAnimantion =
        {
            PlayerAnimationState.Attack, PlayerAnimationState.Skill, PlayerAnimationState.Death
        };



        public IObservable<PlayerAnimationState> State;

        private void Awake()
        {
            _animator = new SpriteAnimator<PlayerAnimationState>(_config, _renderer);
            _animator.PlayAnimation(PlayerAnimationState.Idle);

            State = _animator.ShownAnimation;

            _controller.IsMoving.DistinctUntilChanged().Subscribe(UpdateWalking);


            _state.Subscribe(state =>
            {
                _animating = StatesWithAnimantion.Contains(state);
                // Debug.Log("New state: " + state);
                // _animator.SetInteger("State", (int) state);
                _animator.PlayAnimation(state);
            });
        }

        private void UpdateWalking(bool walking)
        {
            /*if (_animating) return;

            if (walking && _state.Value == PlayerAnimationState.Idle)
                _state.Value = PlayerAnimationState.Walk;
            if(!walking && _state.Value == PlayerAnimationState.Walk)
                _state.Value = PlayerAnimationState.Idle;*/
        }

        public void Attack()
        {
            if (_animator.CurrentAnimation != PlayerAnimationState.Death)
                _animator.PlayAnimation(PlayerAnimationState.Attack);
        }

        public void Jump()
        {
            if (_animating) return;
            if (_state.Value == PlayerAnimationState.Jump) return;

            _state.Value = PlayerAnimationState.Jump;
        }

        public void HitGround()
        {
            if (_animator.CurrentAnimation == PlayerAnimationState.Jump)
                _animator.PlayAnimation(PlayerAnimationState.Idle);
        }

        public void Die()
        {
            Debug.Log("Die yo!");
            _animator.PlayAnimation(PlayerAnimationState.Death);
        }

        public IObservable<Unit> UseSkill()
        {
            if (_animating) return Observable.Empty<Unit>();

            _state.Value = PlayerAnimationState.Skill;
            return _animationFinished.Where(anim => anim == PlayerAnimationState.Skill)
                .Take(1).AsUnitObservable();
        }

        public RuntimeAnimatorController Controller
        {
            set { }
        } //_animator.runtimeAnimatorController = value; } }

        private void OnDestroy()
        {
            _animator.InterruptCurrentAnimation();
        }
    }
}