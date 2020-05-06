using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Vector3 offeset = new Vector3(2, 0, -3);

    

    private void Start()
    {
        
        StartCoroutine(setPositions());
    }

    IEnumerator setPositions()
    {
        yield return new WaitUntil(Game.playerExists);
        yield return new WaitUntil(Game.player.isPlayerSetup);


        Debug.Log("SET THE PLAYERS LOCATION");
        Game.Player_Starting_Location = transform.position + offeset;
        Game.player.transform.position = Game.Player_Starting_Location;
    }


}
