using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : EntityController
{

    public bool Continue_Spawning = false;
    public int Amount_To_Spawn = 10;

    public int[] Enemys_To_Spawn;
    public float Spawn_Rate = 1f;
    public GameObject LocationReference;

    private int nextEnemyToSpawn = 0;
    float timeTracker = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(LocationReference)
            Destroy(LocationReference);

        timeTracker = 0f;
    }

    private void FixedUpdate()
    {
        timeTracker += Time.fixedDeltaTime;

        if (timeTracker > Spawn_Rate)
        {
            spawn();
            timeTracker = 0f;
        }
    }

    public void spawn()
    {
        if (Enemys_To_Spawn.Length > 0)
        {
            if (Game.SpawmEnemy(Enemys_To_Spawn[nextEnemyToSpawn], _transf.position))
            {

            }
        }
    }


}
