using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipProperties : MonoBehaviour
{
    public int health;
    public int ammoCount;

    public bool DeductAmmo()
    {
        if (ammoCount > 0)
        {
            ammoCount--;
            return true;
        }
        else
            return false;
    }
    
    public bool DeductAmmo(int amount)
    {
        if (ammoCount >= amount)
        {
            ammoCount -= amount;
            return true;
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

}
