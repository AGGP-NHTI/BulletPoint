using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{



    private static InputManager instance;

    Gamepad gp;

    float timer = 0f;

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

    // Start is called before the first frame update
    void Start()
    {
        gp = Gamepad.current;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
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

    //Xbox Y and PS4 Triangle
    public static bool button_North()
    {
        return instance.gp.buttonNorth.wasPressedThisFrame;
    }

    //Xbox A and PS4 Cross
    public static bool button_South()
    {
        return instance.gp.buttonSouth.wasPressedThisFrame;
    }

    //Xbox B and PS4 Circle    
    public static bool button_East()
    {
        return instance.gp.buttonEast.wasPressedThisFrame;
    }

    //Xbox X and PS4 Square
    public static bool button_West()
    {
        return instance.gp.buttonWest.wasPressedThisFrame;
    }

    //start button
    public static bool button_Start()
    {
        return instance.gp.startButton.wasPressedThisFrame;
    }

    //select button
    public static bool button_Select()
    {
        return instance.gp.selectButton.wasPressedThisFrame;
    }



}
