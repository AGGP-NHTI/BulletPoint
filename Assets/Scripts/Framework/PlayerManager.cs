using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadButton = UnityEngine.InputSystem.LowLevel.GamepadButton;


public class PlayerManager : Pawn
{
    public bool itemPickedUp = false;

    public GameObject Hand_Node;

    public float moveSpeed = 10f;
    public float rollForce = 10f;

    bool canRoll = true;

    // Update is called once per frame
    void Update()
    {
        moveAround();
        spinAround();
        attack();
        roll();
    }

    void moveAround()
    {
        Vector2 leftStick = InputManager.getLeftJoyStick();
        _rb.AddForce(new Vector3((leftStick.x + leftStick.y) / 2, 0, (leftStick.y - leftStick.x) / 2) * moveSpeed);
    }

    void spinAround()
    {
        Vector2 rightStick = InputManager.getRightJoyStick();

        Vector3 lookDir = new Vector3((rightStick.x + rightStick.y) / 2, 0 , (rightStick.y - rightStick.x) / 2) + _transf.position;
        //lookDir = lookDir.normalized;

        _transf.LookAt(lookDir);
    }

    void roll()
    {

        Vector2 leftStickDir = InputManager.getLeftJoyStick();

        Vector3 rollDir = new Vector3((leftStickDir.x + leftStickDir.y) / 2, 0, (leftStickDir.y - leftStickDir.x) / 2);

        Vector3 lookDir = rollDir + _transf.position;


            
        if (canRoll && InputManager.GetButtonPressed(GamepadButton.South, true))
        {
            if (leftStickDir.sqrMagnitude == 0)
            {
                _rb.AddForce(_transf.forward * rollForce, ForceMode.Impulse);
            }
            else
            {
                _rb.AddForce(rollDir * rollForce,ForceMode.Impulse);
                _transf.LookAt(lookDir);
            }
            canRoll = false;
            StartCoroutine(rollCoolDown());
        }
    }

    IEnumerator rollCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        canRoll = true;
    }
}
