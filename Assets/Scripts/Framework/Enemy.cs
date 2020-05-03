using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Pawn
{
    public bool isDummy;


    protected List<(Vector3 Location, float magnitude, Color color)> gizmoSpheres = new List<(Vector3 Location, float magnitude, Color color)>();
    protected List<(Vector3 Location, Vector3 dimensions, Color color)> gizmoCubes = new List<(Vector3 Location, Vector3 dimensions, Color color)>();
    protected System.Action action;

    protected System.Action runOnFrame;

    public NavMeshAgent agent;
    
    public int damage = 10;
    public float wantedFOV = 30f;
    public float sightDistance = 10f;
    public float minSightDistance = 2f;
    public float randomMoveFactor = 10f;
    public float spinSpeed = 10f;
    public bool UseSight;
    public float fleeSpeed = 10f; //speed to flee
    public float fleeDistance = 25f; // how far to flee

    public float distanceFromPlayer;
    protected float startingMoveSpeed;
    protected Vector3? lastKnownPlayerLocation;
    protected bool _canSeePlayer;
    protected float distance;

    protected bool isFleeing = false;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        startingMoveSpeed = agent.speed;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        gizmoSpheres.Add((transform.position, sightDistance, Color.yellow));
        gizmoSpheres.Add((transform.position, minSightDistance, Color.green));
        StartCoroutine(clearGizmos());




        distanceFromPlayer = Vector3.Distance(_transf.position, Game.player.transform.position);

        

        //if(!isDummy) LOG(_obj.name + "'s action is " + action.Method.Name);
    }
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

    protected virtual void idle()
    {

    }

    protected virtual void attack()
    {

    }

    protected virtual void flee()
    {
        runOnFrame = null;

        //LOG("IS FLEEING: " + isFleeing);
        if (!isFleeing)
        {
            //LOG("IN FLEE___________________________________________________________________________________________________________");
            isFleeing = true;
            //LOG("FLEE");

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
    }

    protected virtual void chase()
    {
        agent.SetDestination(Game.player.transform.position);
    }

    protected virtual IEnumerator Think()
    {
        yield return new WaitForSeconds(Game.getlevelThreeAI());
        StartCoroutine(Think());
    }

    protected virtual bool inSightRange()
    {
        distance = Vector3.Distance(Game.player.transform.position, _transf.position);
        return distance <= sightDistance;
    }

    protected virtual bool inFOV()
    {
        Vector3 targetDir; // = player.transform.position - _transf.position;
        float angle; // = Vector3.Angle(targetDir, _transf.forward);

        targetDir = Game.player.transform.position - _transf.position;
        angle = Vector3.Angle(targetDir, _transf.forward);

        return angle < wantedFOV;
    }

    protected virtual bool inMinSightRange()
    {
        Vector3 tranPos = _transf.position;
        Vector3 playerPos = Game.player.transform.position;
        return Vector3.Distance(tranPos, playerPos) <= minSightDistance;
    }

    protected virtual void MoveRandomly()
    {
        if (!agent.pathPending && agent.remainingDistance < 1)
        {
            Vector3 startingPos = _transf.position;
            Vector3 randomAddition = new Vector3(Random.Range(-1 * randomMoveFactor, 1 * randomMoveFactor), startingPos.y, Random.Range(-1 * randomMoveFactor, 1 * randomMoveFactor));

            Vector3 moveLocation = startingPos + randomAddition;

            if(UseSight) Debug.DrawLine(startingPos, moveLocation, Color.magenta, 5f);

            agent.SetDestination(moveLocation);
        }
    }

    protected virtual bool ObjectBlockingView()
    {
        Vector3 targetDir;
        targetDir = Game.player.transform.position - _transf.position;

        bool objectInWay = true;

        RaycastHit hit;
        Vector3 playerDirection = Game.player.transform.position - _transf.position;
        playerDirection.Normalize();


        if (UseSight) Debug.DrawRay(_transf.position, playerDirection * sightDistance, Color.red, Game.getlevelThreeAI());

        if (Physics.Raycast(_transf.position, playerDirection.normalized, out hit,sightDistance, ~(1 << 8)))
        {
            if (UseSight) Debug.DrawLine(_transf.position, hit.transform.position, Color.green, Game.getlevelTwoAI());
            if (hit.transform.name == Game.player.name)
            {
                objectInWay = false;
            }
        }

        return objectInWay;
    }

    protected virtual void rotateAndShoot()
    {
        _transf.Rotate(Vector3.up * -spinSpeed * Time.deltaTime, Space.World);

    }

    protected void projectileShoot(GameObject origin, GameObject projectile)
    {
        GameObject bullet = Instantiate(projectile, origin.transform.position, origin.transform.rotation);

        bullet.GetComponent<bulletScript>().owner = this;
    }
    protected void projectileShoot(GameObject origin, GameObject toward, GameObject projectile)
    {
        Vector3 lookDir = toward.transform.position - origin.transform.position;

        Quaternion rotation = Quaternion.LookRotation(lookDir, Vector3.up);

        GameObject bullet = Instantiate(projectile, origin.transform.position, rotation);
        bullet.GetComponent<bulletScript>().owner = this;
    }

    protected void rayCastShoot(GameObject origin)
    {

    }

}
