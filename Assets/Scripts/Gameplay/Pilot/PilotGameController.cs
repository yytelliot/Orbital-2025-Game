using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PilotGameController : MonoBehaviour
{
    public static PilotGameController Instance { get; private set; }

    [Tooltip("How many times the player has jumped")]
    public int jumpCount = 0;

    [Tooltip("Base difficulty multiplier (1 = default)")]
    public float difficultyMultiplier = 1f;

    [Tooltip("How much to bump difficulty each jump")]
    public float difficultyPerJump = 0.1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If reloading the same scene, destroy the new copy
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Call this when your ship “jumps” (full energy).
    /// It increments jumpCount, raises the multiplier, then reloads the scene.
    /// </summary>
    public void OnJump()
    {
        jumpCount++;
        difficultyMultiplier += difficultyPerJump;

        // reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // 1. Stop game logic
        Time.timeScale = 0f;             // freeze physics & animations  
        DisablePlayerControls();         // your own method to turn off inputs  
        
        // 2. Show UI or load scene
        if (gameOverUIPanel != null) {
            gameOverUIPanel.SetActive(true);
        } else if (!string.IsNullOrEmpty(gameOverSceneName)) {
            Time.timeScale = 1f;        // reset before scene load
            SceneManager.LoadScene(gameOverSceneName);
        }

    }

    void DisablePlayerControls()
    {
        // find your pilot & technician controllers and disable them
        var pilot = FindObjectOfType<PilotController>();
        if (pilot != null) pilot.enabled = false;

        var tech = FindObjectOfType<TechnicianController>();
        if (tech != null) tech.enabled = false;
    }
}