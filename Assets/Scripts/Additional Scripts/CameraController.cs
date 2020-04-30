using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{



    public Vector3 offset;


    // Update is called once per frame
    void Update()
    {
        if (Game.player)
        {
            transform.position = Game.player.transform.position + offset;
        }
        else
        {
            Debug.Log("Player Not Assigned");
        }
    }
}
