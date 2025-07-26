using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;


public class ShipProperties : MonoBehaviour, IStunnable, ITakeDamage
{

    [Header("Ship Hp Attributes")]
    public int maxHp;
    public int currentHp;
    public int maxHpThresholds = 4;
    public int currentHpThrehsolds = 4;

    [Header("Ship Ammo Attributes")]
    public int maxAmmoCount = 100;
    public int currentAmmoCount;

    [Header("Ship Stun Attributes")]
    public float defaultStunedTime = 0.5f;
    private bool isStunned = false;
    [Header("Damage Invul")]
    public float onHitInvulTime = 3;
    private bool isInvul = false;

    [SerializeField]
    [Tooltip("Minimum speed after being stunned for the pilot to regain control")]
    private float speedToRegainControl = 1;


    [Header("Events")]
    public GameEvent onAmmoCountChange;
    public GameEvent onOutOfAmmo;
    public GameEvent onAmmoFull;
    public GameEvent updateUI;
    public GameEvent onShipHpChange;
    public GameEvent onShipHpReachZero;
    public GameEvent emergencyRepairsRequired;
    public GameEvent gameOverEvent;

    private Rigidbody2D rb;
    private SpriteBlinker blinker;

    public bool IsStunned()
    {
        return isStunned;
    }

    public void Stun()
    {
        StartCoroutine(StunCoroutine(defaultStunedTime));
    }
    public void Stun(float time)
    {
        StartCoroutine(StunCoroutine(time));
    }

    public void Stun(Component sender, object data)
    {
        float stunnedTime = (float)data;
        StartCoroutine(StunCoroutine(stunnedTime));
    }

    public void StunUntilStop()
    {
        StartCoroutine(StunUntilStopCoroutine());
    }

    private IEnumerator StunCoroutine(float time)
    {
        isStunned = true;
        yield return new WaitForSeconds(time);
        isStunned = false;
    }
    private IEnumerator StunUntilStopCoroutine()
    {
        isStunned = true;

        while (rb.velocity.sqrMagnitude > speedToRegainControl * speedToRegainControl)
        {
            yield return new WaitForFixedUpdate();
        }

        isStunned = false;

    }



    // AMMO FUNCTIONS
    public int GetCurrentAmmo()
    {
        return currentAmmoCount;
    }

    public bool AmmoIsFull()
    {
        return currentAmmoCount == maxAmmoCount;
    }

    public bool AmmoIsEmpty()
    {
        return currentAmmoCount <= 0;
    }

    public bool DeductAmmo()
    {
        if (currentAmmoCount > 0)
        {
            onAmmoCountChange.RaiseNetworked(this, -1);
            return true;
        }
        else
            return false;
    }

    public bool DeductAmmo(int amount)
    {

        if (currentAmmoCount >= amount)
        {
            onAmmoCountChange.RaiseNetworked(this, -amount);
            return true;
        }
        else if (currentAmmoCount <= 0)
        {
            onOutOfAmmo.RaiseNetworked(this, null);
            return false;
        }
        else
            return false;

    }

    public void UpdateAmmo(Component sender, object data)
    {
        int amount = (int)data;
        if (currentAmmoCount + amount <= 0)
        {
            currentAmmoCount = 0;
            onOutOfAmmo.RaiseNetworked(this, null);

        }
        else if (currentAmmoCount + amount >= maxAmmoCount)
        {
            currentAmmoCount = maxAmmoCount;
            onAmmoFull.RaiseNetworked(this, null);
        }
        else
        {
            currentAmmoCount += amount;
        }

        updateUI.Raise();
    }



    // HP Functions
    public bool HpAtCap()
    {
        return currentHp == maxHp * currentHpThrehsolds / maxHpThresholds || currentHp == 0;
    }
    public int GetMaxHp()
    {
        return maxHp;
    }
    public int GetCurrentHp()
    {
        return currentHp;
    }

    public void TakeDamage(int amount)
    {
        if (isInvul) return;
        Debug.Log("Took damage:");
        Debug.Log(amount);
        DeductHp(amount);
        AudioManager.PlaySound(AudioLibrary.GetClip("DamageNoise"));
        StartCoroutine(InvulCorutine(onHitInvulTime));
    }
    private bool DeductHp(int amount)
    {
        if (currentHp >= 0)
        {
            onShipHpChange.RaiseNetworked(this, -amount);
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator InvulCorutine(float time)
    {
        isInvul = true;
        if (blinker != null)
            blinker.StartBlink(time);

        yield return new WaitForSeconds(time);
        isInvul = false;
        if (blinker != null)
            blinker.StopBlink();
    }

    public void HandleProjectileHit(Component sender, object data)
    {
        ProjectileHitPayload payload = (ProjectileHitPayload)data;

        if (payload.target == gameObject)
        {
            TakeDamage(payload.damage);
        }
    }


    public void RestoreHpThreshold()
    {
        ChangeHpThresholdBy(1);
    }

    public void ChangeHpThresholdBy(int thresholds)
    {
        if (currentHpThrehsolds + thresholds <= maxHpThresholds && currentHpThrehsolds + thresholds >= 0)
        {
            currentHpThrehsolds += thresholds;
        }
        else if (currentHpThrehsolds + thresholds > maxHpThresholds)
        {
            currentHpThrehsolds = maxHpThresholds;
        }
        else
        {
            currentHpThrehsolds = 0;
        }

        updateUI.Raise();

    }

    public bool AddHp(int amount)
    {
        if (currentHp == maxHp * currentHpThrehsolds / maxHpThresholds || currentHp == 0)
        {
            return false;
        }
        onShipHpChange.RaiseNetworked(this, amount);
        return true;
    }


    public void UpdateHp(Component sender, object data)
    {
        int currentMaxHp = maxHp * currentHpThrehsolds / maxHpThresholds;
        int amount = (int)data;

        // if hp is zero, die
        if (currentHp + amount <= 0 || currentMaxHp <= 0)
        {
            currentHp = 0;
            // Debug.Log("Lmao ded");
            updateUI.Raise();
            // onShipHpReachZero.RaiseNetworked(this, null);
            gameOverEvent.RaiseNetworked(this, null);
            return;
        }

        if (amount >= 0)
        {
            if (currentHp + amount >= currentMaxHp)
            {
                currentHp = currentMaxHp;
                updateUI.Raise();
                return;
            }
            currentHp += amount;
            updateUI.Raise();
            return;
        }


        // if hp restore will be abover cap, cap it at max hp        
        else
        {
            int oldThresholds = Mathf.CeilToInt((float)currentHp / maxHp * maxHpThresholds);

            currentHp += amount;

            int newThresholds = Mathf.CeilToInt((float)currentHp / maxHp * maxHpThresholds);
            newThresholds = Mathf.Clamp(newThresholds, 0, maxHpThresholds);

            // 4) if you actually crossed downward into a lower band, do the emergency
            if (newThresholds < oldThresholds)
            {
                currentHpThrehsolds-- ;
                emergencyRepairsRequired.RaiseNetworked(this, null);
            }

            updateUI.Raise();
        }

        updateUI.Raise();
    }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        blinker = GetComponent<SpriteBlinker>();
    }

}
