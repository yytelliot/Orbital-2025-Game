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
        // Add Score to Shared Scene
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.AddGalaxiesJumped(1);
        }
        
        jumpCount++;
        difficultyMultiplier += difficultyPerJump;

        // reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void OnGameOver()
    {
        StartCoroutine(DestroyNextFrame());
    }

    private IEnumerator DestroyNextFrame()
    {
        // let the event system finish calling every subscriber
        yield return null;
        Destroy(gameObject);
    }

    
}