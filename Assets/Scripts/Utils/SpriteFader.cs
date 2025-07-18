using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFader : MonoBehaviour
{
    private Coroutine fadeCoroutine;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) Debug.LogError($"SpriteFader on {gameObject.name} reqyres a SpriteRenderer!");
    }

    public void FadeToAlpha(float targetAlpha, float duration)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeAlphaCoroutine(targetAlpha, duration));
    }

    private IEnumerator FadeAlphaCoroutine(float targetAlpha, float duration)
    {
        float startAlpha = sr.color.a;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            sr.color = new Color(1, 1, 1, newAlpha);
            yield return null;
        }
        sr.color = new Color(1, 1, 1, targetAlpha);
        fadeCoroutine = null;
    }
}