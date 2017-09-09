using System;
using System.Collections;
using Assets.Scripts.Util;
using Entities;
using UnityEngine;

namespace Control.Actions
{
    public class HammerThrow : Action
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private HammerConfig _conf;

        [SerializeField] public PlayerControllerBase PlayerController;
        [SerializeField] private Player _player;


        private Vector2 _dir;
        private float _distance;
        private bool _flyBack;
        private Cooldown _cooldown;
        private Cooldown _hammerReturn;
        private bool _active;
        private GameObject _hammerInstance;
        private SpriteRenderer _spriteRendererOfHammerInstance;

        private void Start()
        {
            _cooldown = new Cooldown(_conf.CooldownTimeInSeconds);
            _hammerReturn = new Cooldown(_conf.CooldownTimeInSeconds / 2);
        }

        public override void TryToActivate(Direction direction)
        {
            if (_cooldown.IsOnCoolDown.Value) return;

            _cooldown.Start();
            _hammerReturn.Start();
            StartCoroutine(Throw());
        }

        private IEnumerator Throw()
        {
            _hammerInstance = Instantiate(_conf.HammerPrefab);
            var hammer = _hammerInstance.AddComponent<Hammer>();
            hammer.TeamId = _player.TeamId;

            _spriteRendererOfHammerInstance = _hammerInstance.GetComponent<SpriteRenderer>();
            _hammerInstance.GetComponent<Collider2D>();

            UpdateVelocity();

            _hammerInstance.transform.position = _player.transform.position;
            _hammerInstance.transform.position += Vector3.right * _conf.Velocity * 2;


            _active = true;

            while (_active)
            {          
                _dir = (_hammerInstance.transform.position - _player.transform.position).normalized;
                _distance = Vector2.Distance(_hammerInstance.transform.position, _player.transform.position);

                if (_distance >= _conf.Range)
                {
                    _flyBack = true;
                }

                if(_flyBack)
                {
                    _hammerInstance.transform.position -= (Vector3)_dir * Math.Abs(_conf.Velocity);
                }else {
                    _hammerInstance.transform.position = new Vector2(_hammerInstance.transform.position.x + _conf.Velocity, _hammerInstance.transform.position.y);
                }

                if (_distance <= Math.Abs(_conf.Velocity))
                {
                    _flyBack = false;
                    _active = false;
                    Destroy(_hammerInstance);
                }

                yield return null;
            }
        }

        private void UpdateVelocity()
        {
            if (PlayerController.isLookingLeft)
            {
                _conf.Velocity = Math.Abs(_conf.Velocity) * -1;
                _spriteRendererOfHammerInstance.flipX = true;
            }

            if (!PlayerController.isLookingLeft)
            {
                _conf.Velocity = Math.Abs(_conf.Velocity);
                _spriteRendererOfHammerInstance.flipX = false;
            }    
        }
    }
}

