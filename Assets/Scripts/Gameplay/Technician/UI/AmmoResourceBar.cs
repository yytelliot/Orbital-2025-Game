using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoResourceBar : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float currentAmmoRes = 100f;
    
    [SerializeField] private float maxAmmo = 100f;
    [Header("References")]
    [SerializeField] private Slider ammoSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text ammoResourceText;

    [Header("Color Settings")]
    [SerializeField] private Color highAmmoColor = Color.green;
    [SerializeField] private Color midAmmoColor = Color.yellow;
    [SerializeField] private Color lowAmmoColor = Color.red;
    [SerializeField, Range(0f, 100f)] private float lowAmmoThreshold = 30f;
    [SerializeField, Range(0f, 100f)] private float midAmmoThreshold = 50f;

    private Color targetColor;

    private void Update()
    {
        currentAmmoRes = Mathf.Clamp(currentAmmoRes, 0f, maxAmmo);
        float normalizedAmmo = currentAmmoRes / maxAmmo;
        ammoSlider.value = normalizedAmmo;
        ammoResourceText.text = currentAmmoRes + "/" + maxAmmo;

        if (currentAmmoRes > midAmmoThreshold)
        {
            float t = (currentAmmoRes - midAmmoThreshold) / (maxAmmo - midAmmoThreshold);
            targetColor = Color.Lerp(midAmmoColor, highAmmoColor, t);
        }
        else if (currentAmmoRes > lowAmmoThreshold)
        {
            float t = (currentAmmoRes - lowAmmoThreshold) / (midAmmoThreshold - lowAmmoThreshold);
            targetColor = Color.Lerp(lowAmmoColor, midAmmoColor, t);
        }
        else
        {
            targetColor = lowAmmoColor;
        }

        fillImage.color = targetColor;
    }


    public void SetAmmoRes(float ammoChange)
    {
        currentAmmoRes += ammoChange;
        
    }
}
