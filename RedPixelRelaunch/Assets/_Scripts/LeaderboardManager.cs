using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Steamworks;

public class LeaderboardManager : MonoBehaviour
{
    public Text[] ranks;
    public Text[] names;
    public Text[] times;
    public Text userRank;

    //int currentPage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void DownloadScores()
    {
        string levelString = GameObject.Find("InputField").GetComponent<InputField>().text;
        int level = 0;
        try
        {
            level = int.Parse(levelString);
        } catch (FormatException e)
        {

        }

        if (level != 0)
        {
            SteamLeaderboardUploader.Instance.GetScores(level);
        }
    }

    public void ShowScores(Dictionary<int, CSteamID> ids, Dictionary<int, int> scores, int entryCount)
    {
        int lastIndex = 0;

        if (entryCount > 5)
        {
            lastIndex = 5;
        } else
        {
            lastIndex = entryCount;
        }
        
        for (int i = 0; i < lastIndex; i++)
        {
            string name = SteamFriends.GetFriendPersonaName(ids[i + 1]);
            string time = TimeManager.StringFrom(SteamLeaderboardUploader.Instance.IntToScore(scores[i + 1]));

            names[i].text = name;
            times[i].text = time;
        }
    }

    public void ShowUserRank(int rank)
    {
        userRank.text = "" + rank;
    }

    public void HideUserRank()
    {
        userRank.text = "None";
    }
}
