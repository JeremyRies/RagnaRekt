using System;
using System.Collections;
using Assets.Scripts.Util;
using UnityEngine;
using Entities;
using Sound;
using UnityEditor.Animations;
using UniRx;

namespace Control.Actions
{
    public class HammerThrow : Action
    {
        [SerializeField] private Player _player  ;
        [SerializeField] private HammerConfig _conf;

        [SerializeField] public PlayerControllerBase PlayerController;
        [SerializeField] private PlayerAnimation _animation;

        [SerializeField] private AnimatorController _thorWithHammer;
        [SerializeField] private AnimatorController _thorWithoutHammer;

        private Vector2 _dir;
        private float _distanceFromStartPoint;
        private float _distanceToPlayer;

        [NonSerialized] public bool IsInHand = true;
        private Cooldown _cooldown;
        private GameObject _hammerInstance;
        private SpriteRenderer _spriteRendererOfHammerInstance;
        private BoxCollider2D _colliderOfHammerInstance;

        private void Start()
        {
            _cooldown = new Cooldown(_conf.CooldownTimeInSeconds);
            _cooldown.IsOnCoolDown.Where(cd => !cd).Subscribe(_ => OnCooldown = false);
        }

        public override void TryToActivate(Direction direction)
        {
            if (_cooldown.IsOnCoolDown.Value) return;
            Debug.Log("Hi");
            _cooldown.Start();
            StartCoroutine(Throw());
            OnCooldown = true;
        }

        private bool OnCooldown
        {
            set { _player._sprite.color = value ? new Color(0.8F,0.8F,0.8F) : Color.white; }
        }

        private IEnumerator Throw()
        {
            SfxSound.SfxSoundInstance.PlayClip(ClipIdentifier.ThorSkillHammerThrow);

            _hammerInstance = Instantiate(_conf.HammerPrefab);
            var hammer = _hammerInstance.GetComponent<Hammer>();
            var hammerVs = _hammerInstance.GetComponent<HammerVsHammer>();

            hammerVs.PlayerController = PlayerController;
            hammer._hammerConfig = _conf;
            hammer.TeamId = _player.Team.TeamId;
                   
            _spriteRendererOfHammerInstance = _hammerInstance.GetComponent<SpriteRenderer>();
            
            hammer.SpriteRendererOfHammerInstance = _spriteRendererOfHammerInstance;

            _colliderOfHammerInstance = _hammerInstance.GetComponent<BoxCollider2D>();

            hammer.ColliderOfHammerInstance = _colliderOfHammerInstance;

            hammer.Velocity = Math.Abs(_conf.Velocity);
            hammer.UpdateVelocity(PlayerController.isLookingLeft,hammer);
            
            IsInHand = false;

            _hammerInstance.transform.position = _player.transform.position;
            _hammerInstance.transform.position += Vector3.right * hammer.Velocity * 2;
            _animation.Controller = _thorWithoutHammer;
            _animation.UseSkill();

            var playerStartPos = _player.transform.position;

            while (IsInHand==false)
            {          
                _dir = (_hammerInstance.transform.position - _player.transform.position).normalized;
                _distanceFromStartPoint = Vector2.Distance(_hammerInstance.transform.position, playerStartPos);
                _distanceToPlayer = Vector2.Distance(_hammerInstance.transform.position, _player.transform.position);

                hammer.Update();
                

                if (_distanceFromStartPoint >= _conf.Range)
                {
                    hammer.FlyBack = true;
                    SfxSound.SfxSoundInstance.PlayClip(ClipIdentifier.ThorSkillHammerReturn);
                }

                if (_cooldown.IsOnCoolDown.Value==false)
                {
                    IsInHand = true;
                    _animation.UseSkill().Subscribe(_ => _animation.Controller = _thorWithHammer);
                }

                if (hammer.FlyBack)
                {
                    _hammerInstance.transform.position -= (Vector3)_dir * Math.Abs(hammer.Velocity);
                }else {
                    _hammerInstance.transform.position = new Vector2(_hammerInstance.transform.position.x + hammer.Velocity, _hammerInstance.transform.position.y);
                }

                if (_distanceToPlayer <= Math.Abs(hammer.Velocity * 5) && hammer.FlyBack )
                {
                    SfxSound.SfxSoundInstance.Stop();
                    hammer.FlyBack = false;
                    IsInHand = true;
                    _animation.UseSkill().Subscribe(_ => _animation.Controller = _thorWithHammer);
                }

                yield return null;
            }
           
            hammer.Reset();
        }

      
    }
}

