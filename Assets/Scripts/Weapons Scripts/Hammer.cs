using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapons
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


            foreach (RaycastHit cast in hit)
            {
                Enemy enemy = cast.collider.gameObject.GetComponentInParent<Enemy>();

                if (enemy)
                {
                    enemy.takeDamage(damage);
                }
            }
            



            coolingDown = true;
            StartCoroutine(coolDown(coolDownDuration));
        }

    }
}
