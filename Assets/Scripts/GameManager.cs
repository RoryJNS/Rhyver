using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float songBpm; //Beats per minute
    private float secPerBeat; //Number of seconds for each song beat
    private float songPositionInSeconds; //Current song position, in seconds
    public float songPositionInBeats; //Current song position, in beats
    private float offset; //Delay before the music started playing

    public AudioSource music; //An AudioSource attached to this GameObject that will play the music
    public AudioSource missedSoundEffect; //A sound effect to play when the player misses a note
    public CameraShake cameraShake; //The script responsible for shaking the camera when the player misses a note

    private bool levelHasStarted = false, levelHasEnded = false;

    public GameObject startMessageScreen, scoreAndCombo, resultsScreenBackground, levelFailedScreen;
    public ResultsScreen resultsScreen; //The script responsible for initialising and navigating the end screen

    public TMPro.TMP_Text levelFailedScoreText; 
    //Text for when the player misses a note and fails the level, only seen when the instant fail option is turned on

    public int currentScore = 0, currentHighestCombo = 0; //Used for tracking personal bests
    private int currentMultiplier, currentCombo = 0, multiplierTracker = 0; 

    public int[] multiplierThresholds; //Stores the required current combo to achieve a higher multiplier
    public int missedHits, okayHits, goodHits, perfectHits; //Used to calculate player accuracy at the end of the level

    public Text scoreText, comboText; //The score and combo UI elements

    void Start()
    {
        instance = this; //Allows the game manager to refer to itself
        scoreText.text = "0";
        currentMultiplier = 1;
        AudioListener.pause = false; //Unpauses the audio system (may have been paused before level was loaded)
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused == true) //Checks the static GameIsPaused attribute of the PauseMenuManager script
        {
            music.Pause();
        }
        else
        {
            music.UnPause();
        }

        if (levelHasStarted == false)
        {
            if (Input.anyKeyDown) //And if any key is pressed...
            {
                startMessageScreen.SetActive(false); //Disable the screen asking the user to press any key to start
                scoreAndCombo.SetActive(true); //Enable the UI text elements for current score and combo
                secPerBeat = 60f / songBpm; //Calculate the number of seconds per beat
                offset = (float)AudioSettings.dspTime; //Record the time when the music starts
                music.Play(); //And start playing the music
                levelHasStarted = true;
            }
        }

        else if (levelHasStarted == true && music.isPlaying == true && PauseMenu.GameIsPaused == false && levelHasEnded == false)
        //If the music is playing and the game is unpaused...
        {
            songPositionInSeconds = (float)(AudioSettings.dspTime - offset); //Calculate how many seconds since the song started
            songPositionInBeats = (songPositionInSeconds / secPerBeat) + 1; //Calculate how many beats since the song started
        }

        else if (levelHasStarted == true && music.isPlaying == false && PauseMenu.GameIsPaused == false && levelHasEnded == false)
        //If the music has finished playing and the level has ended...
        {
            //Check which level this is and whether any personal bests have been broken
            //PlayerPrefs is a class provided by Unity which stores this data between play sessions
            if (SceneManager.GetActiveScene().name == "Level 1")
            {
                if (currentScore > PlayerPrefs.GetInt("Level1PersonalBest", 0))
                {
                    PlayerPrefs.SetInt("Level1PersonalBest", currentScore);
                }
                if (currentHighestCombo > PlayerPrefs.GetInt("Level1HighestCombo", 0))
                {
                    PlayerPrefs.SetInt("Level1HighestCombo", currentHighestCombo);
                }
            }

            else if (SceneManager.GetActiveScene().name == "Level 2")
            {
                if (currentScore > PlayerPrefs.GetInt("Level2PersonalBest", 0))
                {
                    PlayerPrefs.SetInt("Level2PersonalBest", currentScore);
                }
                if (currentHighestCombo > PlayerPrefs.GetInt("Level2HighestCombo", 0))
                {
                    PlayerPrefs.SetInt("Level2HighestCombo", currentHighestCombo);
                }
            }

            resultsScreen.InitialiseResultsScreen();
            //This will set the values of the text elements of the results screen and enable it

            levelHasEnded = true; //Prevent the results screen from being initialised every frame
        }
    }

    public void NoteHit()
    {
        currentCombo += 1;
        if (currentCombo > currentHighestCombo)
        {
            currentHighestCombo = currentCombo;
        }

        if (currentMultiplier - 1 < multiplierThresholds.Length) //If there is an existing multiplier threshold to check...
        //e.g. if currentMultiplier - 1 = 3 and multiplierThresholds.Length = 3, don't check for a multiplier increase
        {
            multiplierTracker += 1; //Increase the multiplierTracker by 1

            if (multiplierThresholds[currentMultiplier - 1] <= multiplierTracker) //And if the multiplierTracker passes a multiplier threshold (e.g. 50)...
            {
                multiplierTracker = 0; //Reset the multiplierTracker
                currentMultiplier += 1; //And increase the current score multiplier by 1
            }
        }

        comboText.text = "x" + currentCombo;
        scoreText.text = currentScore.ToString();
        //Update on screen UI to show the current combo and score
    }

    public void OkayHit()
    {
        currentScore += 100 * currentMultiplier;
        NoteHit(); //Call the NoteHit function to check for a multiplier increase and update UI
        okayHits += 1;
    }

    public void GoodHit()
    {
        currentScore += 125 * currentMultiplier; //Provide more points for more accurate hits
        NoteHit();
        goodHits += 1;
    }

    public void PerfectHit()
    {
        currentScore += 150 * currentMultiplier;
        NoteHit();
        perfectHits += 1;
    }

    public void NoteMissed()
    {
        if (PlayerPrefs.GetString("instaFail") == "true")
        //If the player has the instant fail option turned on
        {
            StartCoroutine(cameraShake.Shake(0.1f, 0.5f));
            resultsScreenBackground.SetActive(true);
            levelFailedScreen.SetActive(true);
            levelHasEnded = true;
            music.Pause();
            AudioListener.pause = true;
            levelFailedScoreText.text = currentScore + " (x" + currentHighestCombo + " COMBO)";
            //End the level prematurely and display a 'level failed' screen
        }
        else if (currentCombo > 300)
        {
            StartCoroutine(cameraShake.Shake(0.1f, 0.5f));
            //Call the Shake Coroutine of the cameraShake script, passing in a duration of 0.1 seconds and a magnitude of 0.5
        }
        else if (currentCombo > 200)
        {
            StartCoroutine(cameraShake.Shake(0.1f, 0.3f));
        }
        else if(currentCombo > 100)
        {
            StartCoroutine(cameraShake.Shake(0.1f, 0.2f));
        }
        else if(currentCombo > 50)
        {
            StartCoroutine(cameraShake.Shake(0.1f, 0.1f));
        }
        else
        {
            StartCoroutine(cameraShake.Shake(0.1f, 0.05f));
        }
        //Losing a higher value combo causes a more intense camera shake

        missedSoundEffect.Play();

        currentCombo = 0;
        currentMultiplier = 1;
        multiplierTracker = 0;
        //Reset the currentCombo, currentMultiplier and multiplierTracker when the player misses a note

        comboText.text = "x0"; //Update the combo text
        missedHits++; //Used for tracking accuracy at the end of the level
    }
}