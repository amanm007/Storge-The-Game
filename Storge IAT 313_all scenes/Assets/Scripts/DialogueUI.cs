using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    //Fields
    [Header("UI Components")]
    //Window
    public GameObject window;
    //Indicator
    public GameObject indicator;
    public GameObject name0;
    public GameObject name1;
    public Image portrait0;
    public Image portrait0fadeout;
    public Image portrait1;
    public Image portrait1fadeout;
    //Text component
    public TextMeshProUGUI dialogueText;
    public pagesMenu pMenu; //for giving the player pages on interaction


    //Dialogues list
    [Header("Dialogue Content")]
    public List<DialogueList> dialogues;
    //The objects that are diabled or enabled following the interaction
    public List<GameObject> ObjectsToDisable;
    public List<GameObject> ObjectsToEnable;
    public List<int> journalEntriesToGive;
    // If the dialogue should change after interaction...
    public bool changeDialogueAfterFirstInteraction = false;
    //Dialogues list
    public List<DialogueList> alternateDialogues;


    [Header("Functionality")]
    //Writing speed
    public float writingSpeed = 1;
    //Index on dialogue
    private int index;
    //Character index
    private int charIndex;
    //Started boolean
    public bool started;
    //Wait for next boolean
    private bool waitForNext;
    //So we know to make her portrait/name on the left side of the screen
    public string mainCharactersName = "June";

    private void Awake()
    {
        ToggleIndicator(false);
        ToggleWindow(false);
    }

    private void ToggleWindow(bool show)
    {
        window.SetActive(show);
    }
    public void ToggleIndicator(bool show)
    {
        indicator.SetActive(show);
    }

    //Start Dialogue
    public void StartDialogue()
    {
        Debug.Log("Start Dialogue");

        if (started)
            return;

        //Boolean to indicate that we have started
        started = true;
        //Show the window
        ToggleWindow(true);
        //hide the indicator
        ToggleIndicator(false);
        //Start with first dialogue
        GetDialogue(0);
    }

    private void GetDialogue(int i)
    {
        Debug.Log("Get Dialogue");

        if(portrait1 != null && portrait0 != null)
        {
            //Get the left portrait
            if (dialogues[i].portraitLeft != null)
            {
                portrait0.sprite = dialogues[i].portraitLeft;
                portrait0.gameObject.SetActive(true);
            }
            else
            {
                portrait0.gameObject.SetActive(false);
            }

            //Get the right portrait
            if (dialogues[i].portraitRight != null)
            {
                portrait1.sprite = dialogues[i].portraitRight;
                portrait1.gameObject.SetActive(true);
            }
            else
            {
                portrait1.gameObject.SetActive(false);
            }
        }

        //start index at zero
        if (dialogues[i].name == mainCharactersName)
        {
            name0.SetActive(true);
            name1.SetActive(false);
            TextMeshProUGUI NameField = name0.GetComponentInChildren<TextMeshProUGUI>();
            if (NameField != null)
            {
                NameField.text = dialogues[i].name;
            }

            if (portrait0fadeout != null && portrait1fadeout != null)
            {
                //fade-out non-talking member
                portrait0fadeout.gameObject.SetActive(false);
                portrait1fadeout.gameObject.SetActive(true);
            }
        }

        else
        {
            name0.SetActive(false);
            name1.SetActive(true);
            TextMeshProUGUI NameField = name1.GetComponentInChildren<TextMeshProUGUI>();
            if (NameField != null)
            {
                NameField.text = dialogues[i].name;
            }

            if (portrait0fadeout != null && portrait1fadeout != null)
            {
                //fade-out non-talking member
                portrait0fadeout.gameObject.SetActive(true);
                portrait1fadeout.gameObject.SetActive(false);
            }

        }

        //If there is an animation attached to this dialogue segement, play it
        if(dialogues[i].animatedObject != null && dialogues[i].animationName != null)
        {
            dialogues[i].animatedObject.Play(dialogues[i].animationName);
        }
        
        index = i;
        //Reset the character index
        charIndex = 0;
        //clear the dialogue component text
        dialogueText.text = string.Empty;
        //Start writing
        StartCoroutine(Writing());
    }

    //End Dialogue
    public void EndDialogue()
    {
        Debug.Log("End Dialogue");

        //Stared is disabled
        started = false;
        //Disable wait for next as well
        waitForNext = false;
        //Stop all Ienumerators
        StopAllCoroutines();
        //Hide the window
        ToggleWindow(false);

        //Re-enable the trigger if the user wants to interact again
        DialogueUITrigger trigger = GetComponentInParent<DialogueUITrigger>();
        if(trigger != null) { trigger.ResetTrigger(); }

        foreach (GameObject obj in ObjectsToEnable)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in ObjectsToDisable)
        {
            obj.SetActive(false);
        }

        if(journalEntriesToGive != null && journalEntriesToGive.Count > 0 && pMenu != null)
        {
            pMenu.setActive(true);
            foreach (int i in journalEntriesToGive)
            {
                pMenu.activatePage(i);
            }
        }

        index = 0;

    }

    //Disable current dialogue and enable next dialogue
    public void SwitchToNext()
    {
        Debug.Log("Switch Next");
        dialogues = alternateDialogues;
    }
    
    //Writing logic
    IEnumerator Writing()
    {
        yield return new WaitForSeconds(writingSpeed);

        string currentDialogue = dialogues[index].text;
        //Write the character
        dialogueText.text += currentDialogue[charIndex];
        //increase the character index 
        charIndex++;
        //Make sure you have reached the end of the sentence
        if(charIndex < currentDialogue.Length)
        {
            //Wait x seconds 
            yield return new WaitForSeconds(writingSpeed);
            //Restart the same process
            StartCoroutine(Writing());
        }
        else
        {
            //End this sentence and wait for the next one
            waitForNext = true;
        }        
    }

    private void Update()
    {
        if (!started)
            return;

        if(waitForNext && Input.GetKeyDown(KeyCode.E))
        {
            waitForNext = false;
            index++;

            //Check if we are in the scope fo dialogues List
            if(index < dialogues.Count)
            {
                //If so fetch the next dialogue
                GetDialogue(index);
            }
            else
            {
                //If not end the dialogue process
                //ToggleIndicator(true);
                EndDialogue();
                if(changeDialogueAfterFirstInteraction && alternateDialogues != null)
                {
                    SwitchToNext();
                }
            }            
        }
    }


    [System.Serializable]
    public class DialogueList
    {
        public string text;
        public string name;
        public Animator animatedObject; //For example, it a bookshelf fell down in this moment, you would assign the bookshelf here
        public string animationName; // And you would assing the falling animation here
        public Sprite portraitLeft;
        public Sprite portraitRight;

    }
}
