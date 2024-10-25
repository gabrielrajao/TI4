using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject Recursos;
    Dictionary<string, TextMeshProUGUI> uiTexts;
    Dictionary<string, int> resourceValues;
    List<string> resourceNames;

    // Start is called before the first frame update
    void Start()
    {
        setResources();
    }

    // Update is called once per frame
    void Update()
    {
       updateUI();
    }

    void setResources(){
        //Variables
        this.uiTexts = new Dictionary<string, TextMeshProUGUI>();
        resourceValues = new Dictionary<string, int>();
        resourceNames = new List<string>(); 
        TextMeshProUGUI[] uiTexts = Recursos.GetComponentsInChildren<TextMeshProUGUI>();

        //Operations
        foreach (TextMeshProUGUI uiText in uiTexts){
            string resourceName = uiText.gameObject.name;
        
            this.uiTexts.Add(resourceName, uiText);
            resourceValues.Add(resourceName, 0);
            resourceNames.Add(resourceName);
        }
    }

    void updateUI(){
        //Operations
        foreach(string resourceName in resourceNames){
            TextMeshProUGUI uiText = uiTexts[resourceName];
            uiText.text = "" + resourceValues[resourceName];
        }
    }
}
