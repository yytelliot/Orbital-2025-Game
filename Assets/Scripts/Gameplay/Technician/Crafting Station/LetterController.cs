using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterController : MonoBehaviour
{
    public TMP_Text textDisplay;
    private string expected;
    private string currentTyped = "";
    private float lifeTime = 5f;

    public void Initialize(string letters, Vector2 anchoredPos)
    {
        expected = letters;
        textDisplay.text = letters;
        GetComponent<RectTransform>().anchoredPosition = anchoredPos;
        Destroy(gameObject, lifeTime);
    }

    public bool TryType(string input)
    {
        if (expected.StartsWith(currentTyped + input))
        {
            currentTyped += input;
            textDisplay.text = expected.Substring(currentTyped.Length);

            if (currentTyped == expected)
            {
                Destroy(gameObject);
                return true;
            }
        }
        return false;
    }
}
