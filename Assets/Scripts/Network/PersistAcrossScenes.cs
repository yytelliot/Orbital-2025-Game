/*
Attach this script to Network Components that
need persist through scene changes. This component ensures
that unique DontDestroyOnLoad components are not created more than once
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistAcrossScenes : MonoBehaviour
{

    // _instance stores the single active instance for this component
    private static PersistAcrossScenes _instance;

    // on awake: if instace does not exist, set it to DontDestroyOnLoad
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
