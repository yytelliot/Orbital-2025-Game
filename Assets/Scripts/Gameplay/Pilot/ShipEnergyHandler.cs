using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEnergyHandler : MonoBehaviour
{

    public int energyToNextLevel = 25;
    public int currentEnergy = 0;
    // Start is called before the first frame update

    [Header("Events")]
    public GameEvent updateUI;

    public void AddEnergy(int amount)
    {
        currentEnergy += amount;
        if (currentEnergy >= energyToNextLevel)
        {
            currentEnergy = energyToNextLevel;
            // raise activatie galaxy jump event to technician side
        }
        updateUI.Raise();
    }

    public void LoseEnergy(int amount)
    {
        currentEnergy -= amount;
        if (currentEnergy <= 0)
        {
            currentEnergy = 0;
        }
        if (currentEnergy < energyToNextLevel)
        {
            //raise disable galaxy jump event to technician side
        }
    }

    public int GetCurrentEnergy()
    {
        return currentEnergy;
    }

    public int GetEnergyToNextLevel()
    {
        return energyToNextLevel;
    }
}
