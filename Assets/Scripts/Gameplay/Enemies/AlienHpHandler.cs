using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

public class AlienHpHandler : MonoBehaviour, ITakeDamage
{
    [Header("Stats")]
    public int maxHp;
    public int currentHp;

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
        // do deathrattle here
        Destroy(gameObject);
    }
}
