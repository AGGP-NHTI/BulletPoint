﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : EntityController
{
	
    
    public int health = 10;
    protected int startingHealth;
	
	HPDisplay hpdisp;

	public virtual void Start()
    {
        startingHealth = health;
		
		hpdisp = Instantiate(Game.game.hpDisplayPrefab, Game.game.canvas.transform).GetComponent<HPDisplay>();
        hpdisp.UpdateHP(health, startingHealth);
	}

	public virtual void Update()
	{
        if(hpdisp) hpdisp.UpdatePosition(transform.position);
	}

    protected virtual void dead(GameObject source = null, float timeUntilRemove = 1)
    {
        if (this is Enemy)
        {
            Game.EnemyCount--;
            Game.game.EnemiesLeft--;
        }
        Collider col = gameObject.GetComponent<Collider>();

        if (col)
        {
            col.enabled = false;
        }

        hpdisp.Remove();

        Destroy(_obj,timeUntilRemove);
    }

    public virtual void takeDamage(int amount, GameObject source = null)
    {
        if (amount > 0)
        {
            AudioManager.playBodyHit();
            health -= amount;

			if (health <= 0)
			{
				dead(source);
			}

			hpdisp?.UpdateHP(health, startingHealth);
		}
	}
}
