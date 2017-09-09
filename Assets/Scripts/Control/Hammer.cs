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
    }
}