using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Pawn
{
    //Protected Variables
    protected List<(Vector3 Location, float magnitude, Color color)> gizmoSpheres = new List<(Vector3 Location, float magnitude, Color color)>();
    protected List<(Vector3 Location, Vector3 dimensions, Color color)> gizmoCubes = new List<(Vector3 Location, Vector3 dimensions, Color color)>();
    protected System.Action action;

    protected System.Action runOnFrame;

    protected float distanceFromPlayer;
    
    protected Vector3? lastKnownPlayerLocation;
    protected bool _canSeePlayer;

    protected bool isFleeing = false;
    protected float startingMoveSpeed;
    //Private Variables


    //Public Variables
    [Header("Control")]
    public bool IsDummy;
    public bool UseSight;

    [Header("Objects")]
    public NavMeshAgent agent;

    [Header("Movement")]
    public float randomMoveFactor = 10f;

    [Header("Decisions")]
    public float wantedFOV = 30f;
    public float sightDistance = 10f;
    public float minSightDistance = 2f;
    public float fleeSpeed = 10f; 
    public float fleeDistance = 25f; 

    [Header("Attacking")]
    public int damage = 10;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        startingMoveSpeed = agent.speed;
    }

    // Update is called once per frame
    public virtual void Update()
    {

        //Add gizmos for sight
        gizmoSpheres.Add((transform.position, sightDistance, Color.yellow));
        gizmoSpheres.Add((transform.position, minSightDistance, Color.green));
        StartCoroutine(clearGizmos());


        //calculate data for every frame
        distanceFromPlayer = Vector3.Distance(_transf.position, Game.player.transform.position);

        //debugging
        //if (!IsDummy) LOG(_obj?.name + "'s action is " + action?.Method.Name);
    }

    //Debugging
    protected virtual void OnDrawGizmos()
    {
        if (UseSight)
        {
            foreach (var element in gizmoSpheres)
            {
                Gizmos.color = element.color;
                Gizmos.DrawWireSphere(element.Location, element.magnitude);
            }
            foreach (var element in gizmoCubes)
            {
                Gizmos.color = element.color;
                Gizmos.DrawCube(element.Location, element.dimensions);
            }

        }  
    }
    IEnumerator clearGizmos()
    {
        yield return new WaitForEndOfFrame();
        gizmoSpheres.Clear();
        gizmoCubes.Clear();
    }

    //Empty Functions
    protected virtual void idle()
    {

    }

    //Empty Functions
    protected virtual void attack()
    {

    }

    //Actions for every inheritor
    protected virtual void flee()
    {
        runOnFrame = null;

        //if not currently in the process of fleeing start fleeing
        if (!isFleeing)
        {
            //make this true so that it finishes current movement before starting another flee
            isFleeing = true;

            //set speed for this action
            agent.speed = fleeSpeed;

            //calculate movement specifics
            Vector3 fleeDir = (_transf.position - Game.player.transform.position).normalized;
            Vector3 moveDestination = _transf.position + fleeDir * fleeDistance;
            moveDestination.y = _transf.position.y;
            
            //if the destination is invalid recalulate the destination to run in the opposit direction
            if (!agent.SetDestination(moveDestination))
            {
                fleeDir *= -1;
                moveDestination = _transf.position + fleeDir * fleeDistance;
                moveDestination.y = _transf.position.y;
                agent.SetDestination(moveDestination);
            }

            //Show location traveling to
            if(UseSight) Debug.DrawLine(_transf.position, moveDestination, Color.magenta, 5f);

        }

        //if the path was already decided and reached close to destination get ready to flee to a new location
        if (!agent.pathPending && agent.remainingDistance < 1)
        {
            isFleeing = false;
        }
    }

    //chase to the players location
    protected virtual void chase()
    {
        agent.SetDestination(Game.player.transform.position);
    }
    
    //Checks if the enemy can see the player
    protected virtual bool inSightRange()
    {
        return distanceFromPlayer <= sightDistance;
    }

    //checks if the player is in the enemies Field of View
    protected virtual bool inFOV()
    {
        Vector3 DirectionFromPlayer = Game.player.transform.position - _transf.position;
        float angle = Vector3.Angle(DirectionFromPlayer, _transf.forward);
        return angle < wantedFOV;
    }

    //if the player is closer than the minimum sight range
    protected virtual bool inMinSightRange()
    {
        return distanceFromPlayer <= minSightDistance;
    }

    //moves the enemy to a random location by adding a random value to the x and z to the enemies position
    protected virtual void MoveRandomly()
    {
        //if the enemy is already moveing it will not start moving to a random location
        if (!agent.pathPending && agent.remainingDistance < 1)
        {
            Vector3 startingPos = _transf.position;
            
            Vector3 randomAddition = new Vector3(Random.Range(-1 * randomMoveFactor, 1 * randomMoveFactor), startingPos.y, Random.Range(-1 * randomMoveFactor, 1 * randomMoveFactor));

            Vector3 moveLocation = startingPos + randomAddition;

            //debugging
            if(UseSight) Debug.DrawLine(startingPos, moveLocation, Color.magenta, 5f);

            //resets the destination
            agent.SetDestination(moveLocation);
        }
    }

    //Returns true if there is not object in the way of the player from the enemies perspective
    protected virtual bool ObjectBlockingView()
    { 
        bool objectInWay = true;
        Vector3 playerDirection = Game.player.transform.position - _transf.position;
        RaycastHit hit;

        //draws a red line in the direction of the player
        if (UseSight) Debug.DrawRay(_transf.position, playerDirection * sightDistance, Color.red, Game.getlevelThreeAI());

        //Raycasts to the player
        if (Physics.Raycast(_transf.position, playerDirection.normalized, out hit,sightDistance, ~(1 << 8)))
        {
            //draws a green line to the object that was hit
            if (UseSight) Debug.DrawLine(_transf.position, hit.transform.position, Color.green, Game.getlevelTwoAI());


            //if player was the object hit then there was not object in the way
            PlayerManager player = hit.collider.gameObject.GetComponent<PlayerManager>();
            if (player)
            {
                objectInWay = false;
            }
        }

        return objectInWay;
    }

    //Shoots a given projectile forward 
    protected void projectileShoot(GameObject origin, GameObject projectile)
    {
        GameObject bullet = Instantiate(projectile, origin.transform.position, origin.transform.rotation);

        bullet.GetComponent<bulletScript>().owner = this;
    }

    //Shoots a given projectile toward a given object
    protected void projectileShoot(GameObject origin, GameObject toward, GameObject projectile)
    {
        Vector3 lookDir = toward.transform.position - origin.transform.position;

        Quaternion rotation = Quaternion.LookRotation(lookDir, Vector3.up);

        GameObject bullet = Instantiate(projectile, origin.transform.position, rotation);
        bullet.GetComponent<bulletScript>().owner = this;
    }

    //Recursive Repeating function to operate State machine
    protected virtual IEnumerator Think()
    {
        yield return new WaitForSeconds(Game.getlevelThreeAI());
        StartCoroutine(Think());
    }
}
