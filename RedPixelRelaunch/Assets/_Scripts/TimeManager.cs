using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour {

	Text bestTimeLabel;
	Text currentTimeLabel;
	int currentLevel;

	float bestTime;
	float currentTime = 0;

	bool timeSaved;

	Dictionary<int, float> speedrunTimes;

	// Use this for initialization
	void Start () {

		FillSpeedrunDictionary();

		timeSaved = false;

		currentLevel = SceneManager.GetActiveScene().buildIndex;

		if (LanguageManager.Instance.GetLanguage() == "en")
        {
			GameObject.Find("CurrentLevel").GetComponent<Text>().text = "Level " + currentLevel;
		} else if (LanguageManager.Instance.GetLanguage() == "ru")
		{
			GameObject.Find("CurrentLevel").GetComponent<Text>().text = "Уровень " + currentLevel;
		}
		


		bestTimeLabel = GameObject.Find ("BestTime").GetComponent<Text>();
		currentTimeLabel = GameObject.Find("CurrentTime").GetComponent<Text>();

		bestTime = GameData.Instance.GetTime("Time"+currentLevel);
		//Debug.Log(bestTime);
		bestTimeLabel.text = StringFrom(bestTime);
	}
	
	// Update is called once per frame
	void Update () {
		if (!timeSaved) {
			currentTime += Time.deltaTime;
			currentTimeLabel.text = StringFrom(currentTime);
		}
	}

	public void SaveTime() {
		timeSaved = true;
		Debug.Log(currentTime);
		if ((bestTime == 0f) || (currentTime < bestTime)) {
			bestTime = currentTime;
			Debug.Log("Saving Time: " + bestTime);
			GameData.Instance.SetTime("Time"+currentLevel, bestTime);
		}
		if (SteamManager.Initialized)
		{
			SteamLeaderboardUploader.Instance.UploadScore(currentLevel, bestTime);
		}

		UnlockAchievements();

		currentTime = 0f;
	}

	void UnlockAchievements()
    {
		string prefix = "SPEEDRUNNER_";

		if (bestTime <= speedrunTimes[currentLevel])
        {
			Debug.Log("Unlocking speedrun");
			SteamAchievements.Instance.UnlockAchievement(prefix + currentLevel);
        } else
        {
			Debug.Log("Needed " + speedrunTimes[currentLevel] + " for achievement");
        }

		if (currentLevel == 50)
        {
			SteamAchievements.Instance.UnlockAchievement("VICTORY");
		}
    }

	void FillSpeedrunDictionary()
    {
		speedrunTimes = new Dictionary<int, float>();

		speedrunTimes.Add(1, 2.5f);
		speedrunTimes.Add(2, 15f);
		speedrunTimes.Add(3, 4.5f);
		speedrunTimes.Add(4, 12f);
		speedrunTimes.Add(5, 14f);
		speedrunTimes.Add(6, 17f);
		speedrunTimes.Add(7, 23f);
		speedrunTimes.Add(8, 12f);
		speedrunTimes.Add(9, 14f);
		speedrunTimes.Add(10, 42f);

		speedrunTimes.Add(11, 15f);
		speedrunTimes.Add(12, 20f);
		speedrunTimes.Add(13, 7f);
		speedrunTimes.Add(14, 15f);
		speedrunTimes.Add(15, 17f);
		speedrunTimes.Add(16, 20f);
		speedrunTimes.Add(17, 17f);
		speedrunTimes.Add(18, 23f);
		speedrunTimes.Add(19, 24f);
		speedrunTimes.Add(20, 66f);

		speedrunTimes.Add(21, 8f);
		speedrunTimes.Add(22, 26f);
		speedrunTimes.Add(23, 25f);
		speedrunTimes.Add(24, 15f);
		speedrunTimes.Add(25, 12f);
		speedrunTimes.Add(26, 7f);
		speedrunTimes.Add(27, 11f);
		speedrunTimes.Add(28, 24f);
		speedrunTimes.Add(29, 35f);
		speedrunTimes.Add(30, 39f);

		speedrunTimes.Add(31, 11f);
		speedrunTimes.Add(32, 26f);
		speedrunTimes.Add(33, 9f);
		speedrunTimes.Add(34, 11f);
		speedrunTimes.Add(35, 15f);
		speedrunTimes.Add(36, 14f);
		speedrunTimes.Add(37, 23f);
		speedrunTimes.Add(38, 24f);
		speedrunTimes.Add(39, 38f);
		speedrunTimes.Add(40, 61f);

		speedrunTimes.Add(41, 7f);
		speedrunTimes.Add(42, 5f);
		speedrunTimes.Add(43, 15f);
		speedrunTimes.Add(44, 10f);
		speedrunTimes.Add(45, 12f);
		speedrunTimes.Add(46, 9f);
		speedrunTimes.Add(47, 38);
		speedrunTimes.Add(48, 65f);
		speedrunTimes.Add(49, 60f);
		speedrunTimes.Add(50, 80f);
	}

	public static string StringFrom(float floattime) {
		int minutes = (int)floattime / 60;
		int seconds = (int)floattime % 60;
		float fraction = (floattime * 100) % 100;
		return string.Format ("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
	}
}
