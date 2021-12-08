using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{

	public static LanguageManager Instance = null;
	string currentLanguage = "en";
	public Sprite[] langButtons; // en, ru

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			Instance = this;
		}
		DontDestroyOnLoad(this.gameObject);

		if (PlayerPrefs.HasKey("language"))
		{
			SetLanguage(PlayerPrefs.GetString("language"));
		}
		else
		{
			PlayerPrefs.SetString("language", "en");
			PlayerPrefs.Save();
		}
	}

    public void SetLanguage(string lan)
    {
		currentLanguage = lan;
		PlayerPrefs.SetString("language", lan);
		UpdateButtonImage();
	}

	public void UpdateButtonImage()
    {
		if (currentLanguage == "ru")
		{
			GameObject.Find("LanguageButton").GetComponent<Image>().sprite = langButtons[1];
		}
		else if (currentLanguage == "en")
		{
			GameObject.Find("LanguageButton").GetComponent<Image>().sprite = langButtons[0];
		}
	}

	public string GetLanguage()
    {
		return currentLanguage;
    }

	public void ChangeLanguage()
    {
		if (currentLanguage == "en")
		{
			SetLanguage("ru");
			
		} else if (currentLanguage == "ru")
        {
			SetLanguage("en");
		}
    }

	// Update is called once per frame
	void Update()
    {
        
    }
}
