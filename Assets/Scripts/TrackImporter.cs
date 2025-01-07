using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using AnotherFileBrowser.Windows; //Allows this script to access the 'AnotherFileBrowser' plugin

public class TrackImporter : MonoBehaviour
{
    public AudioSource music;
    public AudioClip defaultMusic;

    void Start()
    {
        StartCoroutine(LoadAudio(PlayerPrefs.GetString("CustomTrack", ""))); 
        //Load the saved main menu music, which may have been imported by the player in a previous session
    }

    IEnumerator LoadAudio(string path) //Coroutine is used so 'yield' statements can be made below
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + path, AudioType.WAV))
        //Attempts to locate the audio clip on the device by passing in a string for the filepath of the audio
        {
            yield return www.SendWebRequest(); //Wait for a result from the web request

            if (www.result == UnityWebRequest.Result.ConnectionError) //If there is no network connection...
            {
                music.clip = defaultMusic; //Use the default start menu music
            }
            else
            {
                music.clip = DownloadHandlerAudioClip.GetContent(www); //Otherwise, download the located audio clip
                music.clip.name = System.IO.Path.GetFileName(path); //Rename the audio clip to the file name
                music.Play(); //Play the audio clip (this will also change the behaviour of the menu visualiser in real time)
            }
        }
    }

    public void AttemptWavImport()
    {
        var bp = new BrowserProperties();
        //Refers to the BrowserProperties class (defined in the AnotherFileBrowser plugin)

        bp.filter = "Audio files (*.wav) | *.wav"; //Only display wav files in the file explorer
        bp.filterIndex = 0;
        
        new FileBrowser().OpenFileBrowser(bp, path => 
        //Attempts to assign a value to the string 'path' using the filepath of a selected file
        //If a valid filepath is assigned, call the OpenFileBrowser function of the FileBrowser script, passing in the BrowserProperties and filepath as parameters
        {
            if (path.EndsWith(".wav"))
            {
                PlayerPrefs.SetString("CustomTrack", path); 
                //Save the filepath of the selected file to be loaded at the start of each play session

                StartCoroutine(LoadAudio(PlayerPrefs.GetString("CustomTrack", "")));
                //Call the coroutine to load the audio data from the selected file
            }
            else //If the file selected is not in the .wav format
            {
                path = ""; //Empty the filepath to remove any reference to the invalid file
            }
        });
    }
}