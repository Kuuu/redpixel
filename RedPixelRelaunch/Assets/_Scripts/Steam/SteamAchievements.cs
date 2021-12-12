using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamAchievements : MonoBehaviour
{
	public static SteamAchievements Instance = null;

	bool statsRecieved = false;

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
	}

    private void Start()
    {
    }

    private void RequestStats()
    {
		statsRecieved = SteamUserStats.RequestCurrentStats();
    }

	public void UnlockAchievement(string ach)
    {
		if (!SteamManager.Initialized)
        {
			return;
        }

		RequestStats();

		if (statsRecieved)
        {
			Debug.Log("Stats recieved");
			SteamUserStats.SetAchievement(ach);
			StoreStats();
			statsRecieved = false;
        } else
        {
			Debug.Log("Not recieved");
        }
    }

	private void StoreStats()
    {
		SteamUserStats.StoreStats();
    }

	private void ClearAchievement(string ach)
    {
		RequestStats();

		if (statsRecieved)
		{
			SteamUserStats.ClearAchievement(ach);
			StoreStats();
			statsRecieved = false;
		}
	}
}
