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
        [SerializeField] private PlayerAnimatorConfig _config;
        [SerializeField] private SpriteRenderer _renderer;

        private SpriteAnimator<PlayerAnimationState> _animator;

        public IObservable<PlayerAnimationState> State { get { return _animator.ShownAnimation; } }

        private void Awake()
        {
            _animator = new SpriteAnimator<PlayerAnimationState>(_config, _renderer);
            _animator.PlayAnimation(PlayerAnimationState.Idle);
        }

        public void UpdateWalking(bool walking)
        {
            if (walking && _animator.CurrentAnimation == PlayerAnimationState.Idle)
                _animator.PlayAnimation(PlayerAnimationState.Walk);
            if(!walking && _animator.CurrentAnimation == PlayerAnimationState.Walk)
                _animator.PlayAnimation(PlayerAnimationState.Idle);
        }

        public void Attack()
        {
            if (_animator.CurrentAnimation != PlayerAnimationState.Death)
                _animator.PlayAnimation(PlayerAnimationState.Attack);
        }

        public void Jump()
        {
            if (_animator.CurrentAnimation == PlayerAnimationState.Walk 
             || _animator.CurrentAnimation == PlayerAnimationState.Idle)
                _animator.PlayAnimation(PlayerAnimationState.Jump);
        }

        public void HitGround()
        {
            if (_animator.CurrentAnimation == PlayerAnimationState.Jump)
                _animator.PlayAnimation(PlayerAnimationState.Idle);
        }

        public void Die()
        {
            _animator.PlayAnimation(PlayerAnimationState.Death);
        }

        public void UseSkill()
        {
            if(_animator.CurrentAnimation != PlayerAnimationState.Death)
                _animator.PlayAnimation(PlayerAnimationState.Skill);
        }

        public PlayerAnimatorConfig Controller
        {
            set { _animator.Config = value; }
        } 

        private void OnDestroy()
        {
            _animator.InterruptCurrentAnimation();
        }
    }
}