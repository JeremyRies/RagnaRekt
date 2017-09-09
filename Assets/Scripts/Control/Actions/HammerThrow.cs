using Control.Actions;
using Assets.Scripts.Util;
using UnityEngine;
using System.Collections;
using System;
using Control;


public class HammerThrow : Control.Actions.Action
{
    [SerializeField]
    private float _cooldownTimeInSeconds;
    [SerializeField]
    public GameObject Hammer;
    [SerializeField]
    public float velocity;
    [SerializeField]
    public float range;
    [SerializeField]
    public PlayerControllerBase PlayerController;


   
    Vector2 Dir;
    float distance;
    bool flyBack;
    private Cooldown _cooldown;
    private Cooldown _hammerReturn;

    private void Start()
    {

        _cooldown = new Cooldown(_cooldownTimeInSeconds);
        _hammerReturn = new Cooldown(_cooldownTimeInSeconds / 2);
    }

    public override void TryToActivate(Direction direction)
    {
        if (_cooldown.IsOnCoolDown.Value==false)
        {
            updateVelocity(direction);
            _cooldown.Start();
            _hammerReturn.Start();
            StartCoroutine(Throw(direction));
        }

    }

 

   public  IEnumerator Throw(Direction direction)
    {

        Hammer.SetActive(true);
        Hammer.transform.position = new Vector2(gameObject.transform.position.x + velocity*2, gameObject.transform.position.y);



        while (Hammer.active==true)
        {
            
            Dir = (Hammer.transform.position - gameObject.transform.position).normalized;
            distance = Vector2.Distance(Hammer.transform.position, gameObject.transform.position);

            Debug.Log(distance);

            if (distance >= range)
            {
                flyBack = true;
            }

            if(flyBack == false)
            {
            
            Hammer.transform.position = new Vector2(Hammer.transform.position.x  +  velocity, Hammer.transform.position.y);

            }else {
                
                Hammer.transform.position -= (Vector3)Dir *Math.Abs(velocity);
            }

            if (distance <= Math.Abs(velocity))
            {
                flyBack = false;
                Hammer.SetActive(false);

            }


            yield return null;
        }

        
    }
   public void updateVelocity(Direction direction)
    {
        if(PlayerController.isLookingLeft)
        { velocity = Math.Abs(velocity) * -1; }

        if (!PlayerController.isLookingLeft)
        { velocity = Math.Abs(velocity); }

    }
    

}

