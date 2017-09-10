using System;
using System.Collections;
using Assets.Scripts.Util;
using UnityEngine;
using Entities;

namespace Control.Actions
{
    public class HammerThrow : Action
    {
        [SerializeField] private Player _player  ;
        [SerializeField] private HammerConfig _conf;

        [SerializeField] public PlayerControllerBase PlayerController;
        [SerializeField] private PlayerAnimation _animation;


        private Vector2 _dir;
        private float _distanceFromStartPoint;
        private float _distanceToPlayer;



        [NonSerialized] public bool _isInHand;
        private Hammer hammer;
        private Cooldown _cooldown;
        private GameObject _hammerInstance;
        private SpriteRenderer _spriteRendererOfHammerInstance;

        private void Start()
        {
            _cooldown = new Cooldown(_conf.CooldownTimeInSeconds);
            
        }

        public override void TryToActivate(Direction direction)
        {
            if (_cooldown.IsOnCoolDown.Value) return;

            _cooldown.Start();
            _animation.UseSkill();
            StartCoroutine(Throw(direction));
        }

        private IEnumerator Throw(Direction direction)
        {
            _hammerInstance = Instantiate(_conf.HammerPrefab);
            
            var hammer = _hammerInstance.AddComponent<Hammer>();

            hammer._hammerConfig = _conf;
         
            hammer.TeamId = _player.Team.TeamId;


            _spriteRendererOfHammerInstance = _hammerInstance.GetComponent<SpriteRenderer>();
            hammer._spriteRendererOfHammerInstance = _spriteRendererOfHammerInstance;

            _hammerInstance.GetComponent<Collider2D>();

            hammer._velocity = Math.Abs(_conf.Velocity);
            hammer.UpdateVelocity(PlayerController.isLookingLeft,hammer);
            
            _isInHand = false;

            _hammerInstance.transform.position = _player.transform.position;
            _hammerInstance.transform.position += Vector3.right * hammer._velocity * 2;


            var playerStartPos = _player.transform.position;

            while (_isInHand==false)
            {          
                _dir = (_hammerInstance.transform.position - _player.transform.position).normalized;
                _distanceFromStartPoint = Vector2.Distance(_hammerInstance.transform.position, playerStartPos);
                _distanceToPlayer = Vector2.Distance(_hammerInstance.transform.position, _player.transform.position);

                hammer.Update();
                

                if (_distanceFromStartPoint >= _conf.Range)
                {
                    hammer._flyBack = true;
                }

                if (_cooldown.IsOnCoolDown.Value==false)
                {

                    _isInHand = true;
                }

                if (hammer._flyBack)
                {
                    _hammerInstance.transform.position -= (Vector3)_dir * Math.Abs(hammer._velocity);
                }else {
                    _hammerInstance.transform.position = new Vector2(_hammerInstance.transform.position.x + hammer._velocity, _hammerInstance.transform.position.y);
                }

                if (_distanceToPlayer <= Math.Abs(hammer._velocity) && hammer._flyBack )
                {
                    hammer._flyBack = false;
                    _isInHand = true;

                }

                yield return null;
            }
            Debug.Log(_isInHand);
            hammer.Reset();
        }

      
    }
}

