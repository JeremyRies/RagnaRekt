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
        [SerializeField] private float _attackTimeInSeconds = 1;
        [SerializeField] private MeleeWeapon _weapon;
        [SerializeField] private PlayerControllerBase _playerControllerBase;

        private Cooldown _cooldown;

        private void Start()
        { 
            _cooldown = new Cooldown(_cooldownTimeInSeconds);
            gameObject.AddComponent<Killable>().TeamId = _playerControllerBase.TeamId;

            _weapon.Visible = false;
        }

        public override void TryToActivate(Direction direction)
        {
            if (_cooldown.IsOnCoolDown.Value) return;

            Attack();
            _cooldown.Start();
        }

        private void Attack()
        {
            _weapon.Visible = true;
            _weapon.Attack();
            PlayAttackAnimation();
            Observable.Timer(TimeSpan.FromSeconds(_attackTimeInSeconds))
                .Subscribe(_ => _weapon.Visible = false);
        }

        private void PlayAttackAnimation()
        {

        }
    }
}