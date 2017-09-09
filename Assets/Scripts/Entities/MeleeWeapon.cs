using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class MeleeWeapon : MonoBehaviour
    {
        private GameObject _gameObject;

        public bool Visible { set { _gameObject.SetActive(value); } } 


    }
}