using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class soundDetector : MonoBehaviour
{
    //  [SerializeField] private Image itemImageUI;
    AudioManager audioManager;
    public GameObject enemy;
  //  [SerializeField] private AudioSource collectSound;
    public Transform[] newPatrolTargets;



    void Start()
    {
        // Get the AudioSource component
        //collectSound = GetComponent<AudioSource>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gramophone"))
        {

            //collectSound.clip = gramophoneClip;
            audioManager.PlaySFXAtLocation(audioManager.gramohphone, collision.gameObject.transform.position);


            // Destroy(collision.gameObject); // Destroy the sword object
            Patrol enemyPatrolScript = enemy.GetComponent<Patrol>();
            if (enemyPatrolScript != null)
            {
                enemyPatrolScript.UpdateTargets(newPatrolTargets);


            }
        }

        if (collision.gameObject.CompareTag("Clock"))
        {

            //collectSound.clip = gramophoneClip;
            audioManager.PlaySFXAtLocation(audioManager.grandfatherclock, collision.gameObject.transform.position);


            // Destroy(collision.gameObject); // Destroy the sword object
            Patrol enemyPatrolScript = enemy.GetComponent<Patrol>();
            if (enemyPatrolScript != null)
            {
                enemyPatrolScript.UpdateTargets(newPatrolTargets);


            }
        }

    }
}