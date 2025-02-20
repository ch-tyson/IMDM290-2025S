using UnityEngine;

public class Sunset : MonoBehaviour
{
    public Material skyboxMaterial; 
    public Color TintStartColor = Color.blue;
    public Color TintEndColor = Color.red;
    public Color GroundStartColor = Color.blue;
    public Color GroundEndColor = Color.red;
    public float duration = 124.0f;    // Duration of song

    private float time = 0.0f;

    void Update()
    {
        // Change the color over time
        time += Time.deltaTime / duration;
        Color currentColor = Color.Lerp(TintStartColor, TintEndColor, time);
        Color GroundCurrentColor = Color.Lerp(GroundStartColor, GroundEndColor, time);

        // Apply the color to the skybox material
        skyboxMaterial.SetColor("_SkyTint", currentColor);
        skyboxMaterial.SetColor("_GroundColor", currentColor);

           /* // Reset time to loop the color change
            if (time >= 1.0f)
            {
                time = 0.0f;
                // Swap colors for a loop effect
                Color temp = TintStartColor;
                TintStartColor = TintEndColor;
                TintEndColor = temp;
            } */
    }
}
