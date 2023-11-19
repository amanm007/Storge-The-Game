using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameUI : MonoBehaviour
{
    public TextMeshProUGUI TextField;
    public GameObject Hugo;
    public GameObject Alcina;
    public GameObject Emil;
    public GameObject Raem;
    public GameObject Iola;
    public GameObject Narakth;
    public GameObject TrueRaem;
    public GameObject TrueIola;
    public GameObject TrueNarakth;
    private GameObject currGameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void setName(string name){
        TextField.text = name;
        if(currGameObject){
            currGameObject.SetActive(false);
        }
        switch (name) {
            case "Hugo?": {
                TextField.text = "???";
                Hugo.SetActive(true);
                currGameObject=Hugo;
                break;
            }
            case "Hugo": {
                Hugo.SetActive(true);
                currGameObject=Hugo;
                break;
            }
            case "Alcina?": {
                TextField.text = "???";
                Alcina.SetActive(true);
                currGameObject=Alcina;
                break;
            }
            case "Alcina": {
                Alcina.SetActive(true);
                currGameObject=Alcina;
                break;
            }
            case "Emil?": {
                TextField.text = "???";
                Emil.SetActive(true);
                currGameObject=Emil;
                break;
            }
            case "Emil": {
                Emil.SetActive(true);
                currGameObject=Emil;
                break;
            }
            case "Raem?": {
                TextField.text = "???";
                Raem.SetActive(true);
                currGameObject=Raem;
                break;
            }
            case "Raem": {
                TrueRaem.SetActive(true);
                currGameObject=Raem;
                break;
            }
            case "Iola?": {
                TextField.text = "???";
                Iola.SetActive(true);
                currGameObject=Iola;
                break;
            }
            case "Iola": {
                TrueIola.SetActive(true);
                currGameObject=Iola;
                break;
            }
            case "Narakth?": {
                TextField.text = "???";
                Narakth.SetActive(true);
                currGameObject=Narakth;
                break;
            }
            case "Narakth": {
                TrueNarakth.SetActive(true);
                currGameObject=Narakth;
                break;
            }
        }
    }
}
