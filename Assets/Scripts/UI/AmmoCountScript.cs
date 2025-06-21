using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCountScript : MonoBehaviour
{
    public ShipProperties shipProperties;
    public Text ammoText;

    // Start is called before the first frame update
    void Start()
    {
        ammoText.text = shipProperties.getCurrentAmmo().ToString();
    }

    public void UpdateUIAmmoCount()
    {
        ammoText.text = shipProperties.getCurrentAmmo().ToString();
    }
}
