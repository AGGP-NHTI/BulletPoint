using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GamepadButton = UnityEngine.InputSystem.LowLevel.GamepadButton;

public class InputManager : MonoBehaviour
{
	class Buffer
	{
		public float timePressed;
		public float timeReleased;

		public Buffer()
		{
			timePressed = -1;
			timeReleased = -1;
		}
	}


	private static InputManager instance;

	Dictionary<GamepadButton, Buffer> buffer = new Dictionary<GamepadButton, Buffer>()
	{
		{GamepadButton.North,	new Buffer() },
		{GamepadButton.East,	new Buffer() },
		{GamepadButton.South,	new Buffer() },
		{GamepadButton.West,	new Buffer() },
	};
	Gamepad gp;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
	
    void Start()
    {
        gp = Gamepad.current;
	}
	
    void Update()
    {
		foreach (KeyValuePair<GamepadButton,Buffer> entry in buffer)
		{
			if (gp[entry.Key].wasPressedThisFrame)
			{
				entry.Value.timePressed = Time.time;
			}
			else if (gp[entry.Key].wasReleasedThisFrame)
			{
				entry.Value.timeReleased = Time.time;
			}
		}
	}

	public static bool GetButtonPressed(GamepadButton btn)
	{
		return instance.gp[btn].wasPressedThisFrame;
	}

	public static bool GetButtonPressed(GamepadButton btn, float bufferTime)
	{
		if (instance.buffer.ContainsKey(btn))
		{
			if (instance.buffer[btn].timePressed != -1)
			{
				if ((Time.time - instance.buffer[btn].timePressed) < bufferTime)
				{
					instance.buffer[btn].timePressed = -1;
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		else
		{
			throw new System.Exception("InputManager does not contain a buffer for button " + System.Enum.GetName(typeof(GamepadButton), btn));
		}
		
	}

	public static bool GetButtonReleased(GamepadButton btn)
	{
		return instance.gp[btn].wasReleasedThisFrame;
	}

	public static bool GetButtonHeld(GamepadButton btn)
	{
		return instance.gp[btn].isPressed;
	}


	//left and right joysticks
	public static Vector2 getLeftJoyStick()
    {
        return instance.gp.leftStick.ReadValue();
    }
    public static Vector2 getRightJoyStick()
    {
        return instance.gp.rightStick.ReadValue();
    }


    //xbox triggers and PS4 L1 and R1 Buttons
    public static bool leftTrigger()
    {
        return instance.gp.leftTrigger.wasPressedThisFrame;
    }
    public static bool rightTrigger()
    {
        return instance.gp.rightTrigger.wasPressedThisFrame;
    }

    //xbox bumpers and PS4 L2 and R2 Buttons
    public static bool leftShoulder()
    {
        return instance.gp.leftShoulder.wasPressedThisFrame;
    }
    public static bool rightShoulder()
    {
        return instance.gp.rightShoulder.wasPressedThisFrame;
    }


    //Dpad (0, 1) = Up
    //Dpad (0,-1) = Down
    //Dpad (-1,0) = Left
    //Dpad (1, 0) = Right
    public static Vector2 D_Pad()
    {
        return instance.gp.dpad.ReadValue();
    }

    //Dpad directions pressed
    public static bool D_Pad_Up()
    {
        return D_Pad().y > 0;
    }
    public static bool D_Pad_Down()
    {
        return D_Pad().y < 0;
    }
    public static bool D_Pad_Left()
    {
        return D_Pad().x < 0;
    }
    public static bool D_Pad_Right()
    {
        return D_Pad().x > 0;
    }

  //  //Xbox Y and PS4 Triangle
  //  public static bool button_North()
  //  {
  //      return instance.gp.buttonNorth.wasPressedThisFrame;
  //  }

  //  //Xbox A and PS4 Cross
  //  public static bool button_South()
  //  {
  //      return instance.gp.buttonSouth.wasPressedThisFrame;
  //  }

  //  //Xbox B and PS4 Circle    
  //  public static bool button_East()
  //  {
  //      return instance.gp.buttonEast.wasPressedThisFrame;
  //  }

  //  //Xbox X and PS4 Square
  //  public static bool button_West()
  //  {
		//return instance.gp.buttonWest.wasPressedThisFrame;

		////return instance.bufferInputs["buttonWest"].UpdateState(instance.gp.buttonWest.wasPressedThisFrame);
  //  }

  //  //start button
  //  public static bool button_Start()
  //  {
  //      return instance.gp.startButton.wasPressedThisFrame;
  //  }

  //  //select button
  //  public static bool button_Select()
  //  {
  //      return instance.gp.selectButton.wasPressedThisFrame;
  //  }



	//abstract class BaseButton
	//{
	//	public readonly GamepadButton button;

	//	public abstract bool wantsUpdate { get; }

	//	protected bool _pressed = false;
	//	protected bool _released = false;
	//	protected bool _held = false;

	//	public virtual bool Pressed() { return _pressed; }
	//	public virtual bool Released() { return _released; }
	//	public virtual bool Held() { return _held; }

	//	public BaseButton(GamepadButton b)
	//	{
	//		button = b;
	//	}

	//	public abstract void UpdateState();
	//}

	//class BasicButton : BaseButton
	//{
	//	public override bool wantsUpdate => false;

	//	public BasicButton(GamepadButton b) : base(b)
	//	{
			
	//	}

	//	public override void UpdateState()
	//	{
	//		_pressed = instance.gp[button].wasPressedThisFrame;
	//		_released = instance.gp[button].wasReleasedThisFrame;
	//		_held = instance.gp[button].isPressed;
	//	}
	//}

	//class BufferButton : BaseButton
	//{
	//	public override bool wantsUpdate => true;

	//	public readonly float timeout;
	//	public float timer = 0;

	//	private bool _consumeBuffer = false;

	//	public BufferButton(GamepadButton b, float timeout) : base(b)
	//	{
	//		this.timeout = timeout;
	//	}

	//	public override void UpdateState()
	//	{
	//		if (_consumeBuffer)
	//		{
	//			_pressed = false;
	//			_held = false;

	//			_consumeBuffer = false;
	//		}
	//		else
	//		{
	//			_pressed = instance.gp[button].wasPressedThisFrame;

	//		}
	//	}

	//	public override bool Pressed()
	//	{
	//		if (_pressed)
	//		{
	//			_consumeBuffer = true;
	//		}
	//		return _pressed;
	//	}
	//}

}
