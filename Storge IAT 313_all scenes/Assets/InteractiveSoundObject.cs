using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class InteractiveSoundObject : MonoBehaviour
{
  //  public AudioClip soundEffect; // Assign the specific sound for this object in the inspector
    public Transform[] newPatrolTargets; // Assign new patrol targets in the inspector
    public GameObject enemy; // Assign the enemy that should change patrol targets

    private AudioManager audioManager;
    public AudioClip soundEffect;
    private bool soundPlayed = false;

    void Start()
    {
        // Find the AudioManager in the scene
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Play the sound effect at this object's location
            if (audioManager != null && !soundPlayed)
            {
                audioManager.PlaySFXAtLocation(soundEffect, transform.position);
                soundPlayed = true;
            }

            // Update enemy patrol targets if set
            if (enemy != null && newPatrolTargets != null && newPatrolTargets.Length > 0)
            {
                Patrol enemyPatrolScript = enemy.GetComponent<Patrol>();
                if (enemyPatrolScript != null)
                {
                    enemyPatrolScript.UpdateTargets(newPatrolTargets);
                }
            }
        }
    }
}
