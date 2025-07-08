using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;

    public ShipProperties shipProperties;


    public void Awake()
    {
        shipProperties = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipProperties>();
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }

    void Start()
    {
        UpdateUIHpBar();
    }

    public void UpdateUIHpBar()
    {
        float currentHp = shipProperties.GetCurrentHp();
        float maxHp = shipProperties.GetMaxHp();
        SetHealth(currentHp/maxHp);


    }

    public void UpdateUIHpBar(Component sender, object data)
    {
        UpdateUIHpBar();
    }



}
