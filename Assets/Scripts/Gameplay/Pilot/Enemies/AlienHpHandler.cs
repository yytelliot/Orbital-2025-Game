using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

public class AlienHpHandler : MonoBehaviour, ITakeDamage
{
    [Header("Stats")]
    public int baseMaxHp = 5;
    private int maxHp;
    private int currentHp;

    [Header("Drops")]
    [Tooltip("Prefab of the pickup to spawn upon death. Leave empty if no drops")]
    public GameObject pickupPrefab;
    [Range(0f, 1f)]
    public float dropChance = 1f;

    void Awake()
    {   
        // difficulty multiplier
        float dm = PilotGameController.Instance.difficultyMultiplier;
        maxHp = Mathf.RoundToInt(baseMaxHp * dm);
        currentHp = maxHp;
    
    }

    public void HandleProjectileHit(Component sender, object data)
    {
        ProjectileHitPayload payload = (ProjectileHitPayload)data;
        if (payload.target == gameObject)
        {
            var intangible = GetComponent<IIntangible>();
            
            if (intangible != null && intangible.isIntangible) return;
            
            TakeDamage(payload.damage);
            

            
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        AudioManager.PlaySound(AudioLibrary.GetClip("DamageNoise"));
        Debug.Log(currentHp);
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
