using System;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamLeaderboardUploader : MonoBehaviour
{
	public static SteamLeaderboardUploader Instance = null;

	private CallResult<LeaderboardFindResult_t> m_LeaderboardFindResult;
	private CallResult<LeaderboardScoresDownloaded_t> m_LeaderboardScoresDownloadedResult;
	private CallResult<LeaderboardScoresDownloaded_t> m_UserScoreDownloadedResult;
	private SteamLeaderboard_t _board;
	private int _score;
	private string _boardName;

	public Dictionary<int, CSteamID> leaderboardIDs = new Dictionary<int, CSteamID>();
	public Dictionary<int, int> leaderboardScores = new Dictionary<int, int>();

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

	public int ScoreToInt(float score)
    {
		return (int)(score * 1000);
    }

	public float IntToScore(int intScore)
    {
		return (float)intScore / 1000;
    }

	public void UploadScore(int level, float score)
    {
		_score = ScoreToInt(score);
		_boardName = "level" + level;

		m_LeaderboardFindResult = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardFindResultToUpload);

		SteamAPICall_t handle = SteamUserStats.FindLeaderboard(_boardName);
		m_LeaderboardFindResult.Set(handle);

	}

	void ClearDictionaries()
    {
		leaderboardIDs = new Dictionary<int, CSteamID>();
	     leaderboardScores = new Dictionary<int, int>();
	}

	public void GetScores(int level)
    {
		_boardName = "level" + level;

		Debug.Log("Getting scores for boardname: " + _boardName);

		m_LeaderboardFindResult = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardFindResultToFetch);

		SteamAPICall_t handle = SteamUserStats.FindLeaderboard(_boardName);
		m_LeaderboardFindResult.Set(handle);
	}

	private void OnLeaderboardFindResultToUpload(LeaderboardFindResult_t pCallback, bool bIOFailure) 
    {
		if (pCallback.m_bLeaderboardFound != 1 || bIOFailure)
        {
			Debug.Log("There was an error when finding the leaderboard");
			Debug.Log(pCallback.m_hSteamLeaderboard);
        } else
        {
			_board = pCallback.m_hSteamLeaderboard;

			var method = ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest;
			SteamUserStats.UploadLeaderboardScore(_board, method, _score, new int[] { }, 0);
        }
    }

	private void OnLeaderboardFindResultToFetch(LeaderboardFindResult_t pCallback, bool bIOFailure)
	{
		if (pCallback.m_bLeaderboardFound != 1 || bIOFailure)
		{
			Debug.Log("There was an error when finding the leaderboard");
			Debug.Log(pCallback.m_hSteamLeaderboard);
		}
		else
		{
			// Download all scores
			_board = pCallback.m_hSteamLeaderboard;
			m_LeaderboardScoresDownloadedResult = CallResult<LeaderboardScoresDownloaded_t>.Create(OnScoresDownloaded);
			SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntries(_board, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, -999, 999);
			m_LeaderboardScoresDownloadedResult.Set(handle);


			// Download player score
			CSteamID[] Users = { SteamUser.GetSteamID() };
			m_UserScoreDownloadedResult = CallResult<LeaderboardScoresDownloaded_t>.Create(OnUserScoreDownloaded);
			SteamAPICall_t handle2 = SteamUserStats.DownloadLeaderboardEntriesForUsers(_board, Users, Users.Length);
			m_UserScoreDownloadedResult.Set(handle2);
		}
	}

	private void OnUserScoreDownloaded(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure)
	{
		SteamLeaderboardEntries_t entries2 = pCallback.m_hSteamLeaderboardEntries;
		Int32[] details = { };

		if (pCallback.m_cEntryCount > 0) // user has a score on this level
        {
			LeaderboardEntry_t entry;
			SteamUserStats.GetDownloadedLeaderboardEntry(entries2, 0, out entry, details, 0);
			GameObject.Find("LeaderboardManager").GetComponent<LeaderboardManager>().ShowUserRank(entry.m_nGlobalRank);
		} else
        {
			GameObject.Find("LeaderboardManager").GetComponent<LeaderboardManager>().HideUserRank();

		}
	}

	private void OnScoresDownloaded(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure)
	{
		SteamLeaderboardEntries_t entries = pCallback.m_hSteamLeaderboardEntries;
		Int32[] details = { };

		ClearDictionaries();

		for (int i = 0; i < pCallback.m_cEntryCount; i++)
        {
			LeaderboardEntry_t entry;
			SteamUserStats.GetDownloadedLeaderboardEntry(entries, i, out entry, details, 0);
			leaderboardIDs.Add(entry.m_nGlobalRank, entry.m_steamIDUser);
			leaderboardScores.Add(entry.m_nGlobalRank, entry.m_nScore);
		}

		

		GameObject.Find("LeaderboardManager").GetComponent<LeaderboardManager>().ShowScores(leaderboardIDs, leaderboardScores, pCallback.m_cEntryCount);
    }
}
