using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBootstrapper : MonoBehaviour
{
    public AudioLibrary library;

    void Awake()
    {
        library.Init();
    }
}

