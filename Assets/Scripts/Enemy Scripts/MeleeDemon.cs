using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDemon : Enemy
{
    public override void Start()
    {
        base.Start();

        action = chase;

        //if (!isDummy)
        //{
            StartCoroutine(think());
        //}
    }


    public override void Update()
    {

        base.Update();
        gizmoSpheres.Add((transform.position, minSightDistance, Color.blue));

        runOnFrame?.Invoke();
    }


    protected override void chase()
    {
        base.chase();

        if (distanceFromPlayer < minSightDistance)
        {
            action = attack;
        }
    }

    protected override void attack()
    {
        
        Game.player.takeDamage((int)damage);
        action = flee;

    }

    protected override void flee()
    {
        base.flee();

        if (distanceFromPlayer > fleeDistance)
        {
            agent.speed = moveSpeed;
            isFleeing = false;
            action = chase;
        }
    }

    void circle()
    {
        
    }
    IEnumerator think()
    {

        _canSeePlayer = ObjectBlockingView();

        action.Invoke();

        yield return new WaitForSeconds(Game.getlevelOneAI());


        StartCoroutine(think());
    }


}
