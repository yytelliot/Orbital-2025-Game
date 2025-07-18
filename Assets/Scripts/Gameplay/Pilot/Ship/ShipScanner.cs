using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

public class ShipScanner : MonoBehaviour
{
    public float defaultRadius;
    public GameEvent onScan;
    public Transform player;

    void Awake()
    {
        player = gameObject.transform;
    }

    public void OnScan(Component sender, object data)
    {
        int strength = (int)data;
        var payload = new ScannerRevealPayload
        {
            scannerPosition = player.position,
            scannerRadius = defaultRadius + 0.5f * defaultRadius * (strength-1),
            scannerStrength = strength
        };
        onScan.Raise(this, payload);
    }


     void OnDrawGizmosSelected()
    {
        // Draw the scan radius in the editor when the object is selected
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); // Orange, semi-transparent
        Gizmos.DrawWireSphere(transform.position, defaultRadius);
    }
}
