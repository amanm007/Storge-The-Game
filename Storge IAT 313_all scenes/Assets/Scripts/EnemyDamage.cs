using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int BeastDamage = 2;
    public int Pool_TendrilDamage = 1;
    private Coroutine damageCoroutine;
    public PlayerHealth playerHealth;
    AudioManager audioManager;
    private float vol = 0.3f;


    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
             playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(BeastDamage);
                
                audioManager.PlaySFXWithLowSound(audioManager.demon_attack, vol);

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(ApplyPoolDamage(collision.gameObject));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator ApplyPoolDamage(GameObject player)
    {
        while (true)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(Pool_TendrilDamage);
            }
            yield return new WaitForSeconds(2f); // Wait for 2 seconds before applying damage again
        }
    }
}
