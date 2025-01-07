using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu: MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public AudioSource music;
    public TMPro.TMP_Text pauseTimer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused == true)
            {
                Resume(); //Allow the user to press the escape key to resume rather than using the button on screen
            }
            else if (music.isPlaying == true)
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true); //Display the pause menu
        Time.timeScale = 0f; //Pause the Unity physics system (controls object movement)
        AudioListener.pause = true; //Unpause the Unity audio system
        GameIsPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        pauseTimer.text = "3";
        pauseTimer.gameObject.SetActive(true);
        StartCoroutine(DecreaseTimer());
    }

    IEnumerator DecreaseTimer()
    {
        yield return new WaitForSecondsRealtime(0.75f); //Wait for 0.75 seconds
        pauseTimer.text = "2"; //Decrease the number shown on screen

        yield return new WaitForSecondsRealtime(0.75f);
        pauseTimer.text = "1";

        yield return new WaitForSecondsRealtime(0.75f);
        pauseTimer.gameObject.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        GameIsPaused = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        GameIsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToStartMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start Menu"); //Begin loading the start menu
        AudioListener.pause = false; //Prevents the start menu from having no sound
        GameIsPaused = false;
    }
}
