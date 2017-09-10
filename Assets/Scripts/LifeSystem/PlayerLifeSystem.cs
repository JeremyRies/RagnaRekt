using System.Collections;
using Control;
using Entities;
using Sound;
using UnityEngine;
using UniRx;

namespace LifeSystem
{
    public class PlayerLifeSystem : MonoBehaviour
    {
        private bool _invincible;

        private bool Invincible
        {
            get { return _invincible; }
            set
            {
                _invincible = value;
                _hitBox.enabled = !value;
            }
        }

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
            if(Invincible) Debug.LogError("Should not be able to hit when invincible");
            //audio
            Die();
            _player.Animation.Die().Subscribe(_ => Respawn());
        }

        private void Die()
        {
            Invincible = true;
            PlayDeathSound();
            _player.GetPlayerController().DisableInput();
            _player.TeamPointSystem.ScorePoint(_player.OtherTeam);
        }

        private void PlayDeathSound()
        {
            var clip = _player.HeroType == HeroType.Thor ? ClipIdentifier.ThorDeath : ClipIdentifier.LokiDeath;
            SfxSound.SfxSoundInstance.PlayClip(clip);
        }

        private void StartInvincibilityAfterDeath()
        {
            StartCoroutine(Invincibility(_invincibilityTimeAfterDeath));
        }

        public void SetInvincible(float invincibilityTime)
        {
            StartCoroutine(Invincibility(invincibilityTime));
        }

        private IEnumerator Invincibility(float invincibilityTime)
        {
            Invincible = true;

            float currentInvincibilityTime = 0f;

            while (currentInvincibilityTime < invincibilityTime)
            {
                currentInvincibilityTime += _timeBetweenInvincibilityAnimation;
                _sprite.enabled = !_sprite.enabled;
                yield return new WaitForSeconds(_timeBetweenInvincibilityAnimation);
            }
            _sprite.enabled = true;

            Invincible = false;
        }

        private void Respawn()
        {
            _player.GetPlayerController().EnableInput();

            var xpos = Random.Range(_levelConfig.LevelLeftMaxPosition, _levelConfig.LevelRightMaxPosition);
            var pos = new Vector2(xpos,_levelConfig.LevelYMaxPosition);
            _playerTransform.position = pos;

            StartInvincibilityAfterDeath();
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
                Die();
                Respawn();
            }
        }
    }
}