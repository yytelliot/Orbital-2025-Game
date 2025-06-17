using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoScroller : MonoBehaviour
{
    public float beatTempo;
    public bool ToggleScrolling;
     public static AmmoScroller Instance; // Singleton pattern


    void Awake()
    {
        Instance = this;
        beatTempo /= 60f; // units at per seconds
    }

    // Update is called once per frame
    void Update()
    {
        if (!ToggleScrolling)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ToggleScrolling = true;
            }
        }
        else
        {
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
            
        }
    }

    public void SetScrolling(bool shouldScroll)
    {
        ToggleScrolling = shouldScroll;
        Debug.Log(shouldScroll ? "Scrolling RESUMED" : "Scrolling PAUSED");
    }
}
