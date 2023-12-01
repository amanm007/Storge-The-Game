using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    public float healthBoost = 2f; // Amount of health to increase

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null && playerHealth.health < playerHealth.maxHealth)
            {
                
                playerHealth.IncreaseHealth(healthBoost);

                
                Destroy(gameObject);
            }
        }
    }
}
