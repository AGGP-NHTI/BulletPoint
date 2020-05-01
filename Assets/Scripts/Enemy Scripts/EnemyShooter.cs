using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyShooter : Enemy
{
    
    public GameObject projectile;

    public float ScareDistance = 8f;//when to flee

    public float attackDistance = 15f;

    public float faceFOV = 10f;


    public int whichFace = 0;

    public GameObject[] faces;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        action = idle;
        runOnFrame = null;
        if(!isDummy)
            StartCoroutine(Think());
    }

    public override void Update()
    {

        base.Update();
        gizmoSpheres.Add((transform.position, ScareDistance,Color.black));
        gizmoSpheres.Add((transform.position, attackDistance, Color.red));
        gizmoSpheres.Add((transform.position, fleeDistance, Color.blue));

        if (UseSight) Debug.DrawRay(faces[0].transform.position, faces[0].transform.forward * sightDistance, Color.red);
        if (UseSight) Debug.DrawRay(faces[1].transform.position, faces[1].transform.forward * sightDistance, Color.red);
        if (UseSight) Debug.DrawRay(faces[2].transform.position, faces[2].transform.forward * sightDistance, Color.red);


        runOnFrame?.Invoke();
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
        runOnFrame = rotateAndShoot;

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

    protected override void chase()
    {
        runOnFrame = null;

        base.chase();

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

        base.flee();

        if (distanceFromPlayer > fleeDistance)
        {
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
        if (!isDummy)
        {
            StartCoroutine(Think());
        }
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

    protected override void rotateAndShoot()
    {
        base.rotateAndShoot();

        GameObject whoIs = inFaceFOV(faces[whichFace], faceFOV);

        if (whoIs)
        {
            projectileShoot(whoIs, Game.player.gameObject, projectile);

            whichFace++;
            if (whichFace == 3)
            {
                whichFace = 0;
            }
        }
    }


}
