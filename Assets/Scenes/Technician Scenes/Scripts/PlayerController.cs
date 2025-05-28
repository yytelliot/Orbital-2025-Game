using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public bool isMoving;
    public Vector2 input;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>(); //Start the animation upon awake
    }


    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal"); // Get keyboard input
            input.y = Input.GetAxisRaw("Vertical");



            if (input.x != 0) // No diagonal movement
            {
                input.y = 0;
            }


            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x); // Give animator input
                animator.SetFloat("moveY", input.y);
                
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                StartCoroutine(Move(targetPos));
            }
        }
    }

    IEnumerator Move(Vector3 targetPos) // Character Movement
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }

    

}
    
