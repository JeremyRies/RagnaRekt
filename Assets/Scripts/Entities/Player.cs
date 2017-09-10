using System;
using Control;
using LifeSystem;
using UnityEditor;
using UnityEngine;

namespace Entities
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerControllerBase _controller;
        [SerializeField] public SpriteRenderer _sprite;
        [SerializeField] public PlayerAnimation Animation;
        [SerializeField] public SpriteRenderer _playerIdSprite;
        [SerializeField] public PlayerIdConfig _playerIdConfig;

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

        [NonSerialized] public TeamPointSystem TeamPointSystem;

        void Start()
        {
            TeamPointSystem = FindObjectOfType<TeamPointSystem>();
            AssetDatabase.CreateAsset(new PlayerIdConfig(), "Assets/PlayerIdConfig.asset");
        }

        public PlayerControllerBase GetPlayerController()
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