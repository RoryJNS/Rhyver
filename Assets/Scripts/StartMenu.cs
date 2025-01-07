using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json; //JSON requests are used to retrieve and display online leaderboards
using UnityEngine.UI;
using TMPro;

public class StartMenu : MonoBehaviour
{
    public GameObject mainMenu, levelSelectMenu, level1Menu, level1MenuWindow, level2Menu, level2MenuWindow, optionsMenu;
    public TMPro.TMP_Text level1PersonalBestText, level1HighestComboText, level2PersonalBestText, level2HighestComboText;
    public Image level1Stars, level2Stars;
    public Sprite stars0, stars1, stars2, stars3, goldStars;
    public TMPro.TMP_Text instaFailButtonText, slowerSongButtonText, fullscreenButtonText;
    public AudioMixer audioMixer;
    public Slider masterVolumeSlider;
    public GameObject levelMenuWindow, leaderboard1Window, leaderboard2Window;
    public GameObject rowPrefab; //The template for an instantiated row on a leaderboard
    public Transform rowsParent; //The empty table to fill with leaderboard rows
    public TrackImporter trackImporter; //The track importer script for importing custom main menu music

    void Start()
    {
        audioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
        //Set the volume of the mixer to the saved value of the volume slider

        level1PersonalBestText.text = "PERSONAL BEST: " + PlayerPrefs.GetInt("Level1PersonalBest", 0);
        level1HighestComboText.text = "HIGHEST COMBO: " + PlayerPrefs.GetInt("Level1HighestCombo", 0);

        if (PlayerPrefs.GetInt("Level1PersonalBest") == 156450)
            level1Stars.sprite = goldStars;
        else if (PlayerPrefs.GetInt("Level1PersonalBest") >= 155000)
            level1Stars.sprite = stars3;
        else if (PlayerPrefs.GetInt("Level1PersonalBest") >= 150000)
            level1Stars.sprite = stars2;
        else if (PlayerPrefs.GetInt("Level1PersonalBest") >= 140000)
            level1Stars.sprite = stars1;
        else
            level1Stars.sprite = stars0;

        level2PersonalBestText.text = "PERSONAL BEST: " + PlayerPrefs.GetInt("Level2PersonalBest", 0);
        level2HighestComboText.text = "HIGHEST COMBO: " + PlayerPrefs.GetInt("Level2HighestCombo", 0);

        if (PlayerPrefs.GetInt("Level2PersonalBest") == 209400)
            level2Stars.sprite = goldStars;
        else if (PlayerPrefs.GetInt("Level2PersonalBest") >= 160000)
            level2Stars.sprite = stars3;
        else if (PlayerPrefs.GetInt("Level2PersonalBest") >= 110000)
            level2Stars.sprite = stars2;
        else if (PlayerPrefs.GetInt("Level2PersonalBest") >= 90000)
            level2Stars.sprite = stars1;
        else
            level2Stars.sprite = stars0;

        //Update the text and image elements of the level select menus based on stored personal bests
        //3 gold stars will be displayed if the player achieved a perfect score
    }

    public void LoadMainMenu()
    {
        levelSelectMenu.SetActive(false);
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void LoadLevelSelectMenu()
    {
        level1Menu.SetActive(false);
        level2Menu.SetActive(false);
        mainMenu.SetActive(false);
        levelSelectMenu.SetActive(true);
    }

    public void LoadLevel1Menu()
    {
        levelSelectMenu.SetActive(false);
        level1MenuWindow.SetActive(true);
        leaderboard1Window.SetActive(false);
        level1Menu.SetActive(true);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadSceneAsync("Level 1");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadSceneAsync("Level 2");
    }

    public void LoadLevel2Menu()
    {
        levelSelectMenu.SetActive(false);
        level2MenuWindow.SetActive(true);
        leaderboard2Window.SetActive(false);
        level2Menu.SetActive(true);
    }

    public void LoadOptionsMenu()
    {
        mainMenu.SetActive(false);

        if (PlayerPrefs.GetString("instaFail") == "true")
            instaFailButtonText.text = "ON";
        else
            instaFailButtonText.text = "OFF";

        if (PlayerPrefs.GetString("slowerSong") == "true")
            slowerSongButtonText.text = "ON";
        else
            slowerSongButtonText.text = "OFF";

        if (PlayerPrefs.GetString("fullscreen") == "true")
            fullscreenButtonText.text = "ON";
        else
            fullscreenButtonText.text = "OFF";

        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");

        optionsMenu.SetActive(true);
    }

    public void LoadLeaderboard1()
    {
        level1MenuWindow.SetActive(false);
        leaderboard1Window.SetActive(true);
    }

    public void LoadLeaderboard2()
    {
        level2MenuWindow.SetActive(false);
        leaderboard2Window.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit(); //Function provided by Unity which exits the application
    }
}
