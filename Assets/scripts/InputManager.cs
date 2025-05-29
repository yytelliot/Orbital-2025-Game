using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Player actions
    public enum Action { Fire, Dash }

    // Controls 
    static Dictionary<Action, KeyCode> bindings = new Dictionary<Action, Keycode>()
    {
        { Action.Fire, KeyCode.Mouse0 }
    };

    public static bool GetKeyDown(Action a) 
    { 

    }
}
