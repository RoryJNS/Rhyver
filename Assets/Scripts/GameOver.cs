using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private bool GameRunning = true;
    public GameObject resultsScreen;

    public void CompleteLevel() //Called when the player reaches the end of the level
    {
        resultsScreen.SetActive(true); //Displays the results screen
    }

    public void EndGame() 
    //Only called if the user missed a note with the instant fail setting turned on
    {
        if (GameRunning == true)
        {
            GameRunning = false;
            Invoke("RestartGame", 2f); //Waits two seconds and then call the RestartGame method
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Reloads the current scene (level)
    }
}
