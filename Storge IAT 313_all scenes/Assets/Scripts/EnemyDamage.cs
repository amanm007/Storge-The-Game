using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int BeastDamage = 2;
    public int PoolDamage = 1;
    public int TendrilDamage = 1; // New variable for tendril damage
    private Animator anim;
    private Coroutine poolDamageCoroutine;
    private Coroutine tendrilDamageCoroutine; // Separate coroutine for tendril damage
    public PlayerHealth playerHealth;
    AudioManager audioManager;
    private float vol = 0.1f;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        anim = GetComponentInChildren<Animator>();
    }

    public void ApplyAttackDamage()
    {
        Debug.Log("ApplyAttackDamage called");
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(BeastDamage);
            audioManager.PlaySFXWithLowSound(audioManager.demon_attack, vol);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (poolDamageCoroutine == null)
            {
                poolDamageCoroutine = StartCoroutine(ApplyPoolDamage(collision.gameObject));
            }
            if (tendrilDamageCoroutine == null)
            {
                tendrilDamageCoroutine = StartCoroutine(ApplyTendrilDamage(collision.gameObject));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (poolDamageCoroutine != null)
            {
                StopCoroutine(poolDamageCoroutine);
                poolDamageCoroutine = null;
            }
            if (tendrilDamageCoroutine != null)
            {
                StopCoroutine(tendrilDamageCoroutine);
                tendrilDamageCoroutine = null;
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
                playerHealth.TakeDamage(PoolDamage);
            }
            yield return new WaitForSeconds(2f); // Wait for 2 seconds before applying damage again
        }
    }

    private IEnumerator ApplyTendrilDamage(GameObject player)
    {
        while (true)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("attack_tendril") || stateInfo.IsName("wiggle"))
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(TendrilDamage);
                }
            }
            yield return new WaitForSeconds(2f); // Wait for 2 seconds before applying damage again
        }
    }
}
