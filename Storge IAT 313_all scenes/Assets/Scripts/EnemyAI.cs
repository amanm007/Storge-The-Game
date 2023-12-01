using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 5f;
    public float attackRange = 2f; // Range within which the beast attacks
    public float alertAnimationDuration = 1f; // Duration of the alert animation
    public float attackCooldown = 2f; // Cooldown time for attacks

    private AIPath aiPath;
    private AIDestinationSetter destinationSetter;
    private Patrol patrolScript;
    private SpriteRenderer sp;
    private Animator anim;
    private enum State { Patrolling, Chasing, Attacking, RandomMoving }
    private State currentState;
    private float lastAttackTime;

    private void Start()
    {
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        patrolScript = GetComponent<Patrol>();
        sp = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();

        SetState(State.Patrolling);
        HandleSpriteFlipping();
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                if (PlayerInRange(chaseRange))
                {
                    StartCoroutine(StartChaseAfterDelay(alertAnimationDuration));
                }
                break;
            case State.Chasing:
                if (!PlayerInRange(chaseRange))
                {
                    SetState(State.Patrolling);
                }
                else if (PlayerInRange(attackRange) && Time.time > lastAttackTime + attackCooldown)
                {
                    SetState(State.Attacking);
                }
                break;
            case State.Attacking:
                if (!PlayerInRange(attackRange))
                {
                    SetState(State.Chasing);
                }
                break;
            case State.RandomMoving:
                // Implement random movement logic here
                break;
        }
        if (currentState == State.Patrolling)
        {
            UpdateSpriteDirectionBasedOnMovement();
        }

        HandleSpriteFlipping();
    }
    private void HandleSpriteFlipping()
    {
        if (currentState == State.Patrolling)
        {
            Vector2 currentPatrolPoint = patrolScript.GetCurrentPatrolPoint();
            FlipSpriteBasedOnDirection(currentPatrolPoint);
        }
        else if (currentState == State.Chasing)
        {
            // Flip sprite based on player's position during chasing
            FlipSpriteBasedOnPlayerPosition();
        }
        // Additional conditions for other states if needed
    }

    private void FlipSpriteBasedOnDirection(Vector2 targetPosition)
    {
        if (targetPosition.x > transform.position.x)
        {
            sp.flipX = false; // Target is to the right, face right
        }
        else if (targetPosition.x < transform.position.x)
        {
            sp.flipX = true; // Target is to the left, face left
        }
    }
    private void FlipSpriteBasedOnPlayerPosition()
    {
        if (player.position.x > transform.position.x)
        {
            sp.flipX = false; // Player is to the right, face right
        }
        else if (player.position.x < transform.position.x)
        {
            sp.flipX = true; // Player is to the left, face left
        }
    }
    private void UpdateSpriteDirectionBasedOnMovement()
    {
        if (aiPath.desiredVelocity.x > 0.01f)
        {
            sp.flipX = false; // Moving right, face right
        }
        else if (aiPath.desiredVelocity.x < -0.01f)
        {
            sp.flipX = true; // Moving left, face left
        }
    }
    private IEnumerator StartChaseAfterDelay(float delay)
    {
        anim.SetBool("is_alerted", true);
        aiPath.canMove = false; // Stop movement

        yield return new WaitForSeconds(delay);

        anim.SetBool("is_alerted", false);
        aiPath.canMove = true; // Resume movement
        SetState(State.Chasing);
    }

    private bool PlayerInRange(float range)
    {
        return Vector2.Distance(transform.position, player.position) <= range;
    }

    private void SetState(State newState)
    {
        currentState = newState;

        if (aiPath != null) aiPath.enabled = false;
        if (destinationSetter != null) destinationSetter.enabled = false;
        if (patrolScript != null) patrolScript.enabled = false;

        switch (newState)
        {
            case State.Patrolling:
                if (patrolScript != null) patrolScript.enabled = true;
                if (aiPath != null) aiPath.enabled = true;
                anim.SetBool("is_attack", false);
                break;
            case State.Chasing:
                if (destinationSetter != null)
                {
                    destinationSetter.target = player;
                    destinationSetter.enabled = true;
                }
                if (aiPath != null) aiPath.enabled = true;
                anim.SetBool("is_attack", false);
                break;
            case State.Attacking:
                anim.SetBool("is_attack", true);
                lastAttackTime = Time.time;
                break;
        }

        Debug.Log("Enemy state changed to: " + newState);
    }
}
