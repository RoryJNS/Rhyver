using UnityEngine;

public class Note : MonoBehaviour
{
    private bool canBePressed; //Whether this note is making contact with a controller
    public KeyCode keyToPress; //The key that must be pressed to hit this note (green = 1, red = 2, blue = 3)
    private bool cleared; //Whether this note has gone through a collision check

    private Vector3 originalPos; //The starting position of this note
    private Vector3 removePos; //The position this note must reach in order to be destroyed
    public float beatOfThisNote; //The beat that this note should be hit

    private GameObject gameManagerObject;
    private GameManager gameManagerScript;

    public GameObject missEffect, okayEffect, goodEffect, perfectEffect;

    void Start()
    {
        originalPos = transform.position; //Records the starting position of this note
        removePos = new Vector3 (transform.position.x, 0.5f, 0f);

        gameManagerObject = GameObject.Find("GameManager");
        gameManagerScript = gameManagerObject.GetComponent<GameManager>();
        //The game manager script is recorded when this note is instantiated
        //Cannot be assigned in the editor before the game starts because this note object does not exist until part way through the song
    }

    void Update()
    {
        if (Input.GetKeyDown(keyToPress)) //If the corresponding key for this note is pressed...
        {
            if (canBePressed == true) //And if a controller has entered the collider area for this object...
            {
                cleared = true; //Prevent the current note from going through multiple collision checks
                GameManager.instance.NoteHit(); //Call the NoteHit function of the GameManager script

                if (transform.position.z > 0.45 || transform.position.z < -0.45)
                //Checks within a margin of error to see how close the player was
                {
                    GameManager.instance.OkayHit();
                    //Calls the OkayHit function of the GameManager class to calculate score and multiplier increases

                    Instantiate(okayEffect, transform.position, okayEffect.transform.rotation);
                    //Instantiate an okayEffect on the exact position of the note
                    //Use the preset rotation of the okayEffect object (0 degrees)
                }
                else if (transform.position.z > 0.4 || transform.position.z < -0.4)
                {
                    GameManager.instance.GoodHit();
                    Instantiate(goodEffect, transform.position, goodEffect.transform.rotation);
                }
                else
                {
                    GameManager.instance.PerfectHit();
                    Instantiate(perfectEffect, transform.position, perfectEffect.transform.rotation);
                }
                DestroyNote();
            }
        }

        if (PauseMenu.GameIsPaused == false && transform.position.z > 0f)
        //If the game is running and this note has not yet reached the bottom of the screen
        {
            transform.position = Vector3.Lerp(originalPos, removePos, (22 - (beatOfThisNote - gameManagerScript.songPositionInBeats)) / 22);
            //Update its position at a constant speed based on the beat it needs to be pressed on and the current song position
        }

        if (PauseMenu.GameIsPaused == false && transform.position.z <= 0f)
        //If this note has gone past the controller
        {
            transform.position = Vector3.MoveTowards(transform.position, removePos - new Vector3(0f, 0f, 2f), 1.95f * Time.deltaTime * 2f);
            //Move it slightly further and at a constant speed
        }
    }

    void OnTriggerEnter(Collider collision)
    //OnTriggerEnter is called when another object enters the collider area for this object
    {
        if (collision.tag == "Controller") //If the object entering the collider area has the 'Controller' tag...
            canBePressed = true; //Allows an upcoming user input to remove the note from the game
    }

    void OnTriggerExit(Collider collision)
    //OnTriggerExit is called when another object leaves the collider area for this object
    {
        if (collision.tag == "Controller") //If the object is leaving a collider area with the 'Controller' tag...
        {
            canBePressed = false;
            if (cleared == false) //And if the object has not already gone through a collision check...
            {
                cleared = true; //Prevent the current note from going through multiple collision checks
                GameManager.instance.NoteMissed(); //Call the NoteMissed function of the GameManager script
                Instantiate(missEffect, transform.position + new Vector3(0f, 0f, 0.5f), missEffect.transform.rotation);
                Invoke("DestroyNote", 0.5f); //Wait 0.5 seconds and then call the DestroyNote function
            }
        }
    }

    void DestroyNote()
    {
        gameObject.SetActive(false); //Remove this note object from the game
    }
}
