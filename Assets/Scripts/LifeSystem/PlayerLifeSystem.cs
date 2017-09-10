using System.Collections;
using Entities;
using UnityEngine;
using UniRx;

namespace LifeSystem
{
    public class PlayerLifeSystem : MonoBehaviour
    {
        private bool _invincible;

        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private Collider2D _hitBox;

        [SerializeField]
        private Player _player;

        private float _invincibilityTimeAfterDeath = 2f;
        private float _timeBetweenInvincibilityAnimation = 0.1f;

        public void ReceiveHit()
        {
            if(_invincible) Debug.LogError("Should not be able to hit when invincible");
            //audio

           _player.TeamPointSystem.ScorePoint(_player.OtherTeamId);
            Die();
            _player.Animation.Die().Subscribe(_ => Respawn());
        }

        private void Die()
        {
            StartCoroutine(Invincibility(_invincibilityTimeAfterDeath));
        }

        public void SetInvincible(float invincibilityTime)
        {
            StartCoroutine(Invincibility(invincibilityTime));
        }

        private IEnumerator Invincibility(float invincibilityTime)
        {
            _invincible = true;
            _hitBox.enabled = false;
            float currentInvincibilityTime = 0f;

            while (currentInvincibilityTime < invincibilityTime)
            {
                currentInvincibilityTime += _timeBetweenInvincibilityAnimation;
                _sprite.enabled = !_sprite.enabled;
                yield return new WaitForSeconds(_timeBetweenInvincibilityAnimation);
            }
            _sprite.enabled = true;
            _invincible = false;
            _hitBox.enabled = true;
        }

        private void Respawn()
        {
            var xpos = Random.Range(_levelConfig.LevelLeftMaxPosition, _levelConfig.LevelRightMaxPosition);
            var pos = new Vector2(xpos,_levelConfig.LevelYMaxPosition);
            _playerTransform.position = pos;
        }

        private void Update()
        {
            CheckFallingOff();
        }

        private void CheckFallingOff()
        {
            var positionY = transform.position.y;
            if (positionY < _levelConfig.LevelYDeathPosition)
            {
                ReceiveHit();
            }
        }
    }
}