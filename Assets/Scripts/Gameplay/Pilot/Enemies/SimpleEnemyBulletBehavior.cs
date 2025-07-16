using System.Collections;
using System.Collections.Generic;
using Game.Events;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SimpleEnemyBulletBehavior : MonoBehaviour
{
    [Header("Object Properties")]
    public float offset = 90f;

    [Header("Bullet Properties")]
    public float speed = 100f;
    public float lifetime = 3f;
    public int damage = 10;

    [Header("GameEvents")]
    public GameEvent onProjectileHit;

    Rigidbody2D rb;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Initialize initializes the bullet with the given parameters (direction, angle, speed multiplier)
    public void Initialize(Vector2 direction, float angle, float speedMult = 1f)
    {
        // set initial rotation
        transform.rotation = Quaternion.Euler(0, 0, angle - offset);

        // set initial velocity
        rb.velocity = direction.normalized * speed * speedMult;

        // schedule self destruct
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player hit!");
            onProjectileHit.Raise(this, new ProjectileHitPayload { target = collision.gameObject, damage = damage });
            Destroy(gameObject);
        }
    
    }
}
