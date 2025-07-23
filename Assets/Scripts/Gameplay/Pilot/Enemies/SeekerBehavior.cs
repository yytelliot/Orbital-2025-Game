using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class SeekerBehavior : MonoBehaviour, IStunnable
{

    [Header("Sprite Info")]
    public float offset = 90;
    private SpriteRenderer sr;

    [Header("Enemy Info")]
    public float moveSpeed;

    [Tooltip("When stunned, can only move once the velocity is lower than stopThreshold")]
    public float stopThreshold = 0.2f;

    [Tooltip("How long before despawn")]
    public float lifetime = 20;

    [Header("SFX")]
    [SerializeField] private AudioClip spawnNoise;

    private GameObject player;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isStunned = false;
    private ShipProperties shipProperties;
    private SpriteFader fader;

    public void Stun(float time) => StartCoroutine(StunCoroutine(time));
    public void StunUntilStop() => StartCoroutine(StunUntilStopCoroutine());

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        fader = GetComponent<SpriteFader>();
        player = GameObject.FindGameObjectWithTag("Player");
        shipProperties = player.GetComponent<ShipProperties>();

        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("SeekerBehavior: Player Not Found in scene");
        }

        fader.SetAlpha(0f);
        StartCoroutine(LifetimeCoroutine(lifetime));
        
    }

    IEnumerator LifetimeCoroutine(float seconds)
    {
        AudioManager.Instance.PlaySFX(spawnNoise);
        fader.FadeToAlpha(1f, 2f);
        yield return new WaitForSeconds(seconds);
        Despawn();
    }

    IEnumerator FadeAndDestroy(float duration)
    {
        fader.FadeToAlpha(0f, duration);
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
    private void Despawn()
    {
        // despawn animation
        StartCoroutine(FadeAndDestroy(1f));
    }

    public IEnumerator StunUntilStopCoroutine()
    {
        isStunned = true;

        while (rb.velocity.sqrMagnitude > stopThreshold * stopThreshold)
        {
            yield return new WaitForFixedUpdate();
        }

        rb.velocity = Vector2.zero;

        isStunned = false;
    }
    IEnumerator StunCoroutine(float stunTime) {
        isStunned = true;
        yield return new WaitForSeconds(stunTime);
        isStunned = false;
    }

    public void onScanSeeker(Component sender, object data)
    { 
        if (data is ScannerRevealPayload payload)
        {
            float dist = Vector2.Distance(transform.position, payload.scannerPosition);
            if (dist <= payload.scannerRadius)
            {
                Stun(payload.scannerStrength);
            }
        }
    }

    void FixedUpdate()
    {
        if (playerTransform == null) return;
        if (isStunned == true) return;

        Vector2 currentPos = rb.position;
        Vector2 targetPos = (Vector2)playerTransform.position;
        Vector2 direction = (targetPos - currentPos).normalized;

        rb.velocity = direction * moveSpeed;
        // Vector2 newPos = currentPos + direction * moveSpeed * Time.fixedDeltaTime;
        // rb.MovePosition(newPos);

    }

    // Update is called once per frame
    void Update()
    {

        // face toward player
        if (!isStunned)
        { 
            Vector2 currentPos = transform.position;
            Vector2 targetPos = (Vector2)playerTransform.position;
            Vector2 direction = (targetPos - currentPos).normalized;

            float angleDeg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angleDeg - offset);
        }


    }
}
