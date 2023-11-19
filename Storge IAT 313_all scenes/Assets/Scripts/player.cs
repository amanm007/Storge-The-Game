using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    AudioManager audioManager;


    public float walkSpeed = 20.0f;
    private const float DiagonalMoveLimiter = 0.7f;
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;
    private float lastFootstepTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }

    void Update()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");


        if (horizontalMove != 0 && verticalMove != 0)
        {
            //PlayFootsetps();
            horizontalMove *= DiagonalMoveLimiter;
            verticalMove *= DiagonalMoveLimiter;
            
        }

        rb.velocity = new Vector2(horizontalMove * walkSpeed, verticalMove * walkSpeed);

        UpdateAnimationState(horizontalMove, verticalMove);
        // Check if the player is walking, and if so, play the footsteps sound
        if (Mathf.Abs(horizontalMove) > 0 || Mathf.Abs(verticalMove) > 0)
        {
            PlayFootsetps();
        }
    }

    public void PlayFootsetps()
    {
        float currentTime = Time.time;

        // Control the frequency of footstep sounds (e.g., play every 0.5 seconds)
        if (currentTime - lastFootstepTime >= 0.5f)
        {
            lastFootstepTime = currentTime;

            // Randomize the pitch within the specified range
            float randomPitch = Random.Range(minPitch, maxPitch);
            audioManager.PlaySFXWithPitch(audioManager.footsteps, randomPitch);
        }
    }

    private void UpdateAnimationState(float horizontalMove, float verticalMove)
    {
        bool isWalking = horizontalMove != 0 || verticalMove != 0;
        animator.SetBool("walking", isWalking);
        



        if (isWalking)
        {
            
            spriteRenderer.flipX = horizontalMove < 0;
        }
    }
}