using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class soundDetector : MonoBehaviour
{
    //  [SerializeField] private Image itemImageUI;
    public AudioClip gramophoneClip;
    public GameObject enemy;
    [SerializeField] private AudioSource collectSound;
    public Transform[] newPatrolTargets;



    void Start()
    {
        // Get the AudioSource component
        collectSound = GetComponent<AudioSource>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gramophone"))
        {
            if (!collectSound.isPlaying)
            {
                collectSound.clip = gramophoneClip;
                collectSound.Play();
            }

            // Destroy(collision.gameObject); // Destroy the sword object
            Patrol enemyPatrolScript = enemy.GetComponent<Patrol>();
            if (enemyPatrolScript != null)
            {
                enemyPatrolScript.UpdateTargets(newPatrolTargets);


            }
        }
    }
}