using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadButton = UnityEngine.InputSystem.LowLevel.GamepadButton;

public abstract class Weapons : EntityController
{
    public int damage = 10;
    public float coolDownDuration = 0.5f;
    protected PlayerManager owner;
    GameObject parent;

    public bool takes_Continuous_Input;
    public Collider trigger;

    protected bool coolingDown = false;


    protected virtual void setParent(PlayerManager whosCalling)
    {
        _transf.SetPositionAndRotation(parent.transform.position, parent.transform.rotation);
        _transf.SetParent(parent.transform);
        owner = whosCalling;
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();

        if (player)
        {
            if (InputManager.GetButtonPressed(GamepadButton.West))
            {
                parent = player.Hand_Node;
                setParent(player);

                player.itemPickedUp = true;
                player.weapon = this;
                trigger.enabled = false;
            }
        }
    }

    public abstract void Use();



    protected IEnumerator coolDown(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);

        coolingDown = false;
    }
}
