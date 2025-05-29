using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SimpleBulletBehavior : MonoBehaviour
{
    [Header("Object Properties")]
    public float offset = 90f;

    [Header("Bullet Properties")]
    public float speed = 200f;
    public float lifetime = 2f;

    Rigidbody2D rb;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Initialize initializes the bullet with the given parameters (direction, angle, speed multiplier)
    public void Initialize(Vector2 direction, float angle, float speedMult)
    {
        // set initial rotation
        transform.rotation = Quaternion.Euler(0, 0, angle - offset);

        // set initial velocity
        rb.velocity = direction * speed * speedMult;

        // schedule self destruct
        Destroy(gameObject, lifetime);
    }
}
