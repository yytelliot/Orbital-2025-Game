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

    // Takes: Action name (string)
    // Returns: if the input key for that action is pressed
    public static bool GetKeyDown(string action)
    {
        return Input.GetKeyDown(bindings[action]);
    }
}
