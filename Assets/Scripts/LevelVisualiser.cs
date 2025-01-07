using UnityEngine;

public class LevelVisualiser : MonoBehaviour
{
    public AudioSource music;
    private float minScale = 0.3f, maxScale = 1f;

    private MeshRenderer[] visualiserBars; 
    //The visualiser bars within the level are 3D objects rather than 2D images, so they use a different type of renderer

    private float[] spectrumData = new float[1024];

    void Start()
    {
        visualiserBars = GetComponentsInChildren<MeshRenderer>();
    }

    void Update()
    {
        if (music.isPlaying)
        {
            music.GetSpectrumData(spectrumData, 0, FFTWindow.Rectangular);
            for (int i = 0; i < visualiserBars.Length; i++)
            {
                Vector3 newSize = visualiserBars[i].GetComponent<Transform>().localScale;
                newSize.y = Mathf.Lerp(newSize.y, (minScale + (spectrumData[i] * (maxScale - minScale) * 25f)), 0.1f);
                visualiserBars[i].GetComponent<Transform>().localScale = newSize;
                //This visualiser is fundamentally the same as the one used for the menu visualiser, but the y scale of the object is adjusted rather than an image area
            }
        }
    }
}
