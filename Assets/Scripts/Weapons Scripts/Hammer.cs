using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapons
{
    [Header("Hammer")]
    public Vector3 positionOffset;
    public float hitRange = 5f;

    // Start is called before the first frame update

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;

        Vector3 playerOffset = Game.player.transform.TransformPoint(positionOffset);
        Gizmos.DrawWireSphere(playerOffset, hitRange);
    }

    public override void Use()
    {
        if (!coolingDown)
        {
            Vector3 playerOffset = Game.player.transform.TransformPoint(positionOffset);
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
