using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    // Dictionary of key bindings 
    static Dictionary<string, KeyCode> bindings = new Dictionary<string, KeyCode>()
    {
        { "fire", KeyCode.Mouse0 },

        //Technician interact key
        { "interact", KeyCode.Z },   

        //Technician Ammo Reload Minigame
        { "Ammo1", KeyCode.A },
        { "Ammo2", KeyCode.S },
        { "Ammo3", KeyCode.K },
        { "Ammo4", KeyCode.L },
        { "StartGame", KeyCode.Space },


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
