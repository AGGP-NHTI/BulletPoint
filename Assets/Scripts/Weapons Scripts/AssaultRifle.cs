using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Weapons
{

    private float bulletSpreadMagnitude = 0f;
    public float standingBulletSpread = 0f;
    public float walkingBulletSpread = 0f;
    public float BulletSpread = 100f;
    private void Start()
    {
        takes_Continuous_Input = true;
    }

    

    public override void Use()
    {

        if (!coolingDown)
        {

            //Caluculateing Bullet Spread
            bulletSpreadMagnitude = Random.Range(-1f, 1f);
            if (Game.player.moveMagnitude <= 0.1)
            {
                bulletSpreadMagnitude *= standingBulletSpread / 10f;
            }
            else
            {
                bulletSpreadMagnitude *= Game.player.moveMagnitude * walkingBulletSpread /10f;
            }
            bulletSpreadMagnitude *= BulletSpread;

            RaycastHit hit;
            float tracerLength = 100f;
            Vector3 origin = Shoot_Node.transform.position;
            Vector3 deviation3D = Random.insideUnitCircle * bulletSpreadMagnitude;
            Quaternion rot = Quaternion.LookRotation(Vector3.forward * 100 + deviation3D);
            Vector3 shootDir = Shoot_Node.transform.rotation * rot * Vector3.forward;
            shootDir.y = 0;
            
            
            //LOG("SHOOT DIR: " + shootDir);
            if (Physics.Raycast(origin, shootDir, out hit, 100f, ~(1 << 9)))
            {
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();

                if (enemy)
                {
                    tracerLength = hit.distance;
                    Debug.DrawRay(origin, shootDir * hit.distance, Color.blue, coolDownDuration*3);
                    enemy.GetComponentInParent<Enemy>().takeDamage(damage, Game.player.gameObject);
                }
            }
            else
            {
                Debug.DrawRay(origin, shootDir * 100f, Color.blue, coolDownDuration*3);
            }

            bulletTrail(origin, shootDir, tracerLength);
            coolingDown = true;
            StartCoroutine(coolDown(coolDownDuration));
        }  
    }
}
