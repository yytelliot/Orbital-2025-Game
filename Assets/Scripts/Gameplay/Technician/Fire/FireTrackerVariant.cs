using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrackerVariant : MonoBehaviour
{
    private Vector3 position;
    private System.Action onDestroyed;

    [SerializeField] private Animator animator;

    public void ExtinguishFireVariant()
    {
        animator.SetTrigger("Extinguish");
        StartCoroutine(DestroyAfterAnimation());
        
    }


    private IEnumerator DestroyAfterAnimation()
    {
        // Wait until the animation starts
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("FireExtinguish"))
            yield return null;

        // Wait for the animation duration
        float duration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duration);

        Destroy(gameObject);
        
    }
}
