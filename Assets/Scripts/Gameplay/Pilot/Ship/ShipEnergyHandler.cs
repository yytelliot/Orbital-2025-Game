using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEnergyHandler : MonoBehaviour
{
    public GameEvent readyToJump;
    public GameEvent energyPercentageFilledEvent;

    public int baseEnergyToNextLevel = 20;
    public int currentEnergy = 0;
    private int energyToNextLevel;

    [Header("Events")]
    public GameEvent updateUI;

    public void Start()
    {
        // difficulty multiplier
        float dm = PilotGameController.Instance.difficultyMultiplier;

        energyToNextLevel = Mathf.RoundToInt(baseEnergyToNextLevel * dm);
    }

    public void AddEnergy(int amount)
    {
        currentEnergy += amount;
        energyPercentageFilledEvent.RaiseNetworked(this, (float)currentEnergy / energyToNextLevel);
        if (currentEnergy >= energyToNextLevel)
        {
            currentEnergy = energyToNextLevel;
            readyToJump.RaiseNetworked(this, null);

            // raise activatie galaxy jump event to technician side
        }
        updateUI.Raise();
    }

    public void LoseEnergy(int amount)
    {
        currentEnergy -= amount;
        energyPercentageFilledEvent.RaiseNetworked(this, (float)currentEnergy / energyToNextLevel);
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
