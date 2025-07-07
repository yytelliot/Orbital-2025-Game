using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SeekerBehavior : MonoBehaviour, IStunnable
{

    [Header("Sprite Info")]
    public float offset = 90;

    public float moveSpeed = 3f;

    [Tooltip("When stunned, can only move once the velocity is lower than stopThreshold")]
    public float stopThreshold = 0.2f;

    private GameObject player;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isStunned = false;
    private ShipProperties shipProperties;

    public void Stun(float time) => StartCoroutine(StunCoroutine(time));
    public void StunUntilStop() => StartCoroutine(StunUntilStopCoroutine());

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        Vector2 currentPos = transform.position;
        Vector2 targetPos = (Vector2)playerTransform.position;
        Vector2 direction = (targetPos - currentPos).normalized;

        float angleDeg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angleDeg - offset);



    }
}
