using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class AlienCollisionHandler : MonoBehaviour
{


    [Tooltip("Knockback applied to player")]
    public float playerKbForce = 5f;

    [Tooltip("Knockback applied to Enemy")]
    public float selfKbForce = 2f;

    [Tooltip("Player Hitstun Time")]
    public float playerHitstun = 0.5f;

    [Tooltip("Damage Dealt on Collision")]
    public int damage = 10;
    [Tooltip("Freeze Rotation")]
    public bool freezeRotation = false;

    [Header("Events")]
    public GameEvent onPilotHitStun;

    private Rigidbody2D rb;
    private GameObject player;
    private ShipProperties shipProperties;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (freezeRotation)
        {
            rb.freezeRotation = true;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        shipProperties = player.GetComponent<ShipProperties>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
    }

    void HandleCollision(Collision2D collision)
    { 
        var intangible = GetComponent<IIntangible>();
        if (!collision.collider.CompareTag("Player")) return;
        if (intangible != null && intangible.isIntangible) return;

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
        shipProperties.TakeDamage(damage);

        onPilotHitStun.RaiseNetworked(this, playerHitstun);

    }
}
