using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipShooter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetKeyDown("fire"))
            ShootAtMouse();
    }

    void ShootAtMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("Pew!");

        // get the direction vector from the ship's firePoint to the mouse
        // Vector2 dir = (mousePosition - fire)
    }
}
