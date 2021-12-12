using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class EnterDoor : MonoBehaviour {

	Text niceone;
	Text hitenter;
	Text coins;
	bool won = false;
	bool nextLevelUnlocked = false;

	int remaining_coins;
	DoorOpen door_open;

	TimeManager timeManager;

	// Use this for initialization
	void Start () {
		niceone = GameObject.Find("NiceOne").GetComponent<Text>();
		hitenter = GameObject.Find("ToContinue").GetComponent<Text>();
		coins = GameObject.Find("Coins").GetComponent<Text>();
		niceone.canvasRenderer.SetAlpha(0.0f);
		hitenter.canvasRenderer.SetAlpha(0.0f);
		remaining_coins = GameObject.FindGameObjectsWithTag("Coin").Length;
		coins.text = ""+remaining_coins;
		door_open = GameObject.FindGameObjectWithTag("Door").GetComponent<DoorOpen>();
		timeManager = GetComponent<TimeManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (won && Input.GetButtonDown ("Vertical")) {

			if (nextLevelUnlocked)
            {
				int scene = SceneManager.GetActiveScene().buildIndex + 1;
				if (scene < 51)
                {
					SceneManager.LoadScene(scene, LoadSceneMode.Single);
				}
			}
		}
	}

	void UpdateCoins() {
		if (remaining_coins == 0) {
			door_open.OpenYourself();
		}

		coins.text = ""+remaining_coins;

	}

	void OnTriggerEnter2D(Collider2D other) {

		if (other.tag == "Coin") {
			Sounds.Instance.PlayCoin();
			Destroy(other.gameObject);
			remaining_coins--;
			UpdateCoins();
		}

		if (other.tag == "Door" && door_open.allow_enter) {
			Win();
		}
	}

	void Win() {
		if (won)
        {
			return;
        }

		won = true;
		Debug.Log("Won!");
		timeManager.SaveTime();

		int currentLevel = SceneManager.GetActiveScene().buildIndex;

		if (currentLevel == 19)
        {
			//PlayerPrefs.SetInt("WonLevel19", 1);
			GameData.Instance.Set("WonLevel19", 1);
		}

		//if (currentLevel == 20 && PlayerPrefs.GetInt("WonLevel19") != 1)
		if (currentLevel == 20 && GameData.Instance.Get("WonLevel19") != 1)
        {
			nextLevelUnlocked = false;
			hitenter.text = "Now finish other levels";
        } else
        {
			nextLevelUnlocked = true;
        }

		//PlayerPrefs.SetInt("Level"+(currentLevel + 1)+"unlocked", 1);
		//PlayerPrefs.Save();
		GameData.Instance.Set("Level" + (currentLevel + 1) + "unlocked", 1);
		niceone.canvasRenderer.SetAlpha(1.0f);
		hitenter.canvasRenderer.SetAlpha(1.0f);
		niceone.CrossFadeAlpha(0.0f, 1f, false);
	}
}
