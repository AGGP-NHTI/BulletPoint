using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gluttony : Enemy
{

    public float ScareDistance = 8f;//when to flee
    public float fleeSpeed = 10f; //speed to flee
    public float fleeDistance = 25f; // how far to flee

    private bool isFleeing = false;

    public float attackDistance = 15f;

    public float faceFOV = 10f;


    public int whichFace = 0;

    public GameObject[] faces;



    float distanceFromPlayer;
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
        gizmoSpheres.Add((transform.position, ScareDistance, Color.black));
        gizmoSpheres.Add((transform.position, attackDistance, Color.red));
        gizmoSpheres.Add((transform.position, fleeDistance, Color.blue));

        LOG("Action is: " + action.Method.Name);

        Debug.DrawRay(faces[0].transform.position, faces[0].transform.forward * sightDistance, Color.red);
        Debug.DrawRay(faces[1].transform.position, faces[1].transform.forward * sightDistance, Color.red);
        Debug.DrawRay(faces[2].transform.position, faces[2].transform.forward * sightDistance, Color.red);


        runOnFrame?.Invoke();
        distanceFromPlayer = Vector3.Distance(_transf.position, Game.player.transform.position);
    }

    protected override void idle()
    {
        runOnFrame = null;

        MoveRandomly();

        if (_canSeePlayer && distanceFromPlayer > attackDistance)
        {
            action = chase;
        }
        else if (_canSeePlayer && distanceFromPlayer > ScareDistance && distanceFromPlayer < attackDistance)
        {
            whichFace = 0;
            action = attack;
        }
        else if (_canSeePlayer && distanceFromPlayer < ScareDistance)
        {
            isFleeing = false;
            action = flee;
        }
    }

    protected override void attack()
    {
        runOnFrame = rotate;

        LOG("ATTACKING");
        if (_canSeePlayer && distanceFromPlayer < ScareDistance)
        {
            isFleeing = false;
            action = flee;
        }
        else if (_canSeePlayer && distanceFromPlayer > attackDistance)
        {
            action = chase;
        }
    }

    private void chase()
    {
        runOnFrame = null;

        agent.SetDestination(Game.player.transform.position);

        if (!_canSeePlayer)
        {
            action = idle;
        }
        else if (_canSeePlayer && distanceFromPlayer > ScareDistance && distanceFromPlayer < attackDistance)
        {
            whichFace = 0;
            agent.SetDestination(_transf.position);
            action = attack;
        }
        else if (_canSeePlayer && distanceFromPlayer < ScareDistance)
        {
            isFleeing = false;
            action = flee;
        }

    }

    protected override void flee()
    {
        runOnFrame = null;

        LOG("IS FLEEING: " + isFleeing);
        if (!isFleeing)
        {
            LOG("IN FLEE___________________________________________________________________________________________________________");
            isFleeing = true;
            LOG("FLEE");

            agent.speed = fleeSpeed;
            Vector3 fleeDir = (_transf.position - Game.player.transform.position).normalized;
            Vector3 moveDestination = _transf.position + fleeDir * fleeDistance;
            moveDestination.y = _transf.position.y;

            if (!agent.SetDestination(moveDestination))
            {
                fleeDir *= -1;
                moveDestination = _transf.position + fleeDir * fleeDistance;
                moveDestination.y = _transf.position.y;
                agent.SetDestination(moveDestination);
            }
            Debug.DrawLine(_transf.position, moveDestination, Color.magenta, 5f);

        }

        if (!agent.pathPending && agent.remainingDistance < 1)
        {
            isFleeing = false;
        }

        if (distanceFromPlayer > fleeDistance)
        {
            LOG("LEAVING FLEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            agent.speed = moveSpeed;
            isFleeing = false;
            action = chase;
        }
    }



    protected override IEnumerator Think()
    {
        _canSeePlayer = inSightRange() && !ObjectBlockingView();



        action.Invoke();

        yield return new WaitForSeconds(Game.getlevelThreeAI());
        StartCoroutine(Think());
    }

    protected virtual GameObject inFaceFOV(GameObject GO, float FOVWanted)
    {
        Vector3 targetDir; // = player.transform.position - _transf.position;
        float angle; // = Vector3.Angle(targetDir, _transf.forward);

        targetDir = Game.player.transform.position - GO.transform.position;
        angle = Vector3.Angle(targetDir, GO.transform.forward);

        if (angle < FOVWanted)
        {
            return GO;
        }
        else
        {
            return null;
        }
    }

    protected override void rotate()
    {
        base.rotate();

        GameObject whoIs = inFaceFOV(faces[whichFace], faceFOV);

        if (whoIs)
        {
            projectileShoot(whoIs, Game.player);

            whichFace++;
            if (whichFace == 3)
            {
                whichFace = 0;
            }
        }
    }


}
