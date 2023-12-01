using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackAnimatiion : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D boxCollider;

    void Start()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger the attack animation
            anim.SetBool("is_triggered", true);
        }

    }
    public void TransitionToWiggle()
    {
        anim.SetTrigger("start_wiggle");
        // Optionally adjust the collider for the wiggle animation if needed
    }

    // Call this from an Animation Event at the end of the attack animation
    public void ReturnToIdle()
    {
        anim.SetBool("is_triggered", false);
    }
}
