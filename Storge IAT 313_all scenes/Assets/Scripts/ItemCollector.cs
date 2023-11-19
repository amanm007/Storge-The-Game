using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private Image itemImageUI;
    [SerializeField] private AudioSource collectSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("sword"))
        {
            Sprite collectedItemSprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
            itemImageUI.sprite = collectedItemSprite; // Set the UI Image to the collected item's sprite
            itemImageUI.enabled = true; // Enable the Image component if it was disabled
            collectSound.Play();

            Destroy(collision.gameObject); // Destroy the sword object
        }
    }
}
