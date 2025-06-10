using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private bool isMoving;
    private Vector2 input;
    private Animator animator;

    //private bool interacting;



    public LayerMask solidObjectsLayer;
    public LayerMask interactablesLayer;

    [SerializeField] private float colliderAdjustment = 0.2f;

    private void Awake()
    {
        animator = GetComponent<Animator>(); //Start the animation upon awake
    }

    private void FixedUpdate()
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

                if (IsWalkable(targetPos) && !(AnimatorIsPlaying("InteractLeft") || AnimatorIsPlaying("InteractRight")))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }
    }


    private void Update()
    {
        

        //animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            //interacting = true;
            animator.SetBool("isInteracting", true);
            Interact();

        }
        else
        {
            //interacting = false;
            animator.SetBool("isInteracting", false);
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

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, colliderAdjustment, solidObjectsLayer | interactablesLayer) != null)
        {
            return false;
        }

        return true;
    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, colliderAdjustment, interactablesLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    bool AnimatorIsPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length >
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    bool AnimatorIsPlaying(string stateName)
    {
        return AnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
    
