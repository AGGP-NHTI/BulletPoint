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

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void Use()
    {
        if (!coolingDown)
        {
            RaycastHit hit;
            Debug.DrawRay(_transf.position, _transf.forward * 100f, Color.blue, 5f);
            if (Physics.Raycast(_transf.position, _transf.forward, out hit, 100f))
            {
                Pawn enemy = hit.transform.GetComponent<Pawn>();
                if (enemy)
                {
                    enemy.takeDamage(damage, Game.player);
                }
            }



            coolingDown = true;
            StartCoroutine(coolDown(coolDownDuration));
        }
    }
}
