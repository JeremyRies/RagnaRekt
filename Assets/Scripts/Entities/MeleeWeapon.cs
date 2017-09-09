using UnityEngine;

namespace Assets.Scripts.Entities
{
    [RequireComponent(typeof(Collider2D))]
    public class MeleeWeapon : MonoBehaviour
    {
        public bool Visible { set { gameObject.SetActive(value); } }

        public void Attack()
        {
            Visible = true;
        }

    }
}