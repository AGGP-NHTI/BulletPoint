using System.Collections;
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
	}

	public virtual void Update()
	{
		hpdisp?.UpdatePosition(transform.position);
	}

    protected virtual void dead(GameObject source = null)
    {
        if (this is Enemy)
        {
            Game.EnemyCount--;
        }

		LOG($"{gameObject.name} was killed by {(source.name??"karma")}");

        Destroy(_obj,2f);
    }

    public void takeDamage(int amount, GameObject source = null)
    {
        if (amount > 0)
        {
            health -= amount;

			if (health <= 0)
			{
				dead(source);
			}

			hpdisp?.UpdateHP(health, startingHealth);
		}
	}
}
