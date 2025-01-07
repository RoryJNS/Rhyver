using UnityEngine;

public class MenuVisualiser : MonoBehaviour
{
    public AudioSource music;
    private float minHeight = 15f, maxHeight = 240f; //The minimum and maximum height of each white bar
    private CanvasRenderer[] visualiserBars; //The array containing all bar images of the visualiser
    private float[] spectrumData = new float[1024]; //The array to fill with sampled spectrum data

    void Start()
    {
        visualiserBars = GetComponentsInChildren<CanvasRenderer>(); //Finds all the bar images contained in this parent visualiser object
    }

    void Update()
    {
        if (music.isPlaying)
        {
            music.GetSpectrumData(spectrumData, 0, FFTWindow.Rectangular);
            //GetSpectrumData provides a float array of the recent samples of the music, where each float represents the amplitude at that point relative to other samples

            for (int i = 0; i < visualiserBars.Length; i++) //For each bar in the visualiser...
            {
                Vector2 newSize = visualiserBars[i].GetComponent<RectTransform>().rect.size; //Find the size component of the image

                newSize.y = Mathf.Lerp(newSize.y, minHeight + (spectrumData[i] * (maxHeight - minHeight) * 5f), 0.1f);
                //The lerp function calculates how much to stretch the bar image during this frame in order for it to move at a constant speed across 0.1 seconds

                visualiserBars[i].GetComponent<RectTransform>().sizeDelta = newSize; //Stretch the bar image by this amount
                //Doing this every frame will help the bars move smoothly on screen
            }
        }
    }
}
