using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 input;
    private Collider2D hitbox;

    [Header("Interaction")]
    public LayerMask interactablesLayer;
    [SerializeField] private float interactRange = 0.2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        hitbox = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // Read input
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // Update animator
        if (input.sqrMagnitude > 0f)
        {
            animator.SetFloat("moveX", input.x);
            animator.SetFloat("moveY", input.y);
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        // 3) Check for interaction
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetBool("isInteracting", true);
            TryInteract();
        }
        else
        {
            animator.SetBool("isInteracting", false);
        }
    }

    private void FixedUpdate()
    {
        Vector2 v = input.normalized * moveSpeed;
        rb.velocity = v;
    }

    private void TryInteract()
    {

        Vector2 facingDir = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        if (facingDir.sqrMagnitude == 0f)
        {
            // if no input, set default (I think this can be changed to last facing direction stored in animator)
            facingDir = Vector2.down;
        }

        // Cast a small circle in the facing direction to find an interactable
        Vector2 interactPos = (Vector2)transform.position + hitbox.offset + facingDir.normalized * interactRange;
        Collider2D hit = Physics2D.OverlapCircle(interactPos, 0.1f, interactablesLayer);
        if (hit != null)
        {
            hit.GetComponent<Interactable>()?.Interact();
        }
    }

    // Debug: show the interaction hitbox in the editor (switch to editor after pressing interact to view)
    private void OnDrawGizmosSelected()
    {
        if (animator == null) return;
        Vector2 facingDir = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        if (facingDir.sqrMagnitude == 0f) facingDir = Vector2.down;
        Vector2 interactPos = (Vector2)transform.position + hitbox.offset + facingDir.normalized * interactRange;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactPos, 0.1f);
    }
}