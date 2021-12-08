using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextObject : MonoBehaviour
{
    public Sprite[] textImages;
    public bool isUI;


    // Start is called before the first frame update
    void Start()
    {
        UpdateImage();
    }

    public void UpdateImage()
    {
        string lang = LanguageManager.Instance.GetLanguage();

        if (lang == "en")
        {
            if (isUI)
            {
                GetComponent<Image>().sprite = textImages[0];
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = textImages[0];
            }
        }
        else if (lang == "ru")
        {
            if (isUI)
            {
                GetComponent<Image>().sprite = textImages[1];
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = textImages[1];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
