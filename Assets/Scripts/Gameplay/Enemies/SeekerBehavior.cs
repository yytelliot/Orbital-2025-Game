using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SeekerBehavior : MonoBehaviour
{

    [Header("Sprite Info")]
    public float offset = 90;

    public float moveSpeed = 3f;

    private GameObject player;
    private Transform playerTransform;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("SeekerBehavior: Player Not Found in scene");
        }
        
    }

    void FixedUpdate()
    {
        if (playerTransform == null) return;

        Vector2 currentPos = rb.position;
        Vector2 targetPos = (Vector2)playerTransform.position;
        Vector2 direction = (targetPos - currentPos).normalized;

        Vector2 newPos = currentPos + direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

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
