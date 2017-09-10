using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Control.Actions
{
    [RequireComponent(typeof(Collider2D))]
    public class HammerVsHammer : MonoBehaviour
    {
        [SerializeField]
        public Hammer Hammer;
        [SerializeField]
        public PlayerControllerBase PlayerController;


        private void OnTriggerEnter2D(Collider2D other)
        {
            
            if (other.CompareTag("Weapon"))
            {

                Hammer._velocity = 0;
                StartCoroutine(HammerCollision(1));
                
            }

        }

        public IEnumerator HammerCollision(float sec)
        {
            yield return new WaitForSeconds(sec);
            Hammer._velocity = Hammer._hammerConfig.Velocity;
            Hammer.UpdateVelocity(PlayerController.isLookingLeft, Hammer);
            Hammer._flyBack = true;

        }

    }

}
