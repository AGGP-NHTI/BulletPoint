using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDemon : Enemy
{
    public Animator anim_controller;


    public float Circle_Precision = 16;
    public float Circle_Distance = 10;

    float destinationCount = 0;
    bool reachedCircleDestination = false;
    public override void Start()
    {
        base.Start();

        action = chase;

        if (!isDummy)
        {
            StartCoroutine(think());
        }
    }


    public override void Update()
    {

        base.Update();
        gizmoSpheres.Add((transform.position, minSightDistance, Color.blue));

        runOnFrame?.Invoke();
        if (!isDummy) LOG(_obj?.name + "'s action is " + action?.Method.Name);


        
    }

    protected override void chase()
    {
        anim_controller.SetFloat("ForwardMovement", 2);
        base.chase();

        if (distanceFromPlayer < minSightDistance)
        {
            action = attack;
        }
    }

    protected override void attack()
    {
        anim_controller.SetFloat("ForwardMovement" , 0);
        Game.player.takeDamage(damage);

        if (distanceFromPlayer < minSightDistance)
        {
            action = flee;
        }
    }

    protected override void flee()
    {
        anim_controller.SetFloat("ForwardMovement", 2);
        base.flee();

        if (distanceFromPlayer > fleeDistance)
        {
            isFleeing = false;
            action = null;
            runOnFrame = circle;
        }
    }

    void circle()
    {
        anim_controller.SetFloat("ForwardMovement", 2);
        Vector3 destination;

        //LOG("Reached Destination: " + reachedCircleDestination);
        if (reachedCircleDestination)
        {
            //LOG("Destination Count: " + destinationCount);
            //LOG("Reached Destination: " + reachedCircleDestination);


            //Calculate Destination

            float angle = Mathf.Deg2Rad * (360 / Circle_Precision * destinationCount);
            destination = Game.player.transform.position + (new Vector3(Mathf.Sin(angle),0, Mathf.Cos(angle)) * Circle_Distance);


            //Debug Ray to Destination
            if (UseSight)
            {
                Debug.DrawLine(_transf.position, destination,Color.green, 2f);
            }


            //Set Destination
            agent.SetDestination(destination);


            //Set Reached Circle Boolean
            reachedCircleDestination = false;
        }


        //LOG("Remaining Distance: " + agent.remainingDistance);
        if (agent.remainingDistance < 2)
        {
            reachedCircleDestination = true;
            destinationCount++;
        }

        if (destinationCount > Circle_Precision)
        {
            //runOnFrame = null;
            action = attack;
        }


    }
    IEnumerator think()
    {

        _canSeePlayer = ObjectBlockingView();

        if (!isDummy) action?.Invoke();

        yield return new WaitForSeconds(Game.getlevelFourAI());


        if (!isDummy) StartCoroutine(think());
    }


}
