using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoScroller : MonoBehaviour
{
    public float beatTempo;
    public bool hasStarted;

    
    void Start()
    {
        beatTempo = beatTempo / 60f; // units at per seconds
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            if (Input.anyKeyDown)
            {
                hasStarted = true;
            }
        }
        else
        {
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        }
            
            
        
        
    }
}
