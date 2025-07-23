using System.Net.Mail;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Tooltip("Exact name of the GameOver scene (must be in Build Settings)")]
    public string gameOverSceneName = "GameOver";


    public void OnGameOver()
    {
        // PlayerPrefs.SetInt("LastScore", PlayerStats.Instance.GalaxiesJumped);
        // PlayerPrefs.Save();
        SceneManager.LoadScene(gameOverSceneName);
    }

}