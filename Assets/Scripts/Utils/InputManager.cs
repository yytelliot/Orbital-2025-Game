using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    // Dictionary of key bindings 
    static Dictionary<string, KeyCode> bindings = new Dictionary<string, KeyCode>()
    {
        { "fire", KeyCode.Mouse0 }
    };

    public static bool GetKeyDown(string action)
    {
        return Input.GetKeyDown(bindings[action]);
    }

    public static bool GetKeyUp(string action)
    {
        return Input.GetKeyUp(bindings[action]);
    }

    public static bool GetKey(string action)
    {
        return Input.GetKey(bindings[action]);
    }
}
