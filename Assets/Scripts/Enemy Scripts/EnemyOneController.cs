using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyOneController : Enemy
{
 
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        runOnFrame = rotate;
        action = idle;
        StartCoroutine(Think());
        
    }

    // Update is called once per frame
    public override void Update()
    {
        runOnFrame?.Invoke();

        
        

    }

    private void FixedUpdate()
    {
        
    }

    protected override IEnumerator Think()
    {

        _canSeePlayer = inMinSightRange() || (inSightRange() && inFOV() && !ObjectBlockingView());


        
        action.Invoke();


        yield return new WaitForSeconds(Game.getlevelTwoAI());
        StartCoroutine(Think());


    }

    protected override void idle()
    {
        runOnFrame = rotate;
        
        
        
        if (_canSeePlayer)
        {
            
            action = attack;
        }
    }

    protected override void attack()
    {
        runOnFrame = null;
        agent.speed = moveSpeed;
        agent.SetDestination(Game.player.transform.position);

        if (!_canSeePlayer)
        {
            action = idle;
        }
    }

 

    
    

}
