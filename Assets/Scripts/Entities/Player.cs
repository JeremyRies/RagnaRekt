using System;
using Control;
using GameLogic;
using LifeSystem;
using UnityEngine;

namespace Entities
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerController _controller;
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private PlayerAnimation _animation;
        [SerializeField] private SpriteRenderer _playerIdSprite;
        [SerializeField] private PlayerIdConfig _playerIdConfig;

        private int _playerId;

        public int PlayerId
        {
            get { return _playerId; }
            set
            {
                _playerId = value;
                _playerIdSprite.sprite = _playerIdConfig.GetSpriteForPlayer(value);
            }
        }

        public Team Team;

        public HeroType HeroType;

        public Team OtherTeam { get { return TeamPointSystem.GetOtherTeam(Team); } }
        public void LookLeft() { _controller.UpdateViewDirection(new Vector2(-1,0)); }
        public Color Color { set { _sprite.color = value; } }

        public IInputProvider InputProvider
        {
            set { _controller.Initialize(value); }
        }

        public PlayerAnimation Animation
        {
            get { return _animation; }         
        }

        [NonSerialized] public TeamPointSystem TeamPointSystem;

        void Start()
        {
            TeamPointSystem = FindObjectOfType<TeamPointSystem>();
        }

        public PlayerController GetPlayerController()
        {
            return _controller;
        }
    }

    public enum HeroType
    {
        Thor =1,
        Loki =2,
    }
}