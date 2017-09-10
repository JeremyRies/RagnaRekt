﻿using System;
using Assets.Scripts.Util;
using Entities;
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

        private Cooldown _cooldown;

        private void Awake()
        {
            _cooldown = new Cooldown(_cooldownTimeInSeconds);
        }

        public override void TryToActivate(Direction direction)
        {
            if (_cooldown.IsOnCoolDown.Value) return;

            StartDash(direction);
            _cooldown.Start();
        }

        private void StartDash(Direction direction)
        {
            _animation.UseSkill().Subscribe(_ => MovePlayer(direction));
        }

        private void MovePlayer(Direction direction)
        {
            Visible = false;
            Observable.Timer(TimeSpan.FromSeconds(_dashTimeInSeconds)).Subscribe(_ => Visible = true);
            switch (direction)
            {
                case Direction.LEFT:
                    _player.transform.localPosition += new Vector3(-_dashRange, 0, 0);
                    break;
                case Direction.RIGHT:
                    _player.transform.localPosition += new Vector3(_dashRange, 0, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction", direction, null);
            }
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