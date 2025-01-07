using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json; //Playfab uses JSON requests to retrieve and display data from its servers

public class PlayFabManager : MonoBehaviour
{
    public GameObject name1Window, leaderboard1Window, name2Window, leaderboard2Window;
    public GameManager gameManager;

    public GameObject rowPrefab;
    public Transform leaderboard1, leaderboard2, rowsParent;

    void Start()
    {
        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
        //Attempt to login, calling the OnSuccess or OnError function depending on the result of the request
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successfully logged in/created account");
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/creating account"); //Used for testing purposes during development
    }

    public void SubmitName(string nameInput) 
    //Called when the user attempts to update their username on the results screen at the end of a level
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameInput,
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
        //Attempts to assign the user's input to the DisplayName associated with this device
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
        //Called when the username associated with this device has been changed successfully
    {
        //Checks which level this is, sends the score to the corresponding leaderboard and displays the leaderboard window
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            SendLeaderboard(1, gameManager.GetComponent<GameManager>().currentScore);
            name1Window.SetActive(false);
            leaderboard1Window.SetActive(true);
        }
        else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            SendLeaderboard(2, gameManager.GetComponent<GameManager>().currentScore);
            name2Window.SetActive(false);
            leaderboard2Window.SetActive(true);
        }
        Invoke("GetCurrentLeaderboard", 1f);
    }

    public void SendLeaderboard(int levelNum, int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Level " + levelNum + " Leaderboard",
                    Value = score                   
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
        //Attempts to send the user's score to the leaderboard
        //The OnError function is called if the device loses its network connection at any point
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Leaderboard entry sent successfully");
    }

    public void GetCurrentLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = SceneManager.GetActiveScene().name + " Leaderboard",
            StartPosition = 0,
            MaxResultsCount = 5
        };

        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
        //Attempts to retrieve the data from the leaderboard for this level
    }

    public void GetLeaderboard(int levelNum)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Level " + levelNum + " Leaderboard",
            StartPosition = 0,
            MaxResultsCount = 5
        };

        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
        //Attempts to retrieve the data from any leaderboard (used in the start menu when neither level is loaded yet)
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        if (name1Window)
        {
            if (name1Window.activeInHierarchy)
            {
                rowsParent = leaderboard1;
            }
        }

        if (name2Window)
        {
            if (name2Window.activeInHierarchy)
            {
                rowsParent = leaderboard2;
            }
        }
        //Find which leaderboardwindow is being requested and assign that to rowsParent

        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
            //Remove any existing row objects in the leaderboard before filling it with newly retrieved values
        }

        foreach (var item in result.Leaderboard) //For each entry in this leaderboard
        {
            if (item.Position < 5) //If the entry is in the top 5 of all entries
            {
                GameObject newGo = Instantiate(rowPrefab, rowsParent);
                TMP_Text[] texts = newGo.GetComponentsInChildren<TMP_Text>();
                texts[0].text = (item.Position + 1).ToString();
                texts[1].text = item.DisplayName;
                texts[2].text = item.StatValue.ToString();
                //Instantiate a row object displaying the position, username and score of each of the top 5 entries
            }
        }
    }
}