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
    public GameObject Player;
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
            
            updateVelocity();
            _cooldown.Start();
            _hammerReturn.Start();
            StartCoroutine(Throw(direction));
        }

    }

 

   public  IEnumerator Throw(Direction direction)
    {

        gameObject.GetComponent<SpriteRenderer>().enabled = true;

        gameObject.transform.position = new Vector2(Player.transform.position.x + velocity*2, Player.transform.position.y);



        while (gameObject.active==true)
        {
            
            Dir = ( gameObject.transform.position - Player.transform.position).normalized;
            distance = Vector2.Distance(gameObject.transform.position, Player.transform.position);

            Debug.Log(distance);

            if (distance >= range)
            {
                flyBack = true;
            }

            if(flyBack == false)
            {

                gameObject.transform.position = new Vector2(gameObject.transform.position.x  +  velocity, gameObject.transform.position.y);

            }else {

                gameObject.transform.position -= (Vector3)Dir *Math.Abs(velocity);
            }

            if (distance <= Math.Abs(velocity))
            {
                flyBack = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;

            }


            yield return null;
        }

        
    }
   public void updateVelocity()
    {
        if(PlayerController.isLookingLeft)
        { velocity = Math.Abs(velocity) * -1; }

        if (!PlayerController.isLookingLeft)
        { velocity = Math.Abs(velocity); }
        
        
    }
    

}

