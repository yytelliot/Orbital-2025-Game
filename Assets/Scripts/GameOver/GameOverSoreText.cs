using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverSoreText : MonoBehaviour
{
    // Start is called before the first frame update
    private Text scoreText;
    void Awake()
    {
        scoreText.text = PlayerPrefs.GetInt("LastScore", 0).ToString();
    }

}
