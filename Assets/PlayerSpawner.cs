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
        yield return new WaitForEndOfFrame();

        Game.Player_Starting_Location = transform.position + offeset;
        Game.player.transform.position = Game.Player_Starting_Location;
        Game.player.playerStartingY = transform.position.y; 
    }
}
