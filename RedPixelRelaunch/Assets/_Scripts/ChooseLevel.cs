using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseLevel : MonoBehaviour {

	public Button[] buttons;
	public Sprite[] backgrounds;
    public Sprite[] sounds;
    public int maxPages;
	static int page;
    public static bool sounds_on = true;
	SpriteRenderer playlvl20;

    // Use this for initialization
    void Start () {
		PlayerPrefs.SetInt("Level1", 1);

		playlvl20 = GameObject.Find("playlvl20").GetComponent<SpriteRenderer>();

		if (page == 0)
			page = 1;
		//PlayerPrefs.DeleteAll();
		LoadButtons();

        AudioListener.volume = sounds_on ? 1.0f : 0.0f;

        if (sounds_on)
        {
            GameObject.Find("Sounds").GetComponent<Image>().sprite = sounds[0];
        }
        else
        {
            GameObject.Find("Sounds").GetComponent<Image>().sprite = sounds[1];
        }
    }


	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}

	public void ButtonPressed(int name) {
		if (name > 0) {
			SceneManager.LoadScene((page-1)*10+name);
		} else if (name == 0) {
			Next();
		} else {
			Previous();
		}
	}

	public void LoadButtons() {
		for (int index = 0; index < buttons.Length; index++) { // Loop through all 10 buttons on screen
			buttons[index].name = (((page-1)*10)+(index+1)).ToString(); // Change name
			Text txt = GameObject.Find(buttons[index].name+"/Text").GetComponent<Text>(); // Change text
			txt.text = buttons[index].name;

			if (PlayerPrefs.GetInt("Level"+(((page-1)*10)+(index+1))) == 1) {
				buttons[index].interactable = true;
			} else {
				buttons[index].interactable = false;
			}
		}

		GameObject.Find("background").GetComponent<SpriteRenderer>().sprite = backgrounds[page-1];


		GameObject.Find("Previous").GetComponent<Button>().interactable = false;
		GameObject.Find("Next").GetComponent<Button>().interactable = false;
		if (page > 1)
			GameObject.Find("Previous").GetComponent<Button>().interactable = true;
		if (page < maxPages)
			GameObject.Find("Next").GetComponent<Button>().interactable = true;

		// Open Level 20
		if (buttons[9].name == "20") {
			buttons[9].interactable = true;
		}

		//Close level 21 if 19 isn't open
		if (buttons[0].name == "21") {
			if (PlayerPrefs.GetInt("WonLevel19") != 1) {
				buttons[0].interactable = false;
			}
		}

		//Remove or add level 20 ad
		if ((page == 1) && (PlayerPrefs.GetInt("Level11") != 1) && (PlayerPrefs.GetInt("playedlvl20") != 1)) {
			playlvl20.enabled = true;
		} else {
			playlvl20.enabled = false;
		}
			
			
	}

	void Next() {
		page++;
		LoadButtons();
	}

	void Previous() {
		page--;
		LoadButtons();
	}

    public void ChangeSounds()
    {
        sounds_on = !sounds_on;
        AudioListener.volume = sounds_on ? 1.0f : 0.0f;
        if (sounds_on)
        {
            GameObject.Find("Sounds").GetComponent<Image>().sprite = sounds[0];
        }
        else
        {
            GameObject.Find("Sounds").GetComponent<Image>().sprite = sounds[1];
        }
    }
}
