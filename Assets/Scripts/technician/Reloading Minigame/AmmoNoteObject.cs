using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoNoteObject : MonoBehaviour
{
    private bool canBePressed;
    public KeyCode keyToPress;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            if (canBePressed)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            
            canBePressed = true;
            
        }
        else if (other.tag == "Stopper")
        {
            AmmoScroller.Instance?.SetScrolling(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            
            canBePressed = false;
            
        }

        else if (other.tag == "Stopper")
        {
            AmmoScroller.Instance?.SetScrolling(true);
        }
    }
}
