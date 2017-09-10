using Control.Actions;
using UnityEngine;
using System;
using System.Collections;

namespace Control

{
    public class Hammer : Killable
    {
        [NonSerialized]
        public bool _flyBack;

        [NonSerialized]
        public SpriteRenderer _spriteRendererOfHammerInstance;

        [NonSerialized]
        public float _velocity;

        [NonSerialized]
        public HammerConfig _hammerConfig;

        public void Update()
        {
            UpdateAnimation();
        }



        public void UpdateAnimation()
        {
            if (_flyBack)
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
                hammer._velocity = Math.Abs(hammer._velocity) * -1;
                _spriteRendererOfHammerInstance.flipX = true;
            }

            if (isLookingLeft == false)
            {
                hammer._velocity = Math.Abs(hammer._velocity);
                _spriteRendererOfHammerInstance.flipX = false;
            }
        }
    }
}