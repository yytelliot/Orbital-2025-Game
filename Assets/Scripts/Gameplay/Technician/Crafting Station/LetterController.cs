using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LetterController : MonoBehaviour
{
    [SerializeField] private TMP_Text textDisplay;
    private CanvasGroup canvasGroup;
    public string expected;
    private string currentTyped = "";
    private float lifeTime;

    [SerializeField] private float fadeElapsed = 0f;

    private Coroutine feedbackRoutine;
    private Color originalColor;
    [SerializeField] private Image backgroundImage; // Assign this in the Inspector

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (!canvasGroup) canvasGroup = gameObject.AddComponent<CanvasGroup>(); // add canvasgroup component if removed  
    }

    public void Initialize(string letters, Vector2 anchoredPos, float lifeTime)
    {
        this.lifeTime = lifeTime;
        expected = letters;
        textDisplay.text = letters;
        GetComponent<RectTransform>().anchoredPosition = anchoredPos;
        StartCoroutine(FadeOutAndDestroy());

    }

    public bool TryType(string input)
    {
        if (expected.StartsWith(currentTyped + input))
        {
            currentTyped += input;
            textDisplay.text = expected.Substring(currentTyped.Length);

            if (currentTyped == expected)
            {
                return true;
            }
        }
        else
        {
            ShowWrongKeyFeedback();
        }
        return false;
    }

    private IEnumerator FadeOutAndDestroy()
    {
        while (fadeElapsed < lifeTime)
        {
            fadeElapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeElapsed / lifeTime);
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, fadeElapsed / lifeTime);
            yield return null;
        }

        Destroy(gameObject);
    }

    public void ShowWrongKeyFeedback()
    {
        if (feedbackRoutine != null)
            StopCoroutine(feedbackRoutine);

        feedbackRoutine = StartCoroutine(WrongKeyFeedback());
    }

    private IEnumerator WrongKeyFeedback()
    {
        originalColor = backgroundImage.color;
        backgroundImage.color = Color.red;

        Vector3 originalPos = transform.localPosition;

        //Shaking settings
        float shakeIntensity = 8f;  
        float duration = 0.15f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float offsetX = Random.Range(-1f, 1f) * shakeIntensity;
            float offsetY = Random.Range(-1f, 1f) * shakeIntensity;
            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);
            yield return null;
        }

        backgroundImage.color = originalColor;
        transform.localPosition = originalPos;
        feedbackRoutine = null;
    }
}
