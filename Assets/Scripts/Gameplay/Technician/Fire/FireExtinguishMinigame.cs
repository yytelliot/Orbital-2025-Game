using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireExtinguishMinigame : MonoBehaviour
{
    [Header("References")]
    public Slider playerSlider;
    public Image safeZoneImage;
    public GameObject minigame;

    [Header("Safe Zone Settings")]
    [Range(0f, 1f)] public float safeZoneWidth = 0.2f;
    //public float safeZoneMoveSpeed = 0.3f; // units per second

    [Header("Completion")]
    public float timeToWin = 5f;
    public float maxTime = 12f;
    private float timeInSafeZone = 0f;

    [Header("Fire Visual")]
    public Image fireImage;
    public Vector3 fireStartScale = Vector3.one;
    public Vector3 fireEndScale = Vector3.zero;

    public static FireExtinguishMinigame Instance; //Singleton pattern

    private RectTransform safeZoneRect;
    private float safeZoneMin;

    private float safeZoneTargetMin;
    private float safeZoneMoveTimer;
    private float safeZoneMoveDuration;
    private float safeZonePauseTimer;
    private float currentSafeZonePos;
    private float startSafeZonePos;

    private bool completed;
    private FireTracker currentFire;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        safeZoneRect = safeZoneImage.GetComponent<RectTransform>();
        //MoveSafeZone(); // initialize       
    }


    public void StartExtinguishingMinigame(FireTracker fire)
    {
        minigame.SetActive(true);

        //Game Setup
        timeInSafeZone = 0f;
        currentFire = fire;
        currentSafeZonePos = Random.Range(0f, 1f - safeZoneWidth);
        safeZoneMoveTimer = 0;
        safeZonePauseTimer = 0;
        completed = false;

        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        float totalElapsedTime = 0f;

        while (!completed)
        {
            HandlePlayerInput();
            MoveSafeZone();
            CheckIfInSafeZone();
            UpdateFireVisual();

            totalElapsedTime += Time.deltaTime;

            if (totalElapsedTime >= maxTime)
            {
                break;
            }

            yield return null;
        }

        if (currentFire != null && timeInSafeZone >= timeToWin)
        {
            currentFire.ExtinguishFire();
        }

        minigame.SetActive(false);
        currentFire = null;

        
    }



    private void HandlePlayerInput()
    {
        float input = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
        playerSlider.value += input * Time.deltaTime;
        playerSlider.value = Mathf.Clamp01(playerSlider.value);

    }

    private void MoveSafeZone()
    {
        // Start a new movement if paused long enough
        if (safeZonePauseTimer <= 0)
        {
            //Starts movement by choosing a target to move to, and the duration
            //(only runs at the start of a new movement)
            if (safeZoneMoveTimer <= 0)
            {
                safeZoneTargetMin = Random.Range(0f, 1f - safeZoneWidth); //Randomly pick a target location
                safeZoneMoveDuration = Random.Range(1f, 2f);
                safeZoneMoveTimer = safeZoneMoveDuration;
                startSafeZonePos = currentSafeZonePos;
            }

            //Move the bar to the position
            //float elapsed = safeZoneMoveDuration - safeZoneMoveTimer; 
            float t = 1 - safeZoneMoveTimer / safeZoneMoveDuration; //represents movement from 0 to 1, intially 0, ending at 1 when timer is at 0.
            t = Mathf.SmoothStep(0f, 1f, t); // smoother acceleration and deceleration
            currentSafeZonePos = Mathf.Lerp(startSafeZonePos, safeZoneTargetMin, t); //interpolation, initial at currentSafeZonePos (0), final at safeZoneTargetMin(1)

            safeZoneMoveTimer -= Time.deltaTime;

            //Checks if the movement is completed
            if (safeZoneMoveTimer <= 0)
            {
                currentSafeZonePos = safeZoneTargetMin;
                safeZonePauseTimer = Random.Range(0.1f, 0.3f); // wait before moving again
            }
        }
        else
        {
            safeZonePauseTimer -= Time.deltaTime;
        }

        safeZoneMin = currentSafeZonePos;
        UpdateSafeZoneUI(); // Update visual safe zone

    }

    private void UpdateSafeZoneUI()         
    {
        float totalWidth = ((RectTransform)playerSlider.fillRect.parent).rect.width;
        float pixelWidth = totalWidth * safeZoneWidth;
        float pixelLeft = totalWidth * safeZoneMin;


        safeZoneRect.anchorMin = new Vector2(0, 0);
        safeZoneRect.anchorMax = new Vector2(0, 1);
        safeZoneRect.pivot = new Vector2(0, 0.5f);
        safeZoneRect.sizeDelta = new Vector2(pixelWidth, 0);
        safeZoneRect.anchoredPosition = new Vector2(pixelLeft, 0);        
    }

    private void UpdateFireVisual()
    {
        float progress = Mathf.Clamp01(timeInSafeZone / timeToWin);
        fireImage.rectTransform.localScale = Vector3.Lerp(fireStartScale, fireEndScale, progress);
    }
    
    private void CheckIfInSafeZone()
    {
        if (playerSlider.value >= safeZoneMin && playerSlider.value <= safeZoneMin + safeZoneWidth)
        {
            timeInSafeZone += Time.deltaTime;
        }

        if (timeInSafeZone >= timeToWin)
        {
            Debug.Log("Minigame Complete!");
            completed = true;
            return;
        }
    }
}
