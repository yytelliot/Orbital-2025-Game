using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class AlienCollision : MonoBehaviour
{

    [Tooltip("Knockback applied to player")]
    public float playerKbForce = 5f;

    [Tooltip("Knockback applied to Enemy")]
    public float selfKbForce = 2f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.collider.attachedRigidbody;

            //Get force vectors
            Vector2 enemyToShip = (playerRb.position - rb.position).normalized;
            Vector2 shipToEnemy = (rb.position - playerRb.position).normalized;

            // Apply force
            playerRb.AddForce(enemyToShip * playerKbForce, ForceMode2D.Impulse);
            rb.AddForce(shipToEnemy * selfKbForce, ForceMode2D.Impulse);

            var stunnable = GetComponent<IStunnable>();
            if (stunnable != null)
            {
                stunnable.StunUntilStop();
            }
        }
    }
}
