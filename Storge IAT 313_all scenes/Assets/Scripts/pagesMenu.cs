using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pagesMenu : MonoBehaviour
{
    public List<Page> pages;
    public GameObject pagesContainer;
    private int activePage = 0;
    public bool activated = true;
    [Header("Spawn/despawn objects after opening book for first time")]
    public int checkingPage = 0;
    private bool pageViewed = false;
    public List<GameObject> toEnableOnClose;
    public List<GameObject> toDisableOnClose;
    private bool spawningTriggered = false;


    // Start is called before the first frame update
    void Start()
    {
        pagesContainer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && activated) //Toggle the menu on and off with Q
        {
            pagesContainer.SetActive(!pagesContainer.activeSelf);
            updateCurrentPage();

            if (pagesContainer.activeSelf)
            {
                Time.timeScale = 0; //pause the game
            }
            else
            {
                if (!spawningTriggered && pageViewed)
                {
                    // Enable/disable objects
                    foreach (GameObject obj in toEnableOnClose)
                    {
                        obj.SetActive(true);
                    }
                    foreach (GameObject obj in toDisableOnClose)
                    {
                        obj.SetActive(false);
                    }

                    spawningTriggered = true;
                }
                

                Time.timeScale = 1; //resume the game
                
            }
        }

        //Scroll between pages (only do it when the menu is active)
        if (pagesContainer.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                activePage++;
                updateCurrentPage();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                activePage--;
                updateCurrentPage();
            }
        }

        //Check is detected page has been viewed, to then activate objects if it has been
        if(activePage == checkingPage)
        {
            pageViewed = true;
        }
    }

    public void activatePage(int pageNum)
    {
        pages[pageNum].unlocked = true;
        activePage = pageNum;
    }

    private void updateCurrentPage()
    {
        //Loop around if value is too high

        if (activePage >= pages.Count)
        {
            activePage = 0;
        }

        // Loop around if value is below 0

        if (activePage < 0)
        {
            activePage = pages.Count - 1;

            while (!pages[activePage].unlocked)
            {
                activePage--;
                if (activePage < 0)
                {
                    activePage = 0;
                    break;
                }
            }
        }

        //Make sure the set page is currently unlocked; skip to the next if it is not; loop around to the first page if no others are unlocked; force page 0 to be unlocked if none are

        while (!pages[activePage].unlocked)
        {
            activePage++;
            if(activePage >= pages.Count)
            {
                activePage = 0;
                pages[0].unlocked = true;
            }
        }

        //Show the currently selected page, and hide all others.
        if (pages[activePage].unlocked)
        {
            foreach (Page i in pages)
            {
                i.pageObject.SetActive(false);
            }
            pages[activePage].pageObject.SetActive(true);
        }
    }

    public void setActive(bool condition)
    {
        activated = condition;
    }

    [System.Serializable]
    public class Page
    {
        public GameObject pageObject;
        public bool unlocked;
    }
}
