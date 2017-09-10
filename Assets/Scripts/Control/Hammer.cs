using Control.Actions;
using UnityEngine;
using System;
using System.Collections;

namespace Control

{
    public class Hammer : Killable
    {
        [NonSerialized]
        public bool FlyBack;

        [NonSerialized]
        public SpriteRenderer SpriteRendererOfHammerInstance;
        public BoxCollider2D ColliderOfHammerInstance;
        public GameObject Effect;

        [NonSerialized]
        public float Velocity;

        [NonSerialized]
        public HammerConfig _hammerConfig;

        public void Update()
        {
            UpdateAnimation();
        }



        public void UpdateAnimation()
        {
            if (FlyBack)
            {
                gameObject.GetComponent<Animator>().SetBool("HammerBack", true);
            }
        }

        internal void Reset()
        {
            
            Destroy(gameObject);
        }

        public void UpdateVelocity(bool isLookingLeft, Hammer hammer)
        {
            if (isLookingLeft == true)
            {
                hammer.Velocity = Math.Abs(hammer.Velocity) * -1;
                SpriteRendererOfHammerInstance.flipX = true;
                ColliderOfHammerInstance.offset = new Vector2(Math.Abs(ColliderOfHammerInstance.offset.x)*(-1), 0);
                Effect.transform.position = new Vector2(hammer.transform.position.x -1, 0);
            }

            if (isLookingLeft == false)
            {
                hammer.Velocity = Math.Abs(hammer.Velocity);
                SpriteRendererOfHammerInstance.flipX = false;
                ColliderOfHammerInstance.offset = new Vector2(Math.Abs(ColliderOfHammerInstance.offset.x), 0);
                Effect.transform.position = new Vector2(hammer.transform.position.x +1, 0);

            }
        }

     
    }
}