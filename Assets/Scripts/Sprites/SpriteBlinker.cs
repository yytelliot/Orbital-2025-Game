using UnityEngine;
using System.Collections;

public class SpriteBlinker : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private float blinkInterval = 0.15f;
    [SerializeField] private float blinkAlpha = 0.3f;

    private Coroutine blinkRoutine;

    void Awake()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
    }

    public void StartBlink(float duration)
    {
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        blinkRoutine = StartCoroutine(BlinkCoroutine(duration));
    }

    public void StopBlink()
    {
        if (blinkRoutine != null)
        {
            StopCoroutine(blinkRoutine);
            blinkRoutine = null;
        }
        SetAlpha(1f);
    }

    private IEnumerator BlinkCoroutine(float duration)
    {
        float timer = 0f;
        bool faded = false;
        while (timer < duration)
        {
            faded = !faded;
            SetAlpha(faded ? blinkAlpha : 1f);

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }
        SetAlpha(1f);
    }

    private void SetAlpha(float a)
    {
        if (sr == null) return;
        var c = sr.color;
        c.a = a;
        sr.color = c;
    }
}
