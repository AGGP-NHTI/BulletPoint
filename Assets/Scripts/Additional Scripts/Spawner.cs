using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : EntityController
{

    public float Spawn_Distance = 10f;

    public bool Continue_Spawning = false;
    public int Amount_To_Spawn = 10;

    int Amount_Spawned = 0;

    public int[] Enemys_To_Spawn;
    public float Spawn_Rate = 1f;
    public GameObject LocationReference;

    private int nextEnemyToSpawn = 0;
    float timeTracker = 0;
    bool spawnerTriggered = false;

    protected float distanceFromPlayer;
    // Start is called before the first frame update
    void Start()
    {
        if(LocationReference)
            Destroy(LocationReference);

        timeTracker = 0f;
    }

    private void FixedUpdate()
    {
        distanceFromPlayer = Vector3.Distance(_transf.position, Game.player.transform.position);
        timeTracker += Time.fixedDeltaTime;

        if (timeTracker > Spawn_Rate)
        {
            spawn();
            timeTracker = 0f;
        }
    }

    public void spawn()
    {
        if (distanceFromPlayer <= Spawn_Distance)
        {
            spawnerTriggered = true;
        }
        if (Enemys_To_Spawn.Length > 0 && spawnerTriggered)
        {
            if (Game.SpawmEnemy(Enemys_To_Spawn[nextEnemyToSpawn], _transf.position))
            {
                Amount_Spawned++;
                if (Amount_Spawned >= Amount_To_Spawn)
                {
                    Destroy(gameObject);
                }
            }
        }
    }


}
