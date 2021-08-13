using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour {

	Text bestTimeLabel;
	Text currentTimeLabel;
	int currentLevel;

	float bestTime;
	float currentTime;

	bool timeSaved;

	// Use this for initialization
	void Start () {
		timeSaved = false;

		currentLevel = SceneManager.GetActiveScene().buildIndex;

		GameObject.Find("CurrentLevel").GetComponent<Text>().text = "Level " + currentLevel;
		bestTimeLabel = GameObject.Find ("BestTime").GetComponent<Text>();
		currentTimeLabel = GameObject.Find("CurrentTime").GetComponent<Text>();

		bestTime = PlayerPrefs.GetFloat("Time"+currentLevel);
		bestTimeLabel.text = StringFrom(bestTime);
	}
	
	// Update is called once per frame
	void Update () {
		if (!timeSaved) {
			currentTime += Time.deltaTime;
			if (PlayerMove.ui_on == 1) currentTimeLabel.text = StringFrom(currentTime);
		}
	}

	public void SaveTime() {
		timeSaved = true;
		if ((bestTime == 0f) || (currentTime < bestTime)) {
			bestTime = currentTime;
			PlayerPrefs.SetFloat("Time"+currentLevel, bestTime);
		}
		currentTime = 0f;
	}

	string StringFrom(float floattime) {
		int minutes = (int)floattime / 60;
		int seconds = (int)floattime % 60;
		float fraction = (floattime * 100) % 100;
		return string.Format ("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
	}
}
