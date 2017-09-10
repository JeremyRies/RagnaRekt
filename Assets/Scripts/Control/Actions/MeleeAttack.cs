using System;
using Assets.Scripts.Util;
using Entities;
using Sound;
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

        [SerializeField] private HammerThrow HammerThrow;
        
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
            if (HammerThrow != null)
            {
                if (HammerThrow._isInHand == false) return;
            }
            
            Attack();
            _cooldown.Start();

        }

        private void Attack()
        {
            SfxSound.SfxSoundInstance.PlayClip( _player.HeroType == HeroType.Thor ? ClipIdentifier.ThorAttack : ClipIdentifier.LokiAttack );
            _killable.TeamId = _player.Team.TeamId;
            _animation.Attack().Subscribe(_weapon.gameObject.SetActive);
        }
    }
}