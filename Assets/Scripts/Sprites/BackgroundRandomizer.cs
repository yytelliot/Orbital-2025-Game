using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRandomizer : MonoBehaviour
{
    // Start is called before the first frame update
    public static int galaxy = -1;
    void Awake()
    {
        galaxy = Random.Range(0, 3);
    }

}
