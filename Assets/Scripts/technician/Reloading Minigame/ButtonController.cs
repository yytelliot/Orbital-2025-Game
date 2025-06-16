using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

    private SpriteRenderer theSR;
    //public Sprite defaultImage;
    [SerializeField] private Color defaultColour = new Color(50, 50, 50);
    [SerializeField] private Color pressedColour = new Color(150, 50, 50);

    public KeyCode keyToPress;

    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
        theSR.color = defaultColour;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            theSR.color = pressedColour;
        }

        if (Input.GetKeyUp(keyToPress))
        {
            theSR.color = defaultColour;
        }
        
    }
}
