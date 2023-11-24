using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth = 10;
    //public GameObject otherObject;
    private Animator animator;

    // public HealthBar healthBar;
    //  public Image[] healthpoints;
    public Image healthbar;
    float lerpSpeed;
    AudioManager audioManager;




    void Start()
    {
        health = maxHealth;
        // healthBar.SetMaxHealth(maxHealth);
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        animator = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        lerpSpeed = 3f * Time.deltaTime;
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        HealthBarFiller();
        

    }

    void HealthBarFiller()
    {
        healthbar.fillAmount = Mathf.Lerp(healthbar.fillAmount, (float)health / maxHealth, lerpSpeed);

       // for (int i=0; i < healthpoints.Length; i++)
       // {
        //    healthpoints[i].enabled = !DisplayHealthPoint(health, i);

       // }

        
    }
    bool DisplayHealthPoint(float health, float pointNumber )
    {
        return ((pointNumber * 3) >= health);
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        animator.SetTrigger("HurtTrigger");
        audioManager.PlaySFX(audioManager.hurt);


        health = Mathf.Clamp(health, 0, maxHealth);
       // healthBar.SetHealth(health);
        if (health<=0)
        {

            Die();

        }
    }
    private void Die()
    {
        if (healthbar != null)
        {
            healthbar.fillAmount = 0;
        }
        // Destroy the other object if it exists
        Destroy(gameObject);
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject); // Destroy the child object
        }

    }
}