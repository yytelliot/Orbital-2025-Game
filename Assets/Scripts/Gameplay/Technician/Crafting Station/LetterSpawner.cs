using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
public enum Difficulty {Easy, Medium, Hard}
public class LetterSpawner : MonoBehaviour
{

    [Header("Letter Prefab")]
    [SerializeField] private GameObject LetterBoxPrefab;

    [Header("Game Setup")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private Difficulty currentDifficulty;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private Transform SpawnPos;
    [SerializeField] private float marginx;
    [SerializeField] private float marginy;
    [SerializeField] private float lifeTime = 5f;
    
    private List<LetterController> activeBoxes = new();
    void Update()
    {
        string input = Input.inputString.ToUpper();
        if (!string.IsNullOrEmpty(input))
        {
            foreach (var box in activeBoxes.ToList())
            {
                if (box == null || box.gameObject == null)
                {
                    activeBoxes.Remove(box); // Cleanup destroyed box
                    continue;
                }
                
                if (box.TryType(input))
                {
                    activeBoxes.Remove(box);
                    Destroy(box.gameObject);
                    break;
                }

            }
        }
    }
    public void StartMinigame()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        for (int i = 0; i < GetBoxesCount(); i++)
        {
            SpawnBox();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnBox()
    {
        GameObject newBox = Instantiate(LetterBoxPrefab);
        newBox.transform.SetParent(panel.transform, false); // important: false = keep local position
        LetterController box = newBox.GetComponent<LetterController>();

        string letters = GetRandomLetters();
        box.Initialize(letters, RandomPositionWithin(panel), lifeTime);
        activeBoxes.Add(box);
    }

    private Vector2 RandomPositionWithin(RectTransform panel) //currently unable to find a way to convert panel coord accurately, so using hard code.
    {
        float x = Random.Range(-1000, -50);
        float y = Random.Range(-650, -50);



        return new Vector2(x, y);
        

    }

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

    public int GetBoxesCount()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => 10,
            Difficulty.Medium => 20,
            Difficulty.Hard => 30,
            _ => 10
        };
    }



    public void EndMinigame()
    {
        foreach (var box in activeBoxes)
        {
            if (box != null)
                Destroy(box.gameObject);
        }
        activeBoxes.Clear();
    }
}
