using UnityEngine;

public class LightColorChanger : MonoBehaviour
{
    public Light lightToChange;  
    public Color startColor = Color.yellow;
    public Color endColor = Color.magenta;
    public float duration = 124.0f;    // Duration of song in seconds

    private float time = 0.0f;

    void Update()
    {
        // Calculate the proportion of the time passed
        time += Time.deltaTime / duration;
        // Interpolate the color based on time
        Color currentColor = Color.Lerp(startColor, endColor, time);

        // Apply the color to the light
        lightToChange.color = currentColor;

        // Reset time to loop the color change
        if (time >= 1.0f)
        {
            time = 0.0f;
            // Swap colors for a loop effect
            Color temp = startColor;
            startColor = endColor;
            endColor = temp;
        }
    }
}
