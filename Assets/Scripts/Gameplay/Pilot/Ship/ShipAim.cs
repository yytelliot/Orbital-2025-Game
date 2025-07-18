using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAim : MonoBehaviour
{
    public Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Build vector to mouse position
        Vector2 direction = (mousePos - transform.position);
        
        // Calculate the angle in degrees
        float angleDeg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float rotationOffset = -90f;  // sprite faces up, so must be rotated 90 deg
        
        float targetAngle = angleDeg + rotationOffset;

        // apply the transformation
        transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
    }
}
