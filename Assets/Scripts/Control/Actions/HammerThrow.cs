using System;
using System.Collections;
using Assets.Scripts.Util;
using UnityEngine;

namespace Control.Actions
{
    public class HammerThrow : Action
    {
        [SerializeField] private float _cooldownTimeInSeconds;
        [SerializeField] public GameObject Player;
        [SerializeField] public float Velocity;
        [SerializeField] public float Range;
        [SerializeField] public PlayerControllerBase PlayerController;
        [SerializeField] private GameObject _hammerPrefab;

        private Vector2 _dir;
        private float _distance;
        private bool _flyBack;
        private Cooldown _cooldown;
        private Cooldown _hammerReturn;
        private bool _active;
        private GameObject _hammerInstance;
        private SpriteRenderer _spriteRendererOfHammerInstance;

        private void Start()
        {
            _cooldown = new Cooldown(_cooldownTimeInSeconds);
            _hammerReturn = new Cooldown(_cooldownTimeInSeconds / 2);
        }

        public override void TryToActivate(Direction direction)
        {
            if (_cooldown.IsOnCoolDown.Value) return;

            _cooldown.Start();
            _hammerReturn.Start();
            StartCoroutine(Throw());
        }

        private IEnumerator Throw()
        {
            _hammerInstance = Instantiate(_hammerPrefab);
            _spriteRendererOfHammerInstance = _hammerInstance.GetComponent<SpriteRenderer>();

            UpdateVelocity();

            _hammerInstance.transform.position = Player.transform.position;
            _hammerInstance.transform.position += Vector3.right * Velocity * 2;


            _active = true;

            while (_active)
            {          
                _dir = (_hammerInstance.transform.position - Player.transform.position).normalized;
                _distance = Vector2.Distance(_hammerInstance.transform.position, Player.transform.position);

                Debug.Log(_distance);

                if (_distance >= Range)
                {
                    _flyBack = true;
                }

                if(_flyBack)
                {
                    _hammerInstance.transform.position -= (Vector3)_dir * Math.Abs(Velocity);
                }else {
                    _hammerInstance.transform.position = new Vector2(_hammerInstance.transform.position.x + Velocity, _hammerInstance.transform.position.y);
                }

                if (_distance <= Math.Abs(Velocity))
                {
                    _flyBack = false;
                    _active = false;
                    Destroy(_hammerInstance);
                }

                yield return null;
            }
        }

        private void UpdateVelocity()
        {
            if (PlayerController.isLookingLeft)
            {
                Velocity = Math.Abs(Velocity) * -1;
                _spriteRendererOfHammerInstance.flipX = true;
            }

            if (!PlayerController.isLookingLeft)
            {
                Velocity = Math.Abs(Velocity);
                _spriteRendererOfHammerInstance.flipX = false;
            }    
        }
    }
}

