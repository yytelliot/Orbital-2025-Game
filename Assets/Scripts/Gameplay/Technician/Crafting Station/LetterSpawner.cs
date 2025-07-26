using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
public enum Difficulty {None, Easy, Medium, Hard}
public class LetterSpawner : MonoBehaviour
{
    

    [Header("References")]
    public GameObject miniGame;
    

    [Header("Prefab")]
    [SerializeField] private GameObject LetterBoxPrefab;

    [Header("Game Setup")]
    [SerializeField] private RectTransform panel;
     private Difficulty currentDifficulty;
    [SerializeField] private Transform SpawnPos;
    [SerializeField] private float marginx;
    [SerializeField] private float marginy;
    [SerializeField] private TextAsset wordListAsset;
    private List<string> wordBank = new();
    
    private List<LetterController> activeBoxes = new();
    [SerializeField] private TMP_Text statusText;
    private int ciphersSolved = 0;
    private CipherFactory cipherFactory;

    private void Awake()
    {
        if (wordListAsset != null)
        {
            wordBank = wordListAsset.text
                .Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
                .Where(word => !string.IsNullOrWhiteSpace(word))
                .Select(word => word.ToUpper())
                .ToList();
        }

        cipherFactory = new CipherFactory(LetterBoxPrefab, panel);
    }

    void Update()
    {
        string input = Input.inputString.ToUpper();
        if (!string.IsNullOrEmpty(input))
        {
            foreach (var box in activeBoxes.ToList())
            {
                if (box == null || box.gameObject == null)
                {
                    activeBoxes.Remove(box); 
                    continue;
                }
                
                if (box.TryType(input))
                {
                    activeBoxes.Remove(box);
                    AddScore();
                    Destroy(box.gameObject);
                    break;
                }

            }
        }
    }
    public void StartMinigame(Component sender, object difficulty)
    {
        currentDifficulty = (Difficulty)difficulty;

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        for (int i = 0; i < GetBoxesCount(); i++)
        {
            if (ciphersSolved >= GetTargetAmount())
            {
                break;
            }

            SpawnBox();
            yield return new WaitForSeconds(GetSpawnInterval());
        }

        StartCoroutine(WaitUntilMinigameEnds());
    }


    private void SpawnBox()
    {
        string word = GetRandomWord();
        Vector2 position = RandomPositionWithin(panel);
        int lifetime = GetBoxLifetime();

        GameObject newBox = cipherFactory.CreateCipher(word, position, lifetime);

        newBox.transform.SetParent(panel.transform, false); // important: false = keep local position
        LetterController box = newBox.GetComponent<LetterController>();

        activeBoxes.Add(box);
    }
    

    private Vector2 RandomPositionWithin(RectTransform panel) //currently unable to find a way to convert panel coord accurately, so using hard code.
    {
        float x = Random.Range(-1000, -200); //not -50 to compensate for word length
        float y = Random.Range(-650, -50);



        return new Vector2(x, y);


    }
    private void EndMinigame()
    {
        foreach (var box in activeBoxes)
        {
            if (box != null)
                Destroy(box.gameObject);
        }
        activeBoxes.Clear();
        miniGame.SetActive(false);
        ciphersSolved = 0;
    }   

    IEnumerator WaitUntilMinigameEnds()
    {
        while (activeBoxes.Count > 0)
        {
            // Remove any destroyed boxes from the list
            activeBoxes.RemoveAll(box => box == null || box.gameObject == null);

            if (ciphersSolved >= GetTargetAmount())
            {
                break; 
            }

            yield return null;
        }

        // Minigame has ended
        CraftingStationController.Instance.SendResult(ciphersSolved >= GetTargetAmount(), GetDifficultyLevel()); 
        EndMinigame();
    }

    #region Difficulty Settings
    private string GetRandomLetters()
    {
        int length = currentDifficulty switch
        {
            Difficulty.Easy => 1,
            Difficulty.Medium => 2,
            Difficulty.Hard => 3,
            _ => 1
        };

        return new string(Enumerable.Range(0, length)
        .Select(_ => (char)Random.Range('A', 'Z' + 1))
        .ToArray());
    }
    
    private string GetRandomWord()
    {
        if (Random.Range(0, 10) >= 7)
        {
            return GetRandomLetters();
        }

        int minLength, maxLength;

        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                minLength = 3; maxLength = 4;
                break;
            case Difficulty.Medium:
                minLength = 5; maxLength = 7;
                break;
            case Difficulty.Hard:
                minLength = 8; maxLength = 20;
                break;
            default:
                minLength = 3; maxLength = 6;
                break;
        }

        var validWords = wordBank.Where(w => w.Length >= minLength && w.Length <= maxLength).ToList();
        if (validWords.Count == 0)
            return "DEFAULT";

        return validWords[Random.Range(0, validWords.Count)];
    }

    private int GetBoxesCount()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => 10,
            Difficulty.Medium => 20,
            Difficulty.Hard => 30,
            _ => 10
        };
    }

    private int GetTargetAmount()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => 6,
            Difficulty.Medium => 15,
            Difficulty.Hard => 25,
            _ => 6
        };
    }
    
    private int GetBoxLifetime()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => 10,
            Difficulty.Medium => 11,
            Difficulty.Hard => 12,
            _ => 10
        };
    }

    private float GetSpawnInterval()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => 1f,
            Difficulty.Medium => 1.3f,
            Difficulty.Hard => 1.5f,
            _ => 1f
        };
    }

    private int GetDifficultyLevel()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => 1,
            Difficulty.Medium => 2,
            Difficulty.Hard => 3,
            _ => 1
        };
    }
    #endregion

    public void AddScore()
    {
        ciphersSolved++;
        statusText.text = "Ciphers Solved: " + ciphersSolved + " / " + GetTargetAmount();

        // Immediately end game if target reached
        if (ciphersSolved >= GetTargetAmount())
        {
            StopAllCoroutines(); // Stop any ongoing spawning
            StartCoroutine(WaitUntilMinigameEnds());
        }
    }


}
