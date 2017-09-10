using System;
using System.Linq;
using UnityEngine;
using UniRx;
using UnityEditor;
using UnityEditor.Animations;

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
        [SerializeField] private float _deathDuration = 1F;

        private readonly ReactiveProperty<PlayerAnimationState> _state = new ReactiveProperty<PlayerAnimationState>(PlayerAnimationState.Idle);
        private readonly Subject<PlayerAnimationState> _animationFinished = new Subject<PlayerAnimationState>();

        private bool _animating;

        private readonly PlayerAnimationState[] StatesWithAnimantion =
        {
            PlayerAnimationState.Attack, PlayerAnimationState.Skill, PlayerAnimationState.Death
        };

        private void Start()
        {
            _controller.IsMoving.DistinctUntilChanged().Subscribe(UpdateWalking);

            _state.Subscribe(state =>
            {
                _animating = StatesWithAnimantion.Contains(state);
                // Debug.Log("New state: " + state);
                _animator.SetInteger("State", (int) state);
            });
        }

        private void UpdateWalking(bool walking)
        {
            if (_animating) return;

            if (walking && _state.Value == PlayerAnimationState.Idle)
                _state.Value = PlayerAnimationState.Walk;
            if(!walking && _state.Value == PlayerAnimationState.Walk)
                _state.Value = PlayerAnimationState.Idle;
        }

        private void OnAnimationFinished(int animation)
        {
            // Debug.Log("Animation finished: " + (PlayerAnimationState) animation);
            _state.Value = PlayerAnimationState.Idle;
            _animationFinished.OnNext((PlayerAnimationState) animation);
        }

        public IObservable<bool> Attack()
        {
            if (_animating) return Observable.Empty<bool>();
            var subject = new Subject<bool>();
            _state.Value = PlayerAnimationState.Attack;
            _animationFinished.Where(anim => anim == PlayerAnimationState.Attack)
                .Take(1).Subscribe(_ => subject.OnNext(false));
            return Observable.Return(true).Concat(subject);
        }

        public void Jump()
        {
            if (_animating) return;
            if (_state.Value == PlayerAnimationState.Jump) return;

            _state.Value = PlayerAnimationState.Jump;
        }

        public void HitGround()
        {
            if (_animating) return;
            if (_state.Value == PlayerAnimationState.Jump)
                _state.Value = PlayerAnimationState.Idle;
        }

        public IObservable<Unit> Die()
        {
            _state.Value = PlayerAnimationState.Death;
            var timer = Observable.Timer(TimeSpan.FromSeconds(_deathDuration));
            timer.Subscribe(_ => OnAnimationFinished((int) PlayerAnimationState.Death));
            return timer.AsUnitObservable();
        }

        public IObservable<Unit> UseSkill()
        {
            if (_animating) return Observable.Empty<Unit>();

            _state.Value = PlayerAnimationState.Skill;
            return _animationFinished.Where(anim => anim == PlayerAnimationState.Skill)
                .Take(1).AsUnitObservable();
        }

        public AnimatorController Controller { set { _animator.runtimeAnimatorController = value; } }
    }
}