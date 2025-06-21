using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipProperties : MonoBehaviour
{
    public int health;
    public int maxAmmoCount = 100;
    public int currentAmmoCount;


    [Header("Events")]
    public GameEvent onAmmoCountChange;
    public GameEvent onOutOfAmmo;
    public GameEvent onAmmoFull;
    public GameEvent updateUI;

    public int getCurrentAmmo()
    {
        return currentAmmoCount;
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

    public bool DeductHealth(int amount)
    {
        if (health >= amount)
        {
            health -= amount;
            return true;
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

}
