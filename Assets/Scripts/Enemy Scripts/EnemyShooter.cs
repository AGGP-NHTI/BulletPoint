using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyShooter : Enemy
{
    //private variables
    int whichFace = 0;

    //public variables
    [Header("Liar's Head")]
    public GameObject SpitProjectile;
    public GameObject[] faces;

    public float ScareDistance = 8f;
    public float attackDistance = 15f;
    public float faceFOV = 10f;
    public float spinSpeed = 10f;

    public override void Start()
    {
        //Setup the first state of the enemy
        base.Start();
        action = idle;
        runOnFrame = null;

        //Debugging Purposes
        if(!IsDummy) StartCoroutine(Think());
    }

    public override void Update()
    {

        base.Update();

        //Debugging
        gizmoSpheres.Add((transform.position, ScareDistance,Color.black));
        gizmoSpheres.Add((transform.position, attackDistance, Color.red));
        gizmoSpheres.Add((transform.position, fleeDistance, Color.blue));
        if (UseSight) Debug.DrawRay(faces[0].transform.position, faces[0].transform.forward * sightDistance, Color.red);
        if (UseSight) Debug.DrawRay(faces[1].transform.position, faces[1].transform.forward * sightDistance, Color.red);
        if (UseSight) Debug.DrawRay(faces[2].transform.position, faces[2].transform.forward * sightDistance, Color.red);

        //Use run on frame for actions that are not based on decision making
        runOnFrame?.Invoke();
    }

    //Moves in a random direction - Defined in the enemy class, selects which state to change to
    protected override void idle()
    {
        runOnFrame = null;

        MoveRandomly();

        LOG("Can see player: " + _canSeePlayer + " Distance From Player: " + distanceFromPlayer);
        //Conditions to switch states
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

    //rotates and shoots when a face is looking at the player
    protected override void attack()
    {
        runOnFrame = rotateAndShoot;

        //Conditions to switch states
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

    //chases the player, defined in enemy class
    protected override void chase()
    {
        runOnFrame = null;

        base.chase();


        //Conditions to switch states
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

    //defined in enemy class
    protected override void flee()
    {
        runOnFrame = null;

        base.flee();

        //Conditions to switch states
        if (distanceFromPlayer > fleeDistance)
        {
            isFleeing = false;
            action = chase;
        }
    }

    //returns the inputted object if the player is in its field of view
    protected virtual GameObject inFaceFOV(GameObject GO, float FOVWanted)
    { 
        Vector3  targetDir = Game.player.transform.position - GO.transform.position;
        float angle = Vector3.Angle(targetDir, GO.transform.forward);

        if (angle < FOVWanted)
        {
            return GO;
        }
        else
        {
            return null;
        }
    }

    //rotates, every 120 degrees, shoots a projectile
    void rotateAndShoot()
    {
        //rotates the faces
        _transf.Rotate(Vector3.up * -spinSpeed * Time.deltaTime, Space.World);
        GameObject whoIs = inFaceFOV(faces[whichFace], faceFOV);

        if (whoIs)
        {
            //function  defined in enemy class
            projectileShoot(whoIs, Game.player.gameObject, SpitProjectile);

            //cycles through which face is shooting
            whichFace++;
            if (whichFace == 3)
            {
                whichFace = 0;
            }
        }
    }

    //defined in enemy class
    protected override IEnumerator Think()
    {
        //determines if the player is visible
        _canSeePlayer = inSightRange() && !ObjectBlockingView();



        action.Invoke();

        yield return new WaitForSeconds(Game.getlevelThreeAI());
        if (!IsDummy)
        {
            StartCoroutine(Think());
        }
    }

}
