using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDemon : Enemy
{

    //private variables
    float destinationCount = 0;
    bool reachedCircleDestination = false;

    //public variables
    [Header("Demon")]
    public Animator anim_controller;

    public float circlePrecision = 16;
    public float cirlceDistance = 10;

    public override void Start()
    {
        base.Start();

        action = chase;

        if (!IsDummy) StartCoroutine(think());
    }

    public override void Update()
    {
        base.Update();

        //debugging
        gizmoSpheres.Add((transform.position, minSightDistance, Color.blue));

        //Use run on frame for actions that are not based on decision making
        runOnFrame?.Invoke();
    }

    //chases the player,  defined in enemy
    protected override void chase()
    {
        //set animation
        anim_controller.SetFloat("ForwardMovement", 2);
        
        base.chase();

        //Conditions to switch states
        if (distanceFromPlayer < minSightDistance)
        {
            action = attack;
        }
    }

    //deals damage to the player
    protected override void attack()
    {
        anim_controller.SetFloat("ForwardMovement" , 0);

        if (distanceFromPlayer < minSightDistance)
        {
            Game.player.takeDamage(damage);
        }
        //Conditions to switch states
        
        action = flee;
    }

    //defined in enemy
    protected override void flee()
    {
        //set animation
        anim_controller.SetFloat("ForwardMovement", 2);

        base.flee();

        //Conditions to switch states
        if (distanceFromPlayer > fleeDistance)
        {
            isFleeing = false;
            action = null;
            destinationCount = 0;
            runOnFrame = circle;
        }
    }

    //The demon sequentially plots a circle around the player, UNLIKE MOST STATES THIS IS DONE ON THE UPDATE
    void circle()
    {
        //set animation
        anim_controller.SetFloat("ForwardMovement", 2);
        
        //declare variable
        Vector3 destination;

        //Only calculates a new circle destination onces the first subdivision has been reached
        if (reachedCircleDestination)
        {
            //Calculate Destination
            float angle = Mathf.Deg2Rad * (360 / circlePrecision * destinationCount);
            destination = Game.player.transform.position + (new Vector3(Mathf.Sin(angle),0, Mathf.Cos(angle)) * cirlceDistance);


            //Debug Ray to Destination
            if (UseSight) Debug.DrawLine(_transf.position, destination,Color.green, 2f);

            //Set Destination
            agent.SetDestination(destination);

            //Set Reached Circle Boolean
            reachedCircleDestination = false;
        }


        //Conditions to switch states
        if (agent.remainingDistance < 2)
        {
            reachedCircleDestination = true;
            destinationCount++;
        }

        //Conditions to switch states
        if (destinationCount > circlePrecision)
        {
            runOnFrame = null;
            action = chase;
        }
    }

    //defined in enemy
    IEnumerator think()
    {
        //checks if the enemy can see the player
        _canSeePlayer = ObjectBlockingView();

        if (!IsDummy) action?.Invoke();

        yield return new WaitForSeconds(Game.getlevelOneAI());


        if (!IsDummy) StartCoroutine(think());
    }
}
