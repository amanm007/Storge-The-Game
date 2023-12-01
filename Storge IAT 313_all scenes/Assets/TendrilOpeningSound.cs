using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TendrilOpeningSound : MonoBehaviour
{
    //  [SerializeField] private Image itemImageUI;
    AudioManager audioManager;
    
    



    void Start()
    {
        // Get the AudioSource component
        //collectSound = GetComponent<AudioSource>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            //collectSound.clip = gramophoneClip;
            audioManager.PlaySFXAtLocationwithLowerVol(audioManager.tendril, transform.position);




        }

    }
}
