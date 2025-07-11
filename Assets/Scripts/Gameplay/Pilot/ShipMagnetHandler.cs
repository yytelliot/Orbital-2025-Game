using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PickupMagnet : MonoBehaviour
{
    [Header("Magnet Settings")]
    public CircleCollider2D magnetCollider;   // drag your child here
    public float magnetForce = 20f;
    public LayerMask pickupLayer;

    void Awake()
    {
        // ensure it’s a trigger
        magnetCollider.isTrigger = true;
    }

    void FixedUpdate()
    {
        // use the collider’s radius & position (in case it’s offset)
        Vector2 center = magnetCollider.bounds.center;
        float   radius = magnetCollider.radius * magnetCollider.transform.lossyScale.x;

        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius, pickupLayer);
        foreach (var hit in hits)
        {
            Rigidbody2D rb = hit.attachedRigidbody;
            if (rb == null) continue;
            Vector2 dir = (center - rb.position).normalized;
            rb.AddForce(dir * magnetForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (magnetCollider == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(magnetCollider.bounds.center,
                              magnetCollider.radius * magnetCollider.transform.lossyScale.x);
    }
}