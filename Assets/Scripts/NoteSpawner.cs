using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameManager gameManager;
    private Note note;

    public float[] greenNotes, redNotes, blueNotes; //Keep all the PositionInBeats of notes in the song
    public GameObject notes, greenNote, redNote, blueNote; //Refer to the prefabs of each type of note to be spawned
    private int greenNextIndex = 0, redNextIndex = 0, blueNextIndex = 0; //The index of the next note to check in the array
    private int noteSpawnDistance;

    void Start()
    {
        if (PlayerPrefs.GetString("slowerSong") == "true") //If the player has the slower song option turned on...
        {
            noteSpawnDistance = 90; //Spawn the notes closer to the controllers
            //The notes all move at a constant speed using a lerp...
            //So spawning them closer to the controlelrs means they have the same amount of time to travel a smaller distance
            //This means they can travel slower than normal while still reaching the bottom in time
        }
        else
        {
            noteSpawnDistance = 140; //By default, notes will spawn further away and move faster down the screen
        }
    }

    void Update()
    {
        if (gameManager.music.isPlaying == true)
        {
            if (greenNextIndex < greenNotes.Length && greenNotes[greenNextIndex] <= gameManager.songPositionInBeats + 22)
            //If there are still notes in the array to check...
            //And the next note is meant to be hit in 22 beats time...
            {
                GameObject thisNote = Instantiate(greenNote, new Vector3(-2.5f, 0.5f, noteSpawnDistance), Quaternion.identity);
                //Spawn a green note object at the top of the screen

                thisNote.transform.parent = notes.transform; 
                //Make this newly instantiated note a child object of the NoteSpawner class

                note = thisNote.GetComponent<Note>();
                note.beatOfThisNote = greenNotes[greenNextIndex];
                //Indicates which beat this newly instantiated note should reach the bottom of the screen

                greenNextIndex++; //Prepare to check the next index of the greenNotes array
            }

            if (redNextIndex < redNotes.Length && redNotes[redNextIndex] <= gameManager.songPositionInBeats + 22)
            {
                GameObject thisNote = Instantiate(redNote, new Vector3(0f, 0.5f, noteSpawnDistance), Quaternion.identity);
                thisNote.transform.parent = notes.transform;
                note = thisNote.GetComponent<Note>();
                note.beatOfThisNote = redNotes[redNextIndex]; 
                redNextIndex++;
            }

            if (blueNextIndex < blueNotes.Length && blueNotes[blueNextIndex] <= gameManager.songPositionInBeats + 22)
            {
                GameObject thisNote = Instantiate(blueNote, new Vector3(2.5f, 0.5f, noteSpawnDistance), Quaternion.identity);
                thisNote.transform.parent = notes.transform;
                note = thisNote.GetComponent<Note>();
                note.beatOfThisNote = blueNotes[blueNextIndex];
                blueNextIndex++;
            }
        }
    }
}