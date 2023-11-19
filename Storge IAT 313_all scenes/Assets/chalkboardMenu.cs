using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class chalkboardMenu : MonoBehaviour
{
    public string correctResponse;
    public TMP_InputField input;
    public List<GameObject> ActivateOnIncorrectResponse;
    public List<GameObject> DeactivateOnIncorrectResponse;
    public List<GameObject> ActivateOnCorrectResponse;
    public List<GameObject> DeactivateOnCorrectResponse;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        Time.timeScale = 0; //pause the game
    }

    private void OnDisable()
    {
        Time.timeScale = 1; //resume the game
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(input.text.ToUpper() == correctResponse.ToUpper())
            {
                foreach(GameObject obj in ActivateOnCorrectResponse)
                {
                    obj.SetActive(true);
                }
                foreach (GameObject obj in DeactivateOnCorrectResponse)
                {
                    obj.SetActive(false);
                }
            }
            else
            {
                foreach (GameObject obj in ActivateOnIncorrectResponse)
                {
                    obj.SetActive(true);
                }
                foreach (GameObject obj in DeactivateOnIncorrectResponse)
                {
                    obj.SetActive(false);
                }
            }

            this.gameObject.SetActive(false);
        }
    }
}
