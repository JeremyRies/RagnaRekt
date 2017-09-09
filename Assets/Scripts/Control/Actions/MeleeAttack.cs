using System;
using System.Timers;
using Assets.Scripts.Entities;
using Assets.Scripts.Util;
using UniRx;
using UnityEngine;

namespace Control.Actions
{
    public class MeleeAttack : Action
    {
        [SerializeField] private float _cooldownTimeInSeconds = 2;
        [SerializeField] private Collider2D _weapon;
        [SerializeField] private PlayerAnimation _animation;

        private Cooldown _cooldown;

        private void Awake()
        { 
            _cooldown = new Cooldown(_cooldownTimeInSeconds);
        }

        public override void TryToActivate(Direction direction)
        {
            if (_cooldown.IsOnCoolDown.Value) return;

            Attack();
            _cooldown.Start();
        }

        private void Attack()
        {
            _weapon.gameObject.SetActive(true);
            _animation.Attack();
            Observable.Timer(TimeSpan.FromSeconds(_animation.AttackDuration))
                .Subscribe(_ => _weapon.gameObject.SetActive(false));
        }
    }
}