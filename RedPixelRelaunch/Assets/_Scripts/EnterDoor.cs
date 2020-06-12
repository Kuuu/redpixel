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
		if (PlayerMove.device_type == DeviceType.Handheld) {
			if (won && CrossPlatformInputManager.GetButtonDown ("Jump")) {
				int scene = SceneManager.GetActiveScene ().buildIndex + 1;
				SceneManager.LoadScene (scene, LoadSceneMode.Single);
			}
		} else {
			if (won && Input.GetButtonDown ("Vertical")) {
				int scene = SceneManager.GetActiveScene ().buildIndex + 1;
				SceneManager.LoadScene (scene, LoadSceneMode.Single);
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
		won = true;
		timeManager.SaveTime();

		PlayerPrefs.SetInt("Level"+(SceneManager.GetActiveScene().buildIndex+1), 1);
		PlayerPrefs.Save();
		niceone.canvasRenderer.SetAlpha(1.0f);
		hitenter.canvasRenderer.SetAlpha(1.0f);
		niceone.CrossFadeAlpha(0.0f, 1f, false);
	}
}
