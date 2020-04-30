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
            RaycastHit[] hit = Physics.RaycastAll(_transf.position, _transf.forward, ~(1 << 9));

            sortEnemies(ref hit);

            Debug.DrawRay(_transf.position, _transf.forward * 100f, Color.blue, coolDownDuration);
            bulletTrail(_transf.position,_transf.forward,100f);
            if (hit.Length > 0)
            {
                
                int damageLeft = damage;
                for (int i = 0; i < hit.Length; i++)
                {
                    //LOG("("+ i + ") ENEMY HIT: " + hit[i].collider.name);
                    Enemy enemy = hit[i].collider.gameObject.GetComponent<Enemy>();
                    if (enemy)
                    {
                        int damageDealing = damageLeft;
                        damageLeft -= enemy.health;

                        if (damageLeft <= 0 && i > 0) { break; }
                        //LOG("DAMAGE LEFT: "+ damageLeft);
                        enemy.GetComponentInParent<Enemy>().takeDamage(damageDealing, Game.player.gameObject);
                    }
                    else
                    {
                        i = hit.Length;
                        break;
                    }
                }
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
