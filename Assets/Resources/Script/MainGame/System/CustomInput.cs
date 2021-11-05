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


    //public static bool isBlock = false;


    public static bool GetPoint()
    {
        // 게임 중단 반영
        if (GameMaster.isBlock)
            return false;

        bool result = false;

        if (pad != null)        { result = result || false; }
        if (screen != null)     { result = result || screen.press.isPressed; }
        if (keyboard != null)   { result = result || false; }
        if (mouse != null)      { result = result || mouse.leftButton.isPressed; }

        return result;
    }
    public static bool GetPointDown()
    {
        // 게임 중단 반영
        if (GameMaster.isBlock)
            return false;

        bool result = false;

        if (pad != null)        { result = result || false; }
        if (screen != null)     { result = result || screen.press.wasPressedThisFrame; }
        if (keyboard != null)   { result = result || false; }
        if (mouse != null)      { result = result || mouse.leftButton.wasPressedThisFrame; }

        return result;
    }
    public static bool GetPointUP()
    {
        // 게임 중단 반영
        if (GameMaster.isBlock)
            return false;

        bool result = false;

        if (pad != null)        { result = result || false; }
        if (screen != null)     { result = result || screen.press.wasReleasedThisFrame; }
        if (keyboard != null)   { result = result || false; }
        if (mouse != null)      { result = result || mouse.leftButton.wasReleasedThisFrame; }

        return result;
    }
    public static Vector2 GetPointPosition()
    {
        //// 게임 중단 반영
        //if (GameMaster.isBlock)
        //    return Vector2.zero;

        if (screen != null && screen.press.isPressed)
            return screen.position.ReadValue();

        if (mouse != null && (mouse.leftButton.isPressed || mouse.leftButton.wasReleasedThisFrame))
            return mouse.position.ReadValue();

        return Vector2.zero;
    }
}
