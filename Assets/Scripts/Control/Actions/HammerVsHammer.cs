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
        [SerializeField]
        public float collisionSeconds;

       

        private void OnTriggerEnter2D(Collider2D other)
        {
            
            if (other.CompareTag("Weapon"))
            {

                Hammer._velocity = 0;
                Hammer.GetComponent<Animator>().SetBool("HammerClash", true);
                StartCoroutine(HammerCollision(collisionSeconds));
                
            }

        }

        public IEnumerator HammerCollision(float sec)
        {
            yield return new WaitForSeconds(sec);
            Hammer._flyBack = true;
            Hammer._velocity = Hammer._hammerConfig.Velocity;
            Hammer.UpdateAnimation();
            Hammer.UpdateVelocity(PlayerController.isLookingLeft, Hammer);
            

        }

    }

}
