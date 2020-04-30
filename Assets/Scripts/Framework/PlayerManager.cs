using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadButton = UnityEngine.InputSystem.LowLevel.GamepadButton;


public class PlayerManager : Pawn
{

    //public bool itemPickedUp = false;

    public Weapons weaponOwned;

    public GameObject Hand_Node;

    public float moveSpeed = 10f;
    public float currentSpeed;
    public float rollForce = 10f;

    bool canRoll = true;
    Vector2 leftStick;

	Animator anim;

	protected override void Awake()
	{
		base.Awake();
		anim = GetComponent<Animator>();
	}

	public override void Start()
    {
        base.Start();
        weaponOwned = null;
    }

    // Update is called once per frame
    void Update()
    {
        leftStick = InputManager.getLeftJoyStick();
        spinAround();
        roll();
        attack();

        //LOG("The player is holding: " + weapon?.name);
    }

    private void FixedUpdate()
    {
        moveAround();
    }

    void moveAround()
    {
        currentSpeed = leftStick.magnitude;
        _rb.AddForce(new Vector3((leftStick.x + leftStick.y) / 2, 0, (leftStick.y - leftStick.x) / 2) * moveSpeed);
		if (anim) anim.SetFloat("Movement", transform.InverseTransformDirection(_rb.velocity).z);
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

    void attack()
    {
		if (weaponOwned && !weaponOwned.takes_Continuous_Input)
		{
			if (InputManager.GetButtonPressed(GamepadButton.RightTrigger, true))
			{
				weaponOwned.Use();
				if (anim) anim.SetBool("Attack", true);
			}
			else
			{
				if (anim) anim.SetBool("Attack", true);
			}
            
        }
        else if(weaponOwned && weaponOwned.takes_Continuous_Input)
        {
           // LOG("CONTINUOUS");
            if (InputManager.rightTriggerConstant())
			{
                weaponOwned.Use();
				if (anim) anim.SetBool("Attack", true);
			}
			else
			{
				if (anim) anim.SetBool("Attack", false);
			}
        }
    }


    IEnumerator rollCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        canRoll = true;
    }

    public virtual void PickUp(Weapons weapon)
    {
        if (InputManager.GetButtonPressed(GamepadButton.West))
        {
           // LOG("TRYING TO PICK UP." + weapon.gameObject.transform.name);
           // LOG("WEAPONOWNED == NULL: "+ weaponOwned);
            if (!weaponOwned)
            {
                weaponOwned = weapon;
                weapon.trigger.enabled = false;

                weapon.transform.localScale = weapon.OwnedScale;
                weapon.transform.SetPositionAndRotation(Hand_Node.transform.position, Hand_Node.transform.rotation);
                weapon.transform.parent = Hand_Node.transform;
                


               // LOG("Parent of " + weapon.transform.name + " is " + weapon.transform.parent);
            }
            else
            {
                //LOG("Parent of " + weapon.transform.name + " is " + weapon.transform.parent);
                LOG("DROP YOUR CURRENT ITEM BEFORE PICKING UP ANOTHER");
            }
        }
    }

    public virtual void Drop(Weapons weapon)
    {
        if (InputManager.GetButtonPressed(GamepadButton.East))
        {
            //LOG("TRYING TO DROP" + weapon.gameObject.transform.name);

            if (weaponOwned)
            {

                //LOG("SETTING PARENT NULL");
                weapon.transform.parent = null;
               // LOG("Parent of " + weapon.transform.name + " is " + weapon.transform.parent);


                weapon.transform.position = Game.player.transform.position;
                weapon.transform.position = new Vector3(_transf.position.x, weapon.defaultHeight, weapon.transform.position.z);
                weapon.transform.rotation = Quaternion.Euler(weapon.PlacedRotation);
                weapon.transform.localScale = weapon.PlacedScale;

                Game.player.weaponOwned = null;
                //Game.player.itemPickedUp = false;
                weapon.trigger.enabled = true;

            }
            else
            {
               // LOG("Parent of " + gameObject.transform.name + " is " + gameObject.transform.parent);
                LOG("NO ITEM TO DROP" + gameObject.transform.name);
            }
        }
    }


}
