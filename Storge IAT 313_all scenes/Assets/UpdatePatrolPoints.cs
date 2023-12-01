using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePatrolPoints : MonoBehaviour
{
    public GameObject npc; // Reference to the NPC GameObject
    public Transform[] newWaypoints; // Array of new waypoints

    private NPCPatrol npcPatrolScript;

    void Start()
    {
        // Get the NPCPatrol script from the NPC GameObject
        if (npc != null)
        {
            npcPatrolScript = npc.GetComponent<NPCPatrol>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player collided with the bookshelf
        if (collision.gameObject.CompareTag("Player") && npcPatrolScript != null)
        {
            // Update the waypoints in the NPCPatrol script
            npcPatrolScript.UpdateWaypoints(newWaypoints);
        }
    }
}