using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void UpdateButtonTextAndColour(TMPro.TMP_Text buttonText)
    //Called when a button in the options menu is pressed
    //This is a general method used for the instant fail, slower song and fullscreen buttons
    {
        if (buttonText.text == "ON")
        {
            buttonText.text = "OFF";        
        }
        else
        {
            buttonText.text = "ON";
        }
        //Set the text value of the button from ON to OFF or vice versa
    }

    public void ToggleInstantFail(TMPro.TMP_Text buttonText)
    {
        if (buttonText.text == "OFF")
        {
            PlayerPrefs.SetString("instaFail", "false");
        }
        else
        {
            PlayerPrefs.SetString("instaFail", "true");
        }
    }

    public void ToggleSlowerSong(TMPro.TMP_Text buttonText)
    {
        if (buttonText.text == "OFF")
        {
            PlayerPrefs.SetString("slowerSong", "false");
        }
        else
        {
            PlayerPrefs.SetString("slowerSong", "true");
        }
    }

    public void ToggleFullscreen(TMPro.TMP_Text buttonText)
    {
        if (buttonText.text == "OFF")
        {
            PlayerPrefs.SetString("fullscreen", "false");
            Screen.fullScreen = false; //Function provided by Unity to disable fullscreen mode
        }
        else
        {
            PlayerPrefs.SetString("fullscreen", "true");
            Screen.fullScreen = true;
        }
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("MasterVolume", volume);
        audioMixer.SetFloat("MasterVolume", volume); //Adjusts the volume of the audio mixer
        //The audio mixer manages the volume of all sounds in the scene
    }

}
