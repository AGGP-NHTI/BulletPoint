using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    bool canMoveOn = false;


    bool noSpawners = false;
    bool timeNeededReached = false;

    float timeSinceEnteredLevel = 0;
    Collider transition;

    private void Start()
    {
        transition = gameObject.GetComponent<Collider>();
        transition.enabled = false;

        timeSinceEnteredLevel = 0;

        canMoveOn = Game.game.canAutoTransition;
    }

    private void Update()
    {
        if (!canMoveOn)
        {
            if (!timeNeededReached)
            {
                timeSinceEnteredLevel += Time.deltaTime;
                timeNeededReached = timeSinceEnteredLevel >= 10;
            }
            if (!noSpawners)
            {
                noSpawners = Game.game.spawners.Count <= 0;
            }
            canMoveOn = (noSpawners && timeNeededReached);
        }
        else if (!transition.enabled)
        {
            transition.enabled = canMoveOn;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();

        if (player && canMoveOn)
        {
            Game.LoadNextScene();
        }
    }

    
}
