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
    private Color fireColour;
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

        Color firstFire = new Color(1f, 1f, 1f);
        Color secondFire = new Color(1f, 0.5f, 0.5f);
        Color thirdFire = Color.red;

        if (fireIndex == 0)
            fireColour = firstFire;
        else if (fireIndex == 1)
            fireColour = secondFire; // Half red
        else
            fireColour = thirdFire; // Full red

        
        SpriteRenderer sr = fire.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = fireColour;

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
