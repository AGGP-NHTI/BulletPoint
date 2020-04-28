using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gluttony : Enemy
{


    public float attackDistance = 15f;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        action = idle;
        runOnFrame = null;
        StartCoroutine(Think());
    }

    public override void Update()
    {

        base.Update();

        
        runOnFrame?.Invoke();
        
    }


    protected override void attack()
    {
        
    }

    protected override void idle()
    {
        agent.SetDestination(Game.player.transform.position);
    }


    protected override IEnumerator Think()
    {
        _canSeePlayer = inSightRange() && !ObjectBlockingView();



        action.Invoke();

        yield return new WaitForSeconds(Game.getlevelThreeAI());
        StartCoroutine(Think());
    }

 

}
