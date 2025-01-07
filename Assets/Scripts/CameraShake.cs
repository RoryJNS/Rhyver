using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    //Using a coroutine ensures the shake function is called outside the update function
    {
        Vector3 originalPos = transform.localPosition; //Starting position of the camera
        float elapsed = 0; //Amount of time that has passed since the camera shake started

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            //Generates random values for x and y, between -1 and 1

            transform.localPosition = new Vector3(x, y, originalPos.z); 
            //Moves the position of the camera object using these random values
            
            elapsed += Time.deltaTime; 
            //Increases the elapsed time using the interval, in seconds, from the last frame to the current one

            yield return null;
            //Waits until the next frame is drawn before continuing on to the next iteration
        }

        transform.localPosition = originalPos; //Resets the camera to its original position
    }

}
