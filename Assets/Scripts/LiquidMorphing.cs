using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidMorphing : MonoBehaviour
{
    public int numShapes = 50; // Number of shapes
    public float minRadius = 2f; // Minimum radius
    public float maxRadius = 5f; // Maximum radius
    public float morphSpeed = 0.5f; // Reduced speed for smoother movement
    public float scaleMultiplier = 2f; // Scale multiplier for audio reactivity
    public float targetChangeInterval = 2f; // Interval for changing target positions

    private GameObject[] shapes;
    private Vector3[] basePositions;
    private Vector3[] targetPositions;
    private float[] audioSpectrumData;
    private float lastTargetChangeTime;
    private float timeElapsed;

    void Start()
    {
        shapes = new GameObject[numShapes];
        basePositions = new Vector3[numShapes];
        targetPositions = new Vector3[numShapes];
        audioSpectrumData = new float[AudioSpectrum.FFTSIZE];

        // Create shapes and set initial positions
        for (int i = 0; i < numShapes; i++)
        {
            shapes[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            shapes[i].transform.position = Random.onUnitSphere * maxRadius;
            basePositions[i] = shapes[i].transform.position;
            targetPositions[i] = Random.onUnitSphere * maxRadius;

            Renderer shapeRenderer = shapes[i].GetComponent<Renderer>();
            float hue = (float)i / numShapes; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
            shapeRenderer.material.color = color;
        }

        lastTargetChangeTime = Time.time;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // Smooth radius oscillation over 2 minutes (1 min decrease, 1 min increase)
        float oscillationValue = Mathf.PingPong(timeElapsed / 60f, 1f);
        float radius = Mathf.Lerp(minRadius, maxRadius, oscillationValue);

        // Get the audio spectrum data
        audioSpectrumData = AudioSpectrum.samples;

        float timeSinceLastChange = Time.time - lastTargetChangeTime;
        float lerpValue = Mathf.SmoothStep(0f, 1f, timeSinceLastChange / targetChangeInterval);

        // Update shape positions and scales based on audio data
        for (int i = 0; i < numShapes; i++)
        {
            // Smoothly interpolate positions
            shapes[i].transform.position = Vector3.Slerp(basePositions[i], targetPositions[i], lerpValue);

            // Scale shapes based on audio amplitude
            float audioScale = Mathf.Lerp(shapes[i].transform.localScale.x, 1f + (audioSpectrumData[i % AudioSpectrum.FFTSIZE] * scaleMultiplier), 0.1f);
            shapes[i].transform.localScale = Vector3.one * audioScale;

            // Color Update over time
            Renderer shapeRenderer = shapes[i].GetComponent<Renderer>();
            float hue = (float)i / numShapes; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(Mathf.Abs(hue * Mathf.Cos(timeElapsed)), Mathf.Cos(AudioSpectrum.audioAmp / 10f), 2f + Mathf.Cos(timeElapsed)); // Full saturation and brightness
            shapeRenderer.material.color = color;
        }

        // Gradually update target positions
        if (timeSinceLastChange >= targetChangeInterval)
        {
            for (int i = 0; i < numShapes; i++)
            {
                basePositions[i] = shapes[i].transform.position; // Update base position
                targetPositions[i] = Random.onUnitSphere * radius;
            }
            lastTargetChangeTime = Time.time;
        }
    }
}
