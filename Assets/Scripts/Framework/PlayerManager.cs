using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadButton = UnityEngine.InputSystem.LowLevel.GamepadButton;


public class PlayerManager : Pawn
{
    //private variables
    float mass = 3.0f; // defines the character mass
    Vector3 impact = Vector3.zero;
    Vector2 leftStick;
    Vector2 rightStick;
    
    Vector3 lastPos;

    //public variables
    [Header("Animation Control")]
    public Animator playerAnimator;
    public RuntimeAnimatorController gunAnimator;
    public RuntimeAnimatorController twoHandedAnimator;
    public RuntimeAnimatorController oneHandedAnimator;
    public Vector2 animationInputs;
    public GameObject playerModel;

    [Header("Attacking")]
    public Weapons weaponOwned;
    public GameObject Hand_Node;

    [Header("Movement")]
    public CharacterController charController;

    public float moveSpeed = 10f;
    public float moveMagnitude;
    public float lookMagnitude;
    public bool canRoll = true;
    public float rollSpeed = 20f;
    
    [Header("Control")]
    public float playerStartingY;

    public System.Func<bool> isPlayerSetup = () => false;

	public override void Start()
    {
        //base.Start();
        if(!weaponOwned) playerAnimator.runtimeAnimatorController = oneHandedAnimator;
        weaponOwned = null;

        isPlayerSetup = () => true;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        rightStick = InputManager.getRightJoyStick();
        leftStick = InputManager.getLeftJoyStick();
        spinAround();
        attack();
    }
    
    

    private void FixedUpdate()
    {
        goToGround();
        animationInputs = moveDirection();
        //LOG("Animation Inputs: " + animationInputs);
        playerAnimator.SetFloat("ForwardMovement", animationInputs.y);
        playerAnimator.SetFloat("RightMovement", animationInputs.x);
		moveAround();

        //LOG("Weapon Owned: " + weaponOwned?.name);
        //LOG("Takes Continuos Input: " + weaponOwned?.takes_Continuous_Input);
    }


    //Returns a Vector2 for Animation control
    Vector2 moveDirection()
    {
        Vector3 dir = transform.InverseTransformDirection(transform.position - lastPos);

        return new Vector2(dir.x, dir.z).normalized;
    }


    //if no right stick input is detected look in the direction of the player, then move that way
    //if right stick input is detected move based off of x and z axis
    void moveAround()
    {
        playerModel.transform.position = Vector3.zero;

        float cushion = 0.25f;
        if (_transf.position.y > playerStartingY + cushion)
        {
            charController.Move(_transf.TransformDirection(Vector3.down)/10);
        }
        else if (_transf.position.y < playerStartingY -cushion)
        {
            charController.Move(_transf.TransformDirection(Vector3.up)/10);
        }

        //last position for animations
		lastPos = transform.position;

        //look direction for when no right stick input
        if (rightStick.magnitude <= 0)
        {
            float deltaX = leftStick.x;
            float deltaY = leftStick.y;
            float joypos = Mathf.Atan2(deltaX, deltaY) * Mathf.Rad2Deg;

            transform.eulerAngles = new Vector3(0, joypos + 45, 0);
            playerModel.transform.eulerAngles = transform.rotation.eulerAngles;
        }
        
        //move
        charController.Move((new Vector3((leftStick.x + leftStick.y) / 2, 0, (leftStick.y - leftStick.x) / 2) * moveSpeed / 10));
        
        //set moveMagnitude to see how much input is from the left stick.
        moveMagnitude = leftStick.magnitude;
    }

    //look in the direction of the right stick
    void spinAround()
    {
        lookMagnitude = rightStick.magnitude;
        if (lookMagnitude > 0)
        {
            float deltaX = rightStick.x;
            float deltaY = rightStick.y;
            float joypos = Mathf.Atan2(deltaX, deltaY) * Mathf.Rad2Deg;


            transform.eulerAngles = new Vector3(0, joypos + 90, 0);

            playerModel.transform.eulerAngles = transform.rotation.eulerAngles;
        }
    }

    //attacks with the weapon that is owned
    void attack()
    {
		if (weaponOwned && !weaponOwned.takes_Continuous_Input)
		{
			if (InputManager.GetButtonPressed(GamepadButton.RightTrigger, true))
			{
                LOG("TRYING TO USE WEAPON");
                weaponOwned.Use();
				if (playerAnimator) playerAnimator.SetBool("Attack", true);
			}
			else
			{
				if (playerAnimator) playerAnimator.SetBool("Attack", true);
			}
            
        }
        else if(weaponOwned && weaponOwned.takes_Continuous_Input)
        {
            if (InputManager.rightTriggerConstant())
			{
                LOG("TRYING TO USE WEAPON");
                weaponOwned.Use();
				if (playerAnimator) playerAnimator.SetBool("Attack", true);
			}
			else
			{
				if (playerAnimator) playerAnimator.SetBool("Attack", false);
			}
        }
    }



    

    public virtual void PickUp(Weapons weapon)
    {
        if (InputManager.GetButtonPressed(GamepadButton.West) || Input.GetKeyDown(KeyCode.Space))
        {
            if (!weaponOwned)
            {
                setOwnedWeapon(weapon);
            }
            else
            {
                LOG("DROP YOUR CURRENT ITEM BEFORE PICKING UP ANOTHER");
            }
        }
    }

    public virtual void setOwnedWeapon(Weapons weapon)
    {
        weaponOwned = weapon;
        weapon.trigger.enabled = false;

        weapon.transform.localScale = weapon.OwnedScale;
        weapon.transform.SetPositionAndRotation(Hand_Node.transform.position, Hand_Node.transform.rotation);
        weapon.transform.parent = Hand_Node.transform;


        setWeaponAnims();

    }

    void setWeaponAnims()
    {
        if (weaponOwned is AssaultRifle || weaponOwned is Sniper)
        {
            playerAnimator.runtimeAnimatorController = gunAnimator;
        }
        else if (true)//SWORD
        {


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
                playerAnimator.runtimeAnimatorController = oneHandedAnimator;
            }
            else
            {
               // LOG("Parent of " + gameObject.transform.name + " is " + gameObject.transform.parent);
                LOG("NO ITEM TO DROP" + gameObject.transform.name);
            }
        }
    }

    void goToGround()
    {
        if (transform.position.y > playerStartingY)
        {
            _transf.Translate(-transform.up);
        }
    }

    

    IEnumerator rollCoolDown(float input)
    {
        yield return new WaitForSeconds(input);
        canRoll = true;
    }

    protected override void dead(GameObject source = null, int timeUntilRemove = 1)
    {
        playerAnimator.SetInteger("Health", 0);
        base.dead();
    }
}
