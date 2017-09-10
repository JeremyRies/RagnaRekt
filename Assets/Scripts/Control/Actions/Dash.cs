using System;
using Assets.Scripts.Util;
using Entities;
using LifeSystem;
using UnityEngine;
using UniRx;

namespace Control.Actions
{
    public class Dash : Action
    {
        [SerializeField] private float _cooldownTimeInSeconds = 1;
        [SerializeField] private Player _player;
        [SerializeField] private PlayerAnimation _animation;
        [SerializeField] private float _dashRange = 3;
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private float _dashTimeInSeconds = 0.1F;
        [SerializeField] private PlayerControllerBase _controller;
        [SerializeField] private PlayerLifeSystem _lifeSystem;


        private Cooldown _cooldown;

        private void Awake()
        {
            _cooldown = new Cooldown(_cooldownTimeInSeconds);
            _cooldown.IsOnCoolDown.Where(cd => !cd).Subscribe(_ => OnCooldown = false);
        }

        private bool OnCooldown
        {
            set { _player._sprite.color = value ? new Color(0.8F, 0.8F, 0.8F) : Color.white; }
        }

        public override void TryToActivate(Direction direction)
        {
            if (_cooldown.IsOnCoolDown.Value) return;

            StartDash(direction);
            _cooldown.Start();
            OnCooldown = true;
        }

        private void StartDash(Direction direction)
        {
            _animation.UseSkill().Subscribe(_ => MovePlayer(direction));
        }

        private void MovePlayer(Direction direction)
        {
            Visible = false;
            _lifeSystem.SetInvincible(_dashTimeInSeconds);

            Observable.Timer(TimeSpan.FromSeconds(_dashTimeInSeconds)).Subscribe(_ =>
            {
                Visible = true;
            });
            switch (direction)
            {
                case Direction.LEFT:
                    _controller.Controller.Move(new Vector2(-_dashRange, 0));
                    break;
                case Direction.RIGHT:
                    _controller.Controller.Move(new Vector2(_dashRange, 0));
                    break;
                case Direction.TOP:
                    break;
                case Direction.DOWN:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction", direction, null);
            }
            _controller.ArrestMovement();
        }

        private bool Visible
        {
            set
            {
                _sprite.enabled = value;
                _controller.enabled = value;
            }
        }
    }
}