using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapons
{
    

    public override void Use()
    {
        if (!coolingDown)
        {
            
            float tracerLength = 100f;
            Vector3 shootDir = Shoot_Node.transform.forward;
            shootDir.y = 0;
            Vector3 origin = Shoot_Node.transform.position;


            RaycastHit[] hit = Physics.RaycastAll(origin, shootDir);


            sortEnemies(ref hit);

            Debug.DrawRay(origin, shootDir * 100f, Color.blue, coolDownDuration);

            if (hit.Length > 0)
            { 
                int damageLeft = damage;

                if (hit.Length == 1)
                {
                    Enemy enemy = hit[0].collider.gameObject.GetComponentInParent<Enemy>();
                    if (enemy)
                    {
                        enemy.takeDamage(damage, Game.player.gameObject);
                        tracerLength = hit[0].distance;
                    }
                }
                else
                {
                    for (int i = 0; i < hit.Length; i++)
                    {

                        Enemy enemy = hit[i].collider.gameObject.GetComponentInParent<Enemy>();

                        if (enemy)
                        {
                            int enemyHealth = enemy.health;
                            enemy.takeDamage(damageLeft, Game.player.gameObject);

                            damageLeft -= enemyHealth;
                        }

                        if (damageLeft <= 0)
                        {
                            tracerLength = hit[i].distance;
                        }
                    }
                }
            }
       

            bulletTrail(origin, shootDir, tracerLength);
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
