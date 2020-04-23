using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Pawn
{

    float spinFactor = 120;

    public float moveSpeed = 10;
    public float spinSpeed = 10;


    // Update is called once per frame
    void Update()
    {
        moveAround();
        spinAround();
        attack();
    }

    void moveAround()
    {
        Vector2 leftStick = InputManager.getLeftJoyStick();
        LOG("Left Stick: "+leftStick);
        _rb.velocity = new Vector3((leftStick.x + leftStick.y) / 2, 0, (leftStick.y - leftStick.x) / 2) * moveSpeed;
    }

    void spinAround()
    {
        Vector2 rightStick = InputManager.getRightJoyStick();

        Vector3 lookDir = new Vector3(rightStick.x, 0, rightStick.y) + _transf.position;
        //lookDir = lookDir.normalized;

        _transf.LookAt(lookDir);
    }


    void attack()
    {
        bool rightTrigger = InputManager.rightTrigger();

        if (rightTrigger)
        {
            RaycastHit hit;
            Debug.DrawRay(_transf.position, _transf.forward * 100f, Color.blue, 5f);
            if (Physics.Raycast(_transf.position, _transf.forward, out hit, 100f))
            {
                Pawn enemy = hit.transform.GetComponent<Pawn>();
                if (enemy)
                {
                    enemy.takeDamage(2, _obj);
                }
            }
            
        }
    }
}
