using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadButton = UnityEngine.InputSystem.LowLevel.GamepadButton;


public class PlayerManager : Pawn
{

    public Vector2 animationDirection;

    protected System.Action runOnFrame;
    float mass = 3.0f; // defines the character mass
    Vector3 impact = Vector3.zero;

    //public bool itemPickedUp = false;
    public GameObject Player_Model;
    public Weapons weaponOwned;
    public CharacterController charController;
    public GameObject Hand_Node;

    public float moveSpeed = 10f;
    public float currentSpeed;
    public float rollSpeed = 20f;


    public bool canRoll = true;

    Vector2 leftStick;
    Vector2 rightStick;
    public Animator anim;

	protected override void Awake()
	{
		base.Awake();
		
	}

	public override void Start()
    {    
        base.Start();
        weaponOwned = null;
    }

    // Update is called once per frame
    void Update()
    {
        rightStick = InputManager.getRightJoyStick();
        leftStick = InputManager.getLeftJoyStick();
        
        //if (InputManager.GetButtonPressed(GamepadButton.South())
        //{

        //}
        spinAround();
        attack();
        //LOG("The player is holding: " + weapon?.name);
    }

    private void FixedUpdate()
    {
        animationDirection = moveDirection();
        moveAround();
    }

    Vector2 moveDirection()
    {
        Vector2 dir = Vector2.zero;

        return dir;
    }

    void moveAround()
    {
        if (rightStick.magnitude <= 0)
        {
            float deltaX = leftStick.x;
            float deltaY = leftStick.y;
            float joypos = Mathf.Atan2(deltaX, deltaY) * Mathf.Rad2Deg;

            transform.eulerAngles = new Vector3(0, joypos + 90, 0);
            Player_Model.transform.eulerAngles = transform.rotation.eulerAngles;
        }
        

        charController.Move((new Vector3((leftStick.x + leftStick.y) / 2, 0, (leftStick.y - leftStick.x) / 2) * moveSpeed / 10));
        
        if (anim) anim.SetFloat("Movement", currentSpeed);



        currentSpeed = leftStick.magnitude;
    }

    void spinAround()
    {
        if (rightStick.magnitude > 0)
        {
            LOG("RIGHT STICK " + rightStick);
            float deltaX = rightStick.x;
            float deltaY = rightStick.y;
            float joypos = Mathf.Atan2(deltaX, deltaY) * Mathf.Rad2Deg;


            transform.eulerAngles = new Vector3(0, joypos + 90, 0);


            Player_Model.transform.eulerAngles = transform.rotation.eulerAngles;
        }
    }

    void AddImpact(Vector3 dir,float force)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
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

    void directForce(float time, Vector3 dir, float amount)
    {
        
    }

    IEnumerator rollCoolDown(float input)
    {
        yield return new WaitForSeconds(input);
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


    //Vector2 getMoveDirection()
    //{
        



    //}

    

}
