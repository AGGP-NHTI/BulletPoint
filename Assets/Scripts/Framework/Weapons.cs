using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadButton = UnityEngine.InputSystem.LowLevel.GamepadButton;

public abstract class Weapons : EntityController
{
    Vector3 defaultRotation;

    private float defaultHeight = 2.5f;

    public int damage = 10;
    public float coolDownDuration = 0.5f;

    GameObject parent;

    public bool takes_Continuous_Input;
    public Collider trigger;

    protected bool coolingDown = false;

    private void Start()
    {
        
        defaultRotation = _transf.rotation.eulerAngles;
    }

    protected virtual void PickUp()
    {
        if (!Game.player.itemPickedUp)
        {
            parent = Game.player.Hand_Node;


            Game.player.itemPickedUp = true;
            Game.player.weapon = this;
            trigger.enabled = true;

            _transf.SetPositionAndRotation(parent.transform.position, parent.transform.rotation);
            _transf.SetParent(parent.transform);
        }
        else
        {
            LOG("DROP YOUR CURRENT ITEM BEFORE PICKING UP ANOTHER");
        }
    }

    protected virtual void Drop()
    {
        if (Game.player.itemPickedUp)
        {
            parent = null;

            Game.player.weapon = null;
            Game.player.itemPickedUp = false;
            trigger.enabled = true;

            LOG("SETTING PARENT NULL");
            _transf.SetParent(null);
            _transf.position = Game.player.transform.position;
            _transf.position = new Vector3(_transf.position.x, defaultHeight, _transf.position.z);
        }
        else
        {
            LOG("NO ITEM TO DROP");
        }
    }

    private void Update()
    {
       if (InputManager.GetButtonPressed(GamepadButton.East))
        {
            LOG("TRYING TO DROP");
            Drop();
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();

        if (player)
        {
            if (InputManager.GetButtonPressed(GamepadButton.West))
            {
                LOG("TRYING TO PICK UP.");
                PickUp();
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
