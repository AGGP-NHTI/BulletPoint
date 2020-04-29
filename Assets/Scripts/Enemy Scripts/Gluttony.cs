﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gluttony : Enemy
{

    public GameObject liar;
    public float attackDistance = 15f;
    public int idleCount = 0;
    public bool StartedSpawning = false;
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
        if (idleCount > health)
        {
            action = spawning;
            idleCount = 0;
        }
        agent.SetDestination(Game.player.transform.position);
        idleCount++;
    }

    void spawning()
    {
        if (!StartedSpawning)
        {
            LOG("IN SPAWNING: and hasStarted Spawning: " + StartedSpawning);
            StartedSpawning = true;
            StartCoroutine(spawnLiars(5, 2f));
        }
    }

    protected override IEnumerator Think()
    {
        _canSeePlayer = inSightRange() && !ObjectBlockingView();



        action.Invoke();

        yield return new WaitForSeconds(Game.getlevelThreeAI());
        StartCoroutine(Think());
    }

    IEnumerator spawnLiars(int howMany, float spawnSpeed)
    {
        LOG("HOW MANY SPAWNING: " + howMany);
        if (howMany <= 0)
        {
            StartedSpawning = false;
            action = idle;
            yield return 0;
        }
        //Game.SpawmEnemy(Game.enemyPrefabs[0], _transf.position);

        Instantiate(liar, _transf.position, Quaternion.identity); 

        yield return new WaitForSeconds(2);
        StartCoroutine(spawnLiars(howMany - 1, spawnSpeed));
    }

}
