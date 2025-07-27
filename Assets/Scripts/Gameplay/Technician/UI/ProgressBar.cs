using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ProgressBar : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float currentProgress = 0f;
    
    [SerializeField] private float maxProgress = 1f;
    [Header("References")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text progressResourceText;

    [Header("Color Settings")]
    [SerializeField] private Color highProgressColor = Color.green;
    [SerializeField] private Color midProgressColor = Color.yellow;
    [SerializeField] private Color lowProgressColor = Color.red;
    [SerializeField, Range(0f, 100f)] private float lowProgressThreshold = 30f;
    [SerializeField, Range(0f, 100f)] private float midProgressThreshold = 50f;

    private Color targetColor;

    private void Update()
    {
        currentProgress = Mathf.Clamp(currentProgress, 0f, maxProgress);
        float normalisedProgress = currentProgress / maxProgress;
        progressSlider.value = normalisedProgress;

        float rounded = Mathf.Ceil(currentProgress *100);
        progressResourceText.text = rounded + "% " + "/" + maxProgress * 100 + "%";

        if (currentProgress > midProgressThreshold)
        {
            float t = (currentProgress - midProgressThreshold) / (maxProgress - midProgressThreshold);
            targetColor = Color.Lerp(midProgressColor, highProgressColor, t);
        }
        else if (currentProgress > lowProgressThreshold)
        {
            float t = (currentProgress - lowProgressThreshold) / (midProgressThreshold - lowProgressThreshold);
            targetColor = Color.Lerp(lowProgressColor, midProgressColor, t);
        }
        else
        {
            targetColor = lowProgressColor;
        }

        fillImage.color = targetColor;
    }


    public void SetProg(Component sender, object data)
    {
        currentProgress = (float)data;
    }
}
