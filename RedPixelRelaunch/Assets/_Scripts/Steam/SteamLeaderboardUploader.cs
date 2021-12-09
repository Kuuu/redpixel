using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamLeaderboardUploader : MonoBehaviour
{
	public static SteamLeaderboardUploader Instance = null;

	private CallResult<LeaderboardFindResult_t> m_LeaderboardFindResult;
	private SteamLeaderboard_t _board;
	private int _score;
	private string _boardName;

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
		return intScore / 1000;
    }

	public void UploadScore(int level, float score)
    {
		_score = ScoreToInt(score);
		_boardName = "level" + level;

		m_LeaderboardFindResult = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardFindResult);

		SteamAPICall_t handle = SteamUserStats.FindLeaderboard(_boardName);
		m_LeaderboardFindResult.Set(handle);

	}

	private void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool bIOFailure) 
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
}
