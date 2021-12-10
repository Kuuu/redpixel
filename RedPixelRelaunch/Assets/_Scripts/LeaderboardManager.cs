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
    public Text levelNumber;
    int currentPage = 0;
    int lastPage = 0;
    public Button prevPage;
    public Button nextPage;

    Dictionary<int, CSteamID> _ids;
    Dictionary<int, int> _scores;
    int _entryCount;

    //int currentPage;

    // Start is called before the first frame update
    void Start()
    {
        SteamLeaderboardUploader.Instance.GetScores(1);
        levelNumber.text = "1";
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

        if (level != 0 && level <= 50)
        {
            SteamLeaderboardUploader.Instance.GetScores(level);
            levelNumber.text = "" + level;
            ClearFields();
            HideButtons();
        }
    }

    public void ShowScores(Dictionary<int, CSteamID> ids, Dictionary<int, int> scores, int entryCount)
    {
        /*
        ids = new Dictionary<int, CSteamID>();
        scores = new Dictionary<int, int>();

        ids.Add(1, new CSteamID(22222));
        ids.Add(2, new CSteamID(222232));
        ids.Add(3, new CSteamID(222422));
        ids.Add(4, new CSteamID(222522));
        ids.Add(5, new CSteamID(222622));
        ids.Add(6, new CSteamID(227222));
        ids.Add(7, new CSteamID(228222));
        ids.Add(8, new CSteamID(229222));
        ids.Add(9, new CSteamID(222122));
        ids.Add(10, new CSteamID(223222));
        ids.Add(11, new CSteamID(222222));
        ids.Add(12, new CSteamID(242222));

        scores.Add(1, 4606);
        scores.Add(2, 4706);
        scores.Add(3, 4806);
        scores.Add(4, 4906);
        scores.Add(5, 5006);
        scores.Add(6, 5106);
        scores.Add(7, 5206);
        scores.Add(8, 5306);
        scores.Add(9, 5406);
        scores.Add(10, 5506);
        scores.Add(11, 5606);
        scores.Add(12, 5706);
        
        entryCount = 12;
        */

        lastPage = entryCount / 5;

        ShowButtons();
        BlockButtons();

        _ids = ids;
        _scores = scores;
        _entryCount = entryCount;
        currentPage = 0;

        ShowPage();

        
    }

    void ShowPage()
    {
        ClearFields();

        int lastIndex = 0;

        if (_entryCount - (currentPage * 5) > 5)
        {
            lastIndex = 5;
        }
        else
        {
            lastIndex = _entryCount - (currentPage * 5);
        }

        for (int i = 0; i < lastIndex; i++)
        {
            Debug.Log("Looking for rank #" + ((currentPage * 5) + i + 1));
            string name = SteamFriends.GetFriendPersonaName(_ids[(currentPage * 5) + i + 1]);
            string time = TimeManager.StringFrom(SteamLeaderboardUploader.Instance.IntToScore(_scores[(currentPage * 5) + i + 1]));

            ranks[i].text = "#" + ((currentPage * 5) + 1 + i);
            names[i].text = name;
            times[i].text = time;
        }
    }

    public void ClearFields()
    {
        for (int i = 0; i < 5; i++)
        {
            ranks[i].text = "";
            names[i].text = "";
            times[i].text = "";
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

    public void NextPage()
    {
        currentPage += 1;
        BlockButtons();
        ShowPage();
    }

    public void PreviousPage()
    {
        currentPage -= 1;
        BlockButtons();
        ShowPage();
    }

    void HideButtons()
    {
        prevPage.gameObject.SetActive(false);
        nextPage.gameObject.SetActive(false);
    }

    void ShowButtons()
    {
        prevPage.gameObject.SetActive(true);
        nextPage.gameObject.SetActive(true);
    }

    void BlockButtons()
    {
        if (currentPage == 0)
        {
            prevPage.interactable = false;
        } else
        {
            prevPage.interactable = true;
        }

        if (currentPage == lastPage)
        {
            nextPage.interactable = false;
        } else
        {
            nextPage.interactable = true;
        }
    }
}
