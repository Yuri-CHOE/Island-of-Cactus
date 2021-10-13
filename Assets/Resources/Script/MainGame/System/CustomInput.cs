using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomInput : MonoBehaviour
{
    public static Gamepad pad = Gamepad.current;
    public static Touchscreen screen = Touchscreen.current;
    public static Keyboard keyboard = Keyboard.current;
    public static Mouse mouse = Mouse.current;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {

    }


    public static bool GetPoint()
    {
        bool result = false;

        if (pad != null)        { result = result || false; }
        if (screen != null)     { result = result || screen.press.isPressed; }
        if (keyboard != null)   { result = result || false; }
        if (mouse != null)      { result = result || mouse.leftButton.isPressed; }

        return result;
    }
    public static bool GetPointDown()
    {
        bool result = false;

        if (pad != null)        { result = result || false; }
        if (screen != null)     { result = result || screen.press.wasPressedThisFrame; }
        if (keyboard != null)   { result = result || false; }
        if (mouse != null)      { result = result || mouse.leftButton.wasPressedThisFrame; }

        return result;
    }
    public static bool GetPointUP()
    {
        bool result = false;

        if (pad != null)        { result = result || false; }
        if (screen != null)     { result = result || screen.press.wasReleasedThisFrame; }
        if (keyboard != null)   { result = result || false; }
        if (mouse != null)      { result = result || mouse.leftButton.wasReleasedThisFrame; }

        return result;
    }
    public static Vector2 GetPointPosition()
    {
        if (screen != null && screen.press.isPressed)
            return screen.position.ReadValue();

        if (mouse != null && mouse.press.isPressed)
            return mouse.position.ReadValue();

        return Vector2.zero;
    }
}
