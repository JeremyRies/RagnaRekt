﻿using System;
using Assets.Scripts.Util;
using Entities;
using UniRx;
using UnityEngine;

namespace Control.Actions
{
    public class MeleeAttack : Action
    {
        [SerializeField] private float _cooldownTimeInSeconds = 2;

        [SerializeField] private Player _player;

        [SerializeField] private Collider2D _weapon;
        [SerializeField] private PlayerAnimation _animation;


        private Cooldown _cooldown;
        private Killable _killable;

        private void Awake()
        { 
            _cooldown = new Cooldown(_cooldownTimeInSeconds);
            _killable = gameObject.AddComponent<Killable>();

            _weapon.gameObject.SetActive(false);
        }

        public override void TryToActivate(Direction direction)
        {
            if (_cooldown.IsOnCoolDown.Value) return;
            
            Attack();
            _cooldown.Start();
        }

        private void Attack()
        {
            _killable.TeamId = _player.TeamId;
            _animation.Attack();
            _weapon.gameObject.SetActive(true);
            Observable.Timer(TimeSpan.FromSeconds(_animation.AttackDuration))
                .Subscribe(_ => _weapon.gameObject.SetActive(false));
        }
    }
}