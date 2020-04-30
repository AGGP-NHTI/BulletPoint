using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Weapons
{

    private float bulletSpreadMagnitude = 0;
    public float standingBulletSpread = 0;
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
            if (Game.player.currentSpeed <= 0.1)
            {
                bulletSpreadMagnitude *= standingBulletSpread / 10f;
            }
            else
            {
                bulletSpreadMagnitude *= Game.player.currentSpeed;
            }
            bulletSpreadMagnitude *= BulletSpread;

            RaycastHit hit;

            Vector3 deviation3D = Random.insideUnitCircle * bulletSpreadMagnitude;
            Quaternion rot = Quaternion.LookRotation(Vector3.forward * 100 + deviation3D);
            Vector3 shootDir = _transf.rotation * rot * Vector3.forward;

            bulletTrail(_transf.position,shootDir, 100f);
            
            //LOG("SHOOT DIR: " + shootDir);
            if (Physics.Raycast(_transf.position, shootDir, out hit, 100f, ~(1 << 9)))
            {
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();

                if (enemy)
                {
                    
                    Debug.DrawRay(_transf.position, shootDir * hit.distance, Color.blue, coolDownDuration*3);
                    enemy.GetComponentInParent<Enemy>().takeDamage(damage, Game.player.gameObject);
                }
            }
            else
            {
                Debug.DrawRay(_transf.position, shootDir * 100f, Color.blue, coolDownDuration*3);
            }


            coolingDown = true;
            StartCoroutine(coolDown(coolDownDuration));
        }  
    }
}
