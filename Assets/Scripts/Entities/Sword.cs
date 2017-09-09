using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Sword : MonoBehaviour
    {
        private GameObject _gameObject;

        private void Start()
        {
            _gameObject = gameObject;
        }

        public bool Visible { set { _gameObject.SetActive(value); } } 


    }
}