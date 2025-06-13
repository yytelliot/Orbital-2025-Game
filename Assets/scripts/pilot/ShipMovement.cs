using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 movement;
    public float thrust;
    // public float acceleration = 20f;
    public float maxSpeed = 20f;
    // public float deceleration = 15f;

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
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        movement = new Vector2(h, v).normalized;



    }

    void FixedUpdate()
    {
        // // Target velocity to reach
        // Vector2 targetVelocity = movement * maxSpeed;

        // // accelerate if we have input, otherwise brake
        // float rate = (movement.sqrMagnitude > 0f) ? acceleration : deceleration;

        // // move current velocity toward our target by up to rate*dt
        // rb.velocity = Vector2.MoveTowards(
        //     rb.velocity,
        //     targetVelocity,
        //     rate * Time.fixedDeltaTime
        // );

        // Apply a continuous force in the input direction
        rb.AddForce(movement * thrust);

        // Enforce a hard speed cap
        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

    }
}
