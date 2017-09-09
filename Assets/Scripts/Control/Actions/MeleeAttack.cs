using System.Timers;
using Assets.Scripts.Entities;
using Assets.Scripts.Util;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace Control.Actions
{
    public class MeleeAttack : Action
    {
        [SerializeField] private float _cooldownTimeInSeconds = 2;
        [SerializeField] private float _attackTimeInSeconds = 1;
        [SerializeField] private Sword _sword;

        private Cooldown _cooldown;

        private void Start()
        {
            _cooldown = new Cooldown(_cooldownTimeInSeconds);
        }

        public override void TryToActivate(Direction direction)
        {
            if (_cooldown.IsOnCoolDown) return;

            Attack();
            _cooldown.Start();
        }

        private void Attack()
        {
            _sword.Visible = true;
            var timer = new Timer(_attackTimeInSeconds);
            timer.Elapsed += (nil, args) => _sword.Visible = false;
            timer.Start();
        }
    }
}