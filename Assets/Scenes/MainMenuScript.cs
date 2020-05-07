using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GamepadButton = UnityEngine.InputSystem.LowLevel.GamepadButton;


public class MainMenuScript : MonoBehaviour
{
    
    Vector2 leftJoystick;
    bool isSouthPressed = false;


    [Header("Objects")]
    public RectTransform cursor;

    [Header("UI Control")]
    public float cursorSpeed = 5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        leftJoystick = InputManager.getLeftJoyStick();
        isSouthPressed = InputManager.GetButtonPressed(GamepadButton.South);
        cursorUsed();
    }

    private void FixedUpdate()
    {
        cursor.position += (Vector3)leftJoystick * cursorSpeed;
    }

    public void Start_Button()
    {
        Game.LoadNextScene();
    }

    public void Quit_Button()
    {
        Application.Quit();
    }

    void cursorUsed()
    {
        List<RaycastResult> list = new List<RaycastResult>();
        PointerEventData eventData = new PointerEventData(EventSystem.current);

        eventData.position = cursor.position;

        if (isSouthPressed)
        {
            Debug.Log("INSIDE");
            EventSystem.current.RaycastAll(eventData, list);

            foreach(RaycastResult cast in list)
            {
                Debug.Log("Object: " + cast.gameObject.name);

                Button button = cast.gameObject.GetComponent<Button>();


                if (button)
                {
                   button.onClick.Invoke();
                }
            }

        }
    }
}
