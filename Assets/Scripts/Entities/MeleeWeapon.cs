using UnityEngine;

namespace Assets.Scripts.Entities
{
    [RequireComponent(typeof(Collider2D))]
    public class MeleeWeapon : MonoBehaviour
    {
        public bool Visible { set { gameObject.SetActive(value); } }

        private void Start()
        {
            Visible = false;
        }

        public void Attack()
        {
            Visible = true;
        }

    }
}