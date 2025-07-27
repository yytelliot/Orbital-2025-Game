using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(CircleCollider2D))]
public class ShipMagnet : MonoBehaviour
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

    private HashSet<Rigidbody2D> trackedPickups = new();

    void OnTriggerEnter2D(Collider2D other) {
        HandleCollision(other);
    }

    void HandleCollision(Collider2D other)
    { 
        if (((1 << other.gameObject.layer) & pickupLayer.value) != 0) {
            var rb = other.attachedRigidbody;
            if (rb != null) trackedPickups.Add(rb);
        }
    }

    void FixedUpdate() {
        foreach (var rb in trackedPickups.ToList()) { 
            if (rb == null) { trackedPickups.Remove(rb); continue; }
            Vector2 dir = ((Vector2)magnetCollider.bounds.center - rb.position).normalized;
            rb.AddForce(dir * magnetForce * Time.fixedDeltaTime, ForceMode2D.Impulse);

            // Optionally: Remove if > 2x radius away (failsafe)
            float dist = Vector2.Distance(magnetCollider.bounds.center, rb.position);
            if (dist > magnetCollider.radius * magnetCollider.transform.lossyScale.x * 3f)
            {
                trackedPickups.Remove(rb);
            }
               
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