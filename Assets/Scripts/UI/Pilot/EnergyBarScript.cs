using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBarScript : MonoBehaviour
{
    public Slider slider;

    private ShipEnergyHandler shipEnergyHandler;


    public void Awake()
    {
        shipEnergyHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipEnergyHandler>();
    }

    public void SetEnergySlider(float energy)
    {
        slider.value = energy;
    }

    void Start()
    {
        UpdateUIEnergyBar();
    }

    public void UpdateUIEnergyBar()
    {
        float currentEnergy = shipEnergyHandler.GetCurrentEnergy();
        float maxEnergy = shipEnergyHandler.GetEnergyToNextLevel();
        if (currentEnergy == 0)
        {
            SetEnergySlider(0);
        }
        else SetEnergySlider((float)currentEnergy / maxEnergy);


    }

    public void UpdateUIEnergyBar(Component sender, object data)
    {
        UpdateUIEnergyBar();
    }



}
