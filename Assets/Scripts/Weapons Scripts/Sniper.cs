using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapons
{
    


    // Start is called before the first frame update
    private void Start()
    {
        takes_Continuous_Input = false;
    }

    public override void Use()
    {
        if (!coolingDown)
        {
            RaycastHit[] hit = Physics.RaycastAll(_transf.position, _transf.forward);

            sortEnemies(ref hit);

            Debug.DrawRay(_transf.position, _transf.forward * 100f, Color.blue, coolDownDuration);

            if (hit.Length > 0)
            { 
                int damageLeft = damage;

                if (hit.Length == 1)
                {
                    Enemy enemy = hit[0].collider.gameObject.GetComponentInParent<Enemy>();
                    if (enemy)
                    {
                        enemy.takeDamage(damage, Game.player.gameObject);
                        bulletTrail(_transf.position, _transf.forward, hit[0].distance);
                    }
                }
                else
                {
                    for (int i = 0; i < hit.Length; i++)
                    {
                        LOG("(" + i + ") ENEMY HIT: " + hit[i].collider.name);

                        Enemy enemy = hit[i].collider.gameObject.GetComponentInParent<Enemy>();

                        if (enemy)
                        {
                            int enemyHealth = enemy.health;
                            enemy.takeDamage(damageLeft, Game.player.gameObject);

                            damageLeft -= enemyHealth;
                            LOG("BASE DAMAGE: " + damage);
                        }

                        if (damageLeft <= 0)
                        {
                            bulletTrail(_transf.position, _transf.forward, hit[i].distance);
                        }
                    }
                }
            }
            else
            {
                bulletTrail(_transf.position, _transf.forward, 100f);
            }

            coolingDown = true;
            StartCoroutine(coolDown(coolDownDuration));
        }
    }

    void sortEnemies(ref RaycastHit[] hit)
    {
        int i, j;
        for (i = 1; i <hit.Length; i++)
        {
            j = i;
            while (j > 0 && hit[j - 1].distance > hit[j].distance)
            {

                RaycastHit temp = hit[j];
                hit[j]=hit[j - 1];
                hit[j - 1] = temp;

                j--;
            }
        }
    }

}
