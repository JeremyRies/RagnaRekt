using System.Collections.Generic;
using Control;
using LifeSystem;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    [RequireComponent(typeof(Collider2D))]
    public class HitBox : MonoBehaviour
    {
        [SerializeField] private List<Collider2D> _ignoredColliders;
        [SerializeField] private PlayerLifeSystem _life;
        [SerializeField] private PlayerControllerBase _playerControllerBase;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other);
            if (_ignoredColliders.Contains(other)) return;
            if (!other.CompareTag("Weapon")) return;
            if (other.GetComponent<Killable>().TeamId == _playerControllerBase.TeamId)return;

            Debug.Log("Player died");
            _life.ReceiveHit();
        }
    }
}