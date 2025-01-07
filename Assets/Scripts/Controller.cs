using UnityEngine;

public class Controller : MonoBehaviour
{
    public KeyCode keyToPress; //The keyboard key that will activate this controller (green = 1, red = 2, blue = 3)
    public Transform controller; //Current position of this controller
    public Vector3 originalposition; //Starting position of this controller

    void Update()
    {
        if (Input.GetKeyDown(keyToPress)) //If the corresponding key is pressed during this frame
        {
            controller.transform.position -= new Vector3(0f, 0.25f, 0f);
            //Move this controller down 0.25 units
        }
        if (Input.GetKeyUp(keyToPress)) //If the corresponding key is released during this frame
            controller.transform.position = originalposition; //Reset this controller to its original position
    }
}
