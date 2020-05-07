using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapons
{
    float hitRange = 5f;
    float forwardOffset = 2f;
    int slashThroughAmount = 5;

    // Start is called before the first frame update

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;

        Vector3 playerLoc = Game.player.transform.position;
        Vector3 playerOffset = playerLoc + (Game.player.transform.forward * forwardOffset);
        Gizmos.DrawWireSphere(playerOffset, hitRange);
    }

    public override void Use()
    {
        if (!coolingDown)
        {
            Vector3 playerLoc = Game.player.transform.position;
            Vector3 playerOffset = playerLoc + (Game.player.transform.forward * forwardOffset);
            RaycastHit[] hit = Physics.SphereCastAll(playerOffset, hitRange, Game.player.transform.forward, 5f);
            
            sortEnemies(ref hit);
            int amountHit = slashThroughAmount;
            if (hit.Length <= slashThroughAmount)
            {
                amountHit = hit.Length;
            }
            int damageAmount = damage / amountHit;
            for (int i = 0; i < amountHit; i++)
            {
                Enemy enemy = hit[i].collider.gameObject.GetComponentInParent<Enemy>();

                if (enemy)
                {
                    enemy.takeDamage(damageAmount);
                }
            }



            coolingDown = true;
            StartCoroutine(coolDown(coolDownDuration));
        }
       
    }


    void sortEnemies(ref RaycastHit[] hit)
    {
        int i, j;
        for (i = 1; i < hit.Length; i++)
        {
            j = i;
            while (j > 0 && hit[j - 1].distance > hit[j].distance)
            {

                RaycastHit temp = hit[j];
                hit[j] = hit[j - 1];
                hit[j - 1] = temp;

                j--;
            }
        }
    }

}
