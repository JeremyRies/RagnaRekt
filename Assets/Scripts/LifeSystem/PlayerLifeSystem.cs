﻿using System.Collections;
using UnityEngine;

namespace LifeSystem
{
    public class PlayerLifeSystem : MonoBehaviour
    {
        private bool _invincible;

        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Collider2D _hitBox;

        private float _invincibilityTime = 2f;
        private float _timeBetweenInvincibilityAnimation = 0.1f;
        private float _currentInvincibilityTime;

        public void ReceiveHit()
        {
            if(_invincible) Debug.LogError("Should not be able to hit when invincible");
            //audio
            Die();
            Respawn();
        }

        private void Die()
        {
            _currentInvincibilityTime = 0;
            StartCoroutine(Invincibility());
        }

        private IEnumerator Invincibility()
        {
            _invincible = true;
            _hitBox.enabled = false;
            while (_currentInvincibilityTime < _invincibilityTime)
            {
                _currentInvincibilityTime += _timeBetweenInvincibilityAnimation;
                _spriteRenderer.enabled = !_spriteRenderer.enabled;
                yield return new WaitForSeconds(_timeBetweenInvincibilityAnimation);
            }
            _spriteRenderer.enabled = true;
            _invincible = false;
            _hitBox.enabled = true;
        }

        private void Respawn()
        {
            var xpos = Random.Range(_levelConfig.LevelLeftMaxPosition, _levelConfig.LevelRightMaxPosition);
            var pos = new Vector2(xpos,_levelConfig.LevelYMaxPosition);
            transform.position = pos;
        }
    }
}