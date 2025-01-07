using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    void Update()
    {
        Destroy(gameObject, 1); //Destroy this particle effect after 1 second
    }
}
