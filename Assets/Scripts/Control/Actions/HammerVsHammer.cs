using System.Collections;
using UnityEngine;

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
        public float CollisionSeconds;

       

        private void OnTriggerEnter2D(Collider2D other)
        {
            
            if (other.CompareTag("Weapon"))
            {

                Hammer.Velocity = 0;
                Hammer.GetComponent<Animator>().SetBool("HammerClash", true);
                StartCoroutine(HammerCollision(CollisionSeconds));
                
            }

        }

        public IEnumerator HammerCollision(float sec)
        {
            yield return new WaitForSeconds(sec);
            Hammer.FlyBack = true;
            Hammer.Velocity = Hammer._hammerConfig.Velocity;
            Hammer.UpdateAnimation();
            Hammer.UpdateVelocity(PlayerController.isLookingLeft, Hammer);

            

        }

    }

}
