using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

public class EnergyPickup : MonoBehaviour
{
    [Tooltip("How many units this pickup grants")]
    public int amount = 1;

    [Tooltip("The game event to raise when collected")]
    // public GameEvent onPickupCollected;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        var shipEnergyHandler = collider.GetComponent<ShipEnergyHandler>();
        if (shipEnergyHandler != null)
             shipEnergyHandler.AddEnergy(amount);
             
        Destroy(gameObject);
    }
}
