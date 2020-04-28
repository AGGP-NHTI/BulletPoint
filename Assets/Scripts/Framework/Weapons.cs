using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadButton = UnityEngine.InputSystem.LowLevel.GamepadButton;

public class Weapons : EntityController
{

    PlayerManager owner;

    GameObject parent;

    

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setParent(PlayerManager whosCalling)
    {
        _transf.SetPositionAndRotation(parent.transform.position, parent.transform.rotation);
        _transf.SetParent(parent.transform);
        owner = whosCalling;
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();

        if (player)
        {
            if (InputManager.GetButtonPressed(GamepadButton.West))
            {
                parent = player.Hand_Node;
                setParent(player);

                player.itemPickedUp = true;
            }
        }
    }
}
