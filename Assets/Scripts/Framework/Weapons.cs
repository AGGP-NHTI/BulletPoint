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

    void use()
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
