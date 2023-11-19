using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;



public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 5f;

    private AIPath aiPath;
    private AIDestinationSetter destinationSetter;
    private Patrol patrolScript;
    private SpriteRenderer sp;
    private enum State { Patrolling, Chasing, RandomMoving }
    private State currentState;

    private void Start()
    {
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        patrolScript = GetComponent<Patrol>();
        sp = GetComponent<SpriteRenderer>();

        // Set the initial state to patrolling
        SetState(State.Patrolling);

        // Debug the initial distance to the player
        Debug.Log("Initial distance to player: " + Vector2.Distance(transform.position, player.position));
    }

    private void Update()
    {
        // Debug the distance to the player every frame
        Debug.Log("Current distance to player: " + Vector2.Distance(transform.position, player.position));

        switch (currentState)
        {
            case State.Patrolling:
                // If the player is within the chase range, switch to chasing
                if (PlayerInRange())
                {
                    SetState(State.Chasing);
                }
                break;
            case State.Chasing:
                // If the player is no longer within the chase range, switch back to patrolling
                if (!PlayerInRange())
                {
                    SetState(State.Patrolling);
                }
                break;
            case State.RandomMoving:
                // Implement random movement logic here
                break;
        }
        if (aiPath.velocity.x > 0.01f)
        {
            // If your sprite faces left by default, set this to true
            sp.flipX = true;
        }
        else if (aiPath.velocity.x < -0.01f)
        {
            // If your sprite faces left by default, set this to false
            sp.flipX = false;
        }
    }

    private bool PlayerInRange()
    {
        // Check if the player is within the chase range
        return Vector2.Distance(transform.position, player.position) <= chaseRange;
    }

    private void SetState(State newState)
    {
        currentState = newState;

        // Disable all scripts to reset the state
        if (aiPath != null) aiPath.enabled = false;
        if (destinationSetter != null) destinationSetter.enabled = false;
        if (patrolScript != null) patrolScript.enabled = false;

        // Enable the necessary scripts for the new state
        switch (newState)
        {
            case State.Patrolling:
                if (patrolScript != null) patrolScript.enabled = true;
                if (aiPath != null) aiPath.enabled = true;
                break;
            case State.Chasing:
                if (destinationSetter != null)
                {
                    destinationSetter.target = player;
                    destinationSetter.enabled = true;
                }
                if (aiPath != null) aiPath.enabled = true;
                break;
            case State.RandomMoving:
                // Enable and configure components for random movement
                break;
        }

        Debug.Log("Enemy state changed to: " + newState);
    }
}