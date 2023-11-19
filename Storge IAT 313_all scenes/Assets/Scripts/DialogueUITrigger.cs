using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUITrigger : MonoBehaviour
{
    public DialogueUI dialogueScript;
    private bool playerDetected;
    public bool active = false;
    public bool triggerAutomatically = false;
    private bool cooldownActive = false;
    private float cooldownTimer;

    //Detect trigger with player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If we triggerd the player enable playerdeteced and show indicator
        if(collision.tag == "Player")
        {
            playerDetected = true;
            dialogueScript.ToggleIndicator(playerDetected);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //If we lost trigger  with the player disable playerdeteced and hide indicator
        if (collision.tag == "Player")
        {
            playerDetected = false;
            dialogueScript.ToggleIndicator(playerDetected);
            //dialogueScript.EndDialogue();
        }
    }

    //While detected if we interact start the dialogue
    private void Update()
    {
        if(playerDetected && !active)
        {
            if (Input.GetKeyDown(KeyCode.E) || triggerAutomatically)
            {
                dialogueScript.StartDialogue();
                active = true;
            }
        }

        if (cooldownActive)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                cooldownActive = false;
                active = false;
                cooldownTimer = 0.1f;
            }
        }
    }

    public void ResetTrigger()
    {
        cooldownActive = true;
        cooldownTimer = 0.1f;
    }
}
