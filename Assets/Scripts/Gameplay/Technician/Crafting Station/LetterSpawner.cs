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
    
    private List<LetterController> activeBoxes = new();

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
        box.Initialize(letters, RandomPositionWithin(panel));
        activeBoxes.Add(box);
    }

    private Vector2 RandomPositionWithin(RectTransform panel)
    {
        /*float x = Random.Range(-1035, -40);
        float y = Random.Range(-696, -40);*/

        
        Vector3[] v = new Vector3[4];
        panel.GetWorldCorners(v);

        float bottomLeftXPos = v[0].x;
        float TopRightXPos = v[2].x;

        float bottomLeftYPos = v[0].y;
        float TopRightYPos = v[2].y;

        Debug.Log("Local size: " + panel.rect.size);
        Debug.Log("Lossy scale: " + panel.lossyScale);



        float x = Random.Range(bottomLeftXPos, TopRightXPos);
        float y = Random.Range(bottomLeftYPos, TopRightYPos);



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

    void Update()
    {
        string input = Input.inputString.ToUpper();
        if (!string.IsNullOrEmpty(input))
        {
            foreach (var box in activeBoxes.ToList())
            {
                activeBoxes.Remove(box);
                break;
            }
        }
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
