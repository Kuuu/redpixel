using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextTranslator : MonoBehaviour
{
    public string en = "";
    public string ru = "";

    // Start is called before the first frame update
    void Start()
    {
        string lang = LanguageManager.Instance.GetLanguage();

        if (lang == "en")
        {
            GetComponent<Text>().text = en;
        } else if (lang == "ru")
        {
            GetComponent<Text>().text = ru;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
