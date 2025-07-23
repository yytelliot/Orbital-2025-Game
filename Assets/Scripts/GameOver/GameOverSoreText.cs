using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverSoreText : MonoBehaviour
{
    // Start is called before the first frame update
    public Text scoreText;
    void Start()
    {
        int final = PlayerStats.Instance != null
                    ? PlayerStats.Instance.CurrentScore
                    : 0;
        scoreText.text = $"Galaxies Jumped: {final}";
    }

}
