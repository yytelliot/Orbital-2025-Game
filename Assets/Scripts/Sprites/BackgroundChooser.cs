using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundChooser : MonoBehaviour
{
    [Tooltip("Assign one sprite per possible galaxy index (0, 1, 2, …)")]
    public Sprite[] backgroundSprites;

    void Start()
    {
        int idx = BackgroundRandomizer.galaxy;

        // Safety check
        if (idx < 0 || idx >= backgroundSprites.Length)
        {
            Debug.LogWarning($"[BackgroundChooser] galaxy index {idx} is out of range; defaulting to 0.");
            idx = 0;
        }

        // Apply the chosen sprite
        var sr = GetComponent<SpriteRenderer>();
        sr.sprite = backgroundSprites[idx];
    }
}