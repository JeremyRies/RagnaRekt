using System.Collections.Generic;
using Control;
using Entities;
using LifeSystem;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    [RequireComponent(typeof(Collider2D))]
    public class HitBox : MonoBehaviour
    {
        [SerializeField] private List<Collider2D> _ignoredColliders;
        [SerializeField] private PlayerLifeSystem _life;
        [SerializeField] private Player _player;


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_ignoredColliders.Contains(other)) return;
            if (!other.CompareTag("Weapon")) return;
            var otherTeamId = other.GetComponent<Killable>().TeamId;
            if (otherTeamId == _player.TeamId)return;

            Debug.Log("Player died");
            _life.ReceiveHit();
        }
    }
}