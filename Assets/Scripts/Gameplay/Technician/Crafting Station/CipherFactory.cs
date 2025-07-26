using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CipherFactory
{
    private GameObject prefab;
    private RectTransform panel;

    public CipherFactory(GameObject prefab, RectTransform panel)
    {
        this.prefab = prefab;
        this.panel = panel;
    }

    public GameObject CreateCipher(string word, Vector2 position, int lifetime)
    {
        GameObject newBox = Object.Instantiate(prefab);
        newBox.transform.SetParent(panel.transform, false);

        LetterController box = newBox.GetComponent<LetterController>();
        box.Initialize(word, position, lifetime);

        AdjustBoxSize(newBox, word.Length);

        return newBox;
    }

    private void AdjustBoxSize(GameObject letterBox, int wordLength)
    {
        RectTransform rectTransform = letterBox.GetComponent<RectTransform>();
        if (rectTransform == null) return;

        float baseWidth = 100f;
        float widthPerCharacter = 50f;
        float baseHeight = 120f;

        float newWidth = baseWidth + (widthPerCharacter * (wordLength - 1));
        rectTransform.sizeDelta = new Vector2(newWidth, baseHeight);

        Image bgImage = letterBox.GetComponent<Image>();
        if (bgImage != null)
        {
            bgImage.preserveAspect = false;
        }
    }
}
