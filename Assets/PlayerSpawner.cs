﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private void Start()
    {
        Game.Player_Starting_Location = transform.position;
        Game.player.playerStartingY = transform.position.y;
    }
}
