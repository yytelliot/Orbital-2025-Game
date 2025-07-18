using System.Collections;
using System.Collections.Generic;
using Game.Events;
using Unity.Mathematics;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.EditorTools;

#endif
using UnityEngine;

public class LurkerBehavior : MonoBehaviour, IIntangible, IStunnable
{
    public float revealRadius = 5f;
    public float proximityRevealTime = 2f;
    public float aggroRadius = 100f;
    public float tangibleDuration = 3f;
    public bool isIntangible { get; private set; } = true;
    public bool isStunned { get; private set; } = false;

    public float despawnDistance = 200f;
    public GameObject projectilePrefab;

    [Header("Projectile Pattern")]
    [Tooltip("Time between each shot in seconds")]
    public float shootInterval = 3f;
    [Tooltip("Bullet lifetime in seconds")]
    public float bulletLifetime = 5f;
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
    private float tangibleTimer = 0f;
    private float stunTimer = 0f;
    private float revealTimer = 0f;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sr = GetComponent<SpriteRenderer>();
        fader = GetComponent<SpriteFader>();
        col = GetComponent<Collider2D>();
        fader.SetAlpha(intangibleAlpha);
        if (isIntangible)
        {
            SetIntangible();
        }
    }

    void Update()
    {
        float dist = Vector2.Distance(player.position, transform.position);

        // DESPAWNING
        if (player != null && dist >= despawnDistance)
        {
            Destroy(gameObject);
        }

        // TIMER UPDATES
        if (tangibleTimer > 0f)
            tangibleTimer -= Time.deltaTime;
        if (stunTimer > 0f)
            stunTimer -= Time.deltaTime;

        // TANGIBLE/STUN HANDLER
        bool shouldBeTangible = (tangibleTimer > 0f) || (stunTimer > 0f);

        // Update isStunned flag
        isStunned = (stunTimer > 0f);

        // Update isIntangible flag & visuals
        if (shouldBeTangible && isIntangible)
        {
            SetTangible();
        }
        else if (!shouldBeTangible && !isIntangible)
        {
            SetIntangible();
        }

        // Reveal if player is close
        if (isIntangible && dist < revealRadius)
        {
            if (revealTimer <= 0f)
            {
                tangibleTimer = Mathf.Max(tangibleTimer, tangibleDuration);
            }
            else
            {
                revealTimer -= Time.deltaTime;
            }
        }
        else
        {
            revealTimer = proximityRevealTime;
        }

        // Shooting
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f && stunTimer <= 0f && dist < aggroRadius)
        {
            ShootSpreadAtPlayer();
            shootTimer = shootInterval;
        }
    }

    public void Reveal(float time)
    {
        // Update timer to the longer of the 2
        tangibleTimer = Mathf.Max(tangibleTimer, time);
    }
    public void OnScanLurker(Component sender, object data)
    {
        if (data is ScannerRevealPayload payload)
        {
            float dist = Vector2.Distance(transform.position, payload.scannerPosition);
            if (dist <= payload.scannerRadius)
            {
                Stun(payload.scannerStrength * 2);
                Reveal(payload.scannerStrength * 3);
            }
        }
    }


    void SetTangible()
    {
        isIntangible = false;
        fader.FadeToAlpha(1f, 0.5f);
        col.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        // Play reveal VFX/SFX here
    }


    void SetIntangible()
    {
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
        bb.Initialize(dir, angle, bulletLifetime);
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
            bb.Initialize(shootDir, shootAngle, bulletLifetime); // Pass the new direction and angle
        }
    }


    // Stunnable fns
    public void StunUntilStop()
    {
        Stun(tangibleDuration);
    }



    public void Stun(float duration)
    {
        stunTimer = Mathf.Max(stunTimer, duration);
    }


    void OnDrawGizmosSelected()
    {
        // Draw the reveal radius in the editor when the object is selected
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); // Orange, semi-transparent
        Gizmos.DrawWireSphere(transform.position, revealRadius);
    }
}