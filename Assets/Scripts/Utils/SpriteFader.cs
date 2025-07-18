using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFader : MonoBehaviour
{
    private Coroutine fadeCoroutine;
    private SpriteRenderer sr;
    private float lastRequestedAlpha = -1f; // For fade smoothing

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) Debug.LogError($"SpriteFader on {gameObject.name} requires a SpriteRenderer!");
    }

    public void SetAlpha(float targetAlpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, targetAlpha);
    }

    public void FadeToAlpha(float targetAlpha, float duration)
    {
        if (Mathf.Approximately(lastRequestedAlpha, targetAlpha))
            return; // No need to fade if target alpha is the same as last
        lastRequestedAlpha = targetAlpha;

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
        lastRequestedAlpha = targetAlpha; // update for smoothing
        fadeCoroutine = null;
    }
}