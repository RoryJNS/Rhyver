using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsScreen : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject resultsScreenBackground, resultsScreen1, resultsScreen2, resultsScreen3, nameWindow, leaderboardWindow;
    public PlayFabManager playFabManager;
    public NoteSpawner noteSpawner;
    public Image stars; //An image of a group of stars to indicate player performance
    public Sprite stars0, stars1, stars2, stars3, goldStars; //The different images for each number of stars
    public TMPro.TMP_Text perfectHitsText, goodHitsText, okayHitsText, missedHitsText, accuracyText, finalScoreText, finalScoreText2, highestComboText;

    private int finalScore;
    public int[] scoreThresholds;
    public float totalNotes, missedHits;

    public void InitialiseResultsScreen()
    {
        finalScore = gameManager.GetComponent<GameManager>().currentScore;
        totalNotes = noteSpawner.greenNotes.Length + noteSpawner.redNotes.Length + noteSpawner.blueNotes.Length;
        missedHits = gameManager.GetComponent<GameManager>().missedHits;
        //Finds these attributes from the GameManager class and assign them to three local variables

        perfectHitsText.text = "PERFECT HITS: " + gameManager.GetComponent<GameManager>().perfectHits;
        goodHitsText.text = "GOOD HITS: " + gameManager.GetComponent<GameManager>().goodHits;
        okayHitsText.text = "OKAY HITS: " + gameManager.GetComponent<GameManager>().okayHits;
        missedHitsText.text = "MISSED HITS: " + missedHits;
        accuracyText.text = "ACCURACY: " + Mathf.Floor(((totalNotes - missedHits) / totalNotes) * 100) + "%";
        finalScoreText.text = finalScore.ToString();
        finalScoreText2.text = finalScore.ToString();
        highestComboText.text = "HIGHEST COMBO: " + gameManager.GetComponent<GameManager>().currentHighestCombo;
        resultsScreenBackground.SetActive(true);

        if (finalScore == scoreThresholds[0])
        {
            stars.sprite = goldStars;
        }
        else if (finalScore >= scoreThresholds[1])
        {
            stars.sprite = stars3;
        }
        else if (finalScore >= scoreThresholds[2])
        {
            stars.sprite = stars2;
        }
        else if (finalScore >= scoreThresholds[3])
        {
            stars.sprite = stars1;
        }
        else
        {
            stars.sprite = stars0;
        }
        //Updates the star image depending on the players score
        //The elements of the scoreThresholds array have been assigned in the editor for each level
        //This means this script can be reused for both levels

        OpenResultsScreen1();
    }

    //These methods allow the user the switch between results screens using the arrow buttons on screen
    public void OpenResultsScreen1()
    {
        resultsScreen2.SetActive(false);
        resultsScreen1.SetActive(true);
    }

    public void OpenResultsScreen2()
    {
        resultsScreen1.SetActive(false);
        resultsScreen3.SetActive(false);
        resultsScreen2.SetActive(true);
    }

    public void OpenResultsScreen3()
    {
        resultsScreen2.SetActive(false);
        resultsScreen3.SetActive(true);
        if (PlayerPrefs.GetString("slowerSong") == "true")
        {
            nameWindow.SetActive(false);
            playFabManager.GetCurrentLeaderboard(); //Retrieves and displays the leaderboard for this level
            leaderboardWindow.SetActive(true);
            //The current score will not be saved to the online leaderboard for this level if the slower song option is turned on
        }
    }
}
