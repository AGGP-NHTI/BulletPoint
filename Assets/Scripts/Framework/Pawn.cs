using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : EntityController
{

    
    

    public int health = 10;
    protected int startingHealth;


    // Start is called before the first frame update
    public virtual void Start()
    {
        startingHealth = health;
        //LOG("Starting Health: " + startingHealth);
    }

    protected virtual void dead()
    {
        if (this is Enemy)
        {
            Game.EnemyCount--;
        }
        else
        {
            LOG(gameObject.name + " is dieing");
        }
        Destroy(_obj);
    }

    public void takeDamage(int howMuch)
    {

        health -= howMuch;
        //LOG(_obj.name + " has " + health + " health");

        if (health <= 0)
        {
            dead();
        }
    }

    public void takeDamage(int howMuch, GameObject source)
    {

        health -= howMuch;
        //LOG(source.name + " dealth "+ howMuch + " damage to" + _obj.name);
        //LOG(_obj.name + " now has " + health + " health");

        if (health <= 0)
        {
            dead();
        }
    }
}
