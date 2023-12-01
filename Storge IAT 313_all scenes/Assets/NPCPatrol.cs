using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoints
    public float patrolSpeed = 2f; // Speed of patrol
    public Animator animator; // Reference to the Animator component

    private int waypointIndex = 0;
    private Transform targetWaypoint;
    private SpriteRenderer spriteRenderer;
    private bool isAttacking = false;
    private PlayerHealth playerHealth;
    public int attackDamage = 1;
    public float patrolRadius = 1f; // Radius around the player for patrolling
    public GameObject player; // Reference to the player GameObject
    private Vector3 patrolTarget;
    private enum PatrolMode { AroundPlayer, Waypoints }
    private PatrolMode currentPatrolMode = PatrolMode.AroundPlayer;
    private Coroutine attackCoroutine;



    private void Start()
    {
        if (waypoints.Length > 0)
        {
            targetWaypoint = waypoints[0];
        }
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetNewPatrolTarget();
    }

    private void Update()
    {
        if(!isAttacking) // Only patrol if not attacking
        {
            switch (currentPatrolMode)
            {
                case PatrolMode.AroundPlayer:
                    PatrolAroundPlayer();
                    break;
                case PatrolMode.Waypoints:
                    Patrol();
                    break;
            }
        }
    }

    private void Patrol()
    {
        if (targetWaypoint != null)
        {
            MoveTowards(targetWaypoint.position);
            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                waypointIndex = (waypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[waypointIndex];
            }
        }
    }

    private void MoveTowards(Vector3 target)
    {
        bool isMovingRight = target.x > transform.position.x;
        spriteRenderer.flipX = !isMovingRight;
        transform.position = Vector3.MoveTowards(transform.position, target, patrolSpeed * Time.deltaTime);
        animator.SetBool("isWalking", true);
    }
    private void PatrolAroundPlayer()
    {
        if (Vector3.Distance(transform.position, patrolTarget) < 0.1f)
        {
            SetNewPatrolTarget();
        }
        MoveTowards(patrolTarget);
    }

    private void SetNewPatrolTarget()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized * patrolRadius;
        patrolTarget = player.transform.position + new Vector3(randomDirection.x, randomDirection.y, 0);
    }

    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth = other.GetComponent<PlayerHealth>(); // Get the PlayerHealth component from the player
            if (playerHealth != null)
            {
                isAttacking = true;
                animator.SetBool("isWalking", false);
                animator.SetTrigger("isAttacking");
                playerHealth.TakeDamage(attackDamage); // Deduct health from the player
            }
        }
    }
    */
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (attackCoroutine == null)
            {
                // Trigger the attack immediately
                isAttacking = true;
                animator.SetBool("isWalking", false);
                animator.SetTrigger("isAttacking");

                attackCoroutine = StartCoroutine(AttackPlayer(collision.collider));
            }
        }
    }
    /*
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left NPC's range - Stopping Attack");
            isAttacking = false;
            animator.ResetTrigger("isAttacking");
            animator.SetBool("isWalking", true);
        }
    }
    */

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
            isAttacking = false;
            animator.ResetTrigger("isAttacking");
            animator.SetBool("isWalking", true);
        }
    }
    private IEnumerator AttackPlayer(Collider2D playerCollider)
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);


            if (playerCollider != null && playerCollider.gameObject.activeInHierarchy)
            {
                PlayerHealth playerHealth = playerCollider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    // Apply damage
                    playerHealth.TakeDamage(attackDamage);
                    animator.SetTrigger("isAttacking");
                }
            }
            else
            {
                break;
            }
        }
    }
    public void UpdateWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
        waypointIndex = 0;
        if (waypoints.Length > 0)
        {
            targetWaypoint = waypoints[0];
            currentPatrolMode = PatrolMode.Waypoints; // Switch to waypoint patrol mode
        }
    }
}