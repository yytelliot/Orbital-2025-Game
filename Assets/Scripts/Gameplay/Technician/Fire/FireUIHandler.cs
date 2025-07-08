using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireUIHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject firePrefab;

    [Header("Spawn Settings")]
    [SerializeField] private Vector3[] firePos;

    private int fireIndex = 0;
    private FireTrackerVariant[] fireList;
    public static FireUIHandler Instance; //Singleton pattern

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Ensure arrays are initialized with the right size
        int size = firePos.Length;
        fireList = new FireTrackerVariant[size];

    }

    public void addFire()
    {

        if (fireIndex >= firePos.Length)
        {
            Debug.LogWarning("No more fire slots available!");
            return;
        }

        GameObject fire = Instantiate(firePrefab, firePos[fireIndex], Quaternion.identity);

         // Gradually increase redness from fire 1 to fire 6
        float intensity = Mathf.Clamp01((fireIndex + 1) / 6f);  // 1/6 to 6/6
        Color fireColor = Color.Lerp(new Color(1f, 0.6f, 0.6f), Color.red, intensity);  // Soft red to full red

        
        SpriteRenderer sr = fire.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = fireColor;

        fireList[fireIndex] = fire.GetComponent<FireTrackerVariant>();
        fireIndex++;
    }

    public void removeFire()
    {
        if (fireIndex <= 0)
        {
            Debug.LogWarning("No fires to remove!");
            return;
        }

        fireIndex--;
        fireList[fireIndex].ExtinguishFireVariant();
        fireList[fireIndex] = null;
    }
}
