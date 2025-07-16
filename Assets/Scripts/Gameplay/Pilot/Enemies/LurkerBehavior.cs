using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;


#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class LurkerBehavior : MonoBehaviour, IIntangible, IStunnable
{
    public float revealRadius = 5f;
    public float tangibleDuration = 3f;
    public float shootInterval = 3f;
    public bool isIntangible { get; private set; } = true;
    public bool isStunned { get; private set; } = false;
    public GameObject projectilePrefab;

    [Header("Projectile Pattern")]
    [Tooltip("Number of bullets per spread")]
    public int numBullets = 3;
    [Tooltip("Spread of bullets in degrees")]
    public float spread = 45f;

    [Header("Sprite Info")]
    public float intangibleAlpha = 0.05f;

    private Transform player;
    private SpriteRenderer sr;
    private SpriteFader fader;
    private Collider2D col;
    private float shootTimer = 0f;
    private Coroutine tangibleCoroutine;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sr = GetComponent<SpriteRenderer>();
        fader = GetComponent<SpriteFader>();
        col = GetComponent<Collider2D>();
        fader.FadeToAlpha(intangibleAlpha, 0);
        if (isIntangible)
        {
            BecomeIntangible();
        }
    }

    void Update()
    {
        float dist = Vector2.Distance(player.position, transform.position);

        if (isIntangible && dist < revealRadius)
        {
            // Start/restart the coroutine
            if (tangibleCoroutine != null)
                StopCoroutine(tangibleCoroutine);

            tangibleCoroutine = StartCoroutine(TangibleStateCoroutine());
        }

        // Shooting logic
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f && !isStunned)
        {
            ShootSpreadAtPlayer();
            shootTimer = shootInterval;
        }
    }

    IEnumerator TangibleStateCoroutine()
    {
        BecomeTangible();

        // Remain tangible as long as the player is inside the radius
        float timer = tangibleDuration;
        while (timer > 0f)
        {
            float dist = Vector2.Distance(player.position, transform.position);

            // If player moves out of radius, start countdown
            if (dist >= revealRadius)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                // Player still in range, reset the timer
                timer = tangibleDuration;
            }
            yield return null;
        }

        BecomeIntangible();
        tangibleCoroutine = null;
    }

    void BecomeTangible()
    {
        isIntangible = false;
        fader.FadeToAlpha(1f, 0.5f);
        col.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        // Play reveal VFX/SFX here
    }

    void BecomeIntangible()
    {
        if (isStunned) return;
        isIntangible = true;
        fader.FadeToAlpha(intangibleAlpha, tangibleDuration);
        col.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("IntangibleEnemy");
        // Play fade VFX/SFX here
    }

    void ShootAtPlayer()
    {
        if (player == null) return;
        Vector2 dir = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var bullet = Instantiate(projectilePrefab, transform.position, Quaternion.LookRotation(Vector3.forward, dir));

        var bb = bullet.GetComponent<SimpleEnemyBulletBehavior>();
        bb.Initialize(dir, angle);
    }

    void ShootSpreadAtPlayer()
    {
        if (player == null) return;
        Vector2 dir = (player.position - transform.position).normalized;
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float angleStep = spread / (numBullets - 1);

        for (int i = 0; i < numBullets; i++)
        {
            float angleOffset = -spread / 2 + angleStep * i; // Spreads bullets symmetrically
            float shootAngle = baseAngle + angleOffset;

            Vector2 shootDir = new Vector2(
                Mathf.Cos(shootAngle * Mathf.Deg2Rad),
                Mathf.Sin(shootAngle * Mathf.Deg2Rad)
            );

            var bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            var bb = bullet.GetComponent<SimpleEnemyBulletBehavior>();
            bb.Initialize(shootDir, shootAngle); // Pass the new direction and angle
        }
    }


    // Stunnable fns
    public void StunUntilStop()
    {
        StartCoroutine(StunCoroutine(tangibleDuration));
    }



    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        BecomeIntangible();
        yield return new WaitForSeconds(duration);
        isStunned = false;
        BecomeIntangible();
    }


    void OnDrawGizmosSelected()
    {
        // Draw the reveal radius in the editor when the object is selected
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); // Orange, semi-transparent
        Gizmos.DrawWireSphere(transform.position, revealRadius);
    }
}