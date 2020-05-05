using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : EntityController
{
    //private variables
    float timeTracker = 0;
    bool spawnerTriggered = false;
    int Amount_Spawned = 0;
    private int nextEnemyToSpawn = 0;
    float distanceFromPlayer;

    //public variables
    [Header("Behavior")]
    public bool SpawnForever = false;
    public float spawnDistance = 10f;
    public int amountToSpawn = 10;
    public float spawnRate = 1f;

    [Header("Control")]
    public int[] Enemys_To_Spawn;
    
    [Header("Design")]
    public GameObject LocationReference;

    void Start()
    {
        if(LocationReference)
            Destroy(LocationReference);

        Game.game.spawners.Add(this);

        timeTracker = 0f;
    }

    private void FixedUpdate()
    {
        //calculations
        distanceFromPlayer = Vector3.Distance(_transf.position, Game.player.transform.position);

        //counts to the amount of spawn rate, calls a function and then resets to zero
        timeTracker += Time.fixedDeltaTime;
        if (Game.player && timeTracker > spawnRate)
        {
            spawn();
            timeTracker = 0f;
        }
    }

    //Spawns an enemy if the spawner is triggered
    public void spawn()
    {
        //sets the spawner to triggered
        if (distanceFromPlayer <= spawnDistance)
        {
            spawnerTriggered = true;
        }

        //Spawns the enemy requested
        if (Enemys_To_Spawn.Length > 0 && spawnerTriggered)
        {
            if (Game.SpawmEnemy(Enemys_To_Spawn[nextEnemyToSpawn], _transf.position))
            {
                //increas the amount spawned by 1
                Amount_Spawned++;

                //if reached spawn cap delete this object
                if (Amount_Spawned >= amountToSpawn)
                {
                    Game.game.spawners.Remove(this);
                    Destroy(gameObject);
                   
                }
            }
        }
    }

}
