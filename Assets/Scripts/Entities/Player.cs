﻿using System;
using Control;
using LifeSystem;
using UnityEngine;

namespace Entities
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerControllerBase _controller;
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] public PlayerAnimation Animation;

        public int PlayerId;
        public Team Team;
        public int OtherTeamId { get { return Team.TeamId == 1 ? 2 : 1; } }
        public void LookLeft() { _controller.UpdateViewDirection(new Vector2(-1,0)); }
        public Color Color { set { _sprite.color = value; } }

        [NonSerialized] public TeamPointSystem TeamPointSystem;

        void Start()
        {
            TeamPointSystem = FindObjectOfType<TeamPointSystem>();
        }
    }
}