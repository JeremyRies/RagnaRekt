using System.Collections.Generic;
using LifeSystem;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    [RequireComponent(typeof(Collider2D))]
    public class HitBox : MonoBehaviour
    {
        [SerializeField] private List<Collider2D> _ignoredColliders;
        [SerializeField] private PlayerLifeSystem _life;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_ignoredColliders.Contains(other)) return;
            if (other.tag != "Weapon") return;

            Debug.Log("Player died");
            _life.ReceiveHit();
        }
    }
}