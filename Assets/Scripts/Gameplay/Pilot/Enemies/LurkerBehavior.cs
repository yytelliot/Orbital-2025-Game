using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class LurkerBehavior : MonoBehaviour, IIntangible
{
    public bool isIntangible { get; private set; }
    public float revealRadius = 5f;
    public float tangibleDuration = 3f;
    public float shootInterval = 2f;
    public GameObject projectilePrefab;

    private Transform player;
    private SpriteRenderer sr;
    private Collider2D col;
    private bool isTangible = false;
    private float tangibleTimer = 0f;
    private float shootTimer = 0f;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        float dist = Vector2.Distance(player.position, transform.position);

        if (!isTangible && dist < revealRadius)
        {
            BecomeTangible();
        }
        else if (isTangible)
        {
            tangibleTimer -= Time.deltaTime;
            if (tangibleTimer <= 0f && dist > revealRadius * 0.9f)
            {
                BecomeIntangible();
            }
        }

        // Shooting logic (always active)
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            ShootAtPlayer();
            shootTimer = shootInterval;
        }
    }

    void BecomeTangible()
    {
        isTangible = true;
        tangibleTimer = tangibleDuration;
        sr.color = new Color(1, 1, 1, 1); // Fully visible
        col.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        // Maybe play a reveal VFX or SFX here!
    }

    void BecomeIntangible()
    {
        isTangible = false;
        sr.color = new Color(1, 1, 1, 0.2f); // Semi-invisible
        col.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("IntangibleEnemy");
        // Maybe play a fade-out effect here!
    }

    void ShootAtPlayer()
    {
        if (player == null) return;
        Vector2 dir = (player.position - transform.position).normalized;
        Instantiate(projectilePrefab, transform.position, Quaternion.LookRotation(Vector3.forward, dir));
        // Customize projectile logic as needed
    }



     void OnDrawGizmosSelected()
    {
        // Draw the reveal radius in the editor when the object is selected
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); // Orange, semi-transparent
        Gizmos.DrawWireSphere(transform.position, revealRadius);
    }
}