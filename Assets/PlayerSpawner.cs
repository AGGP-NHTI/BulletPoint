using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Game.Player_Starting_Location = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
