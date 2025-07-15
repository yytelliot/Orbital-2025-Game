using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

public class AlienHpHandler : MonoBehaviour, ITakeDamage
{
    [Header("Stats")]
    public int maxHp;
    public int currentHp;

    [Header("Drops")]
    [Tooltip("Prefab of the pickup to spawn upon death. Leave empty if no drops")]
    public GameObject pickupPrefab;
    [Range(0f, 1f)]
    public float dropChance = 1f;

    void Awake()
    {
        currentHp = maxHp;
    }

    public void HandleProjectileHit(Component sender, object data)
    {
        ProjectileHitPayload payload = (ProjectileHitPayload)data;
        if (payload.target == gameObject)
        {
            TakeDamage(payload.damage);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        TryDropPickup();
        // do deathrattle here
        Destroy(gameObject);
    }

    public void TryDropPickup()
    {
        if (pickupPrefab == null) return;

        if (Random.value <= dropChance)
        {
            Instantiate(
                pickupPrefab,
                transform.position,
                Quaternion.identity
            );
        }
    }
}
