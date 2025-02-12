using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    GameObject[][] spheres; 
    static int numHearts = 3; // Number of hearts
    static int numSphere = 500;
    float[] times;
    Vector3[][] initPos; 
    Vector3[][] startPosition, endPosition;
    float[] lerpFractions; 
    float[] heartSizes; // Random sizes for each heart
    Vector3[] heartPositions; // Random positions for each heart

    // Start is called before the first frame update
    void Start()
    {
        // Initialize arrays
        spheres = new GameObject[numHearts][];
        initPos = new Vector3[numHearts][];
        startPosition = new Vector3[numHearts][];
        endPosition = new Vector3[numHearts][];
        times = new float[numHearts];
        lerpFractions = new float[numHearts];
        heartSizes = new float[numHearts];
        heartPositions = new Vector3[numHearts];

        // Randomize heart sizes and positions
        for (int h = 0; h < numHearts; h++)
        {
            heartSizes[h] = Random.Range(0.5f, 2f); // Random size for each heart
            heartPositions[h] = new Vector3(Random.Range(-10f, 10f), Random.Range(-5f, 5f), Random.Range(-10f, 10f)); // Random position for each heart
            times[h] = Random.Range(0f, 2f * Mathf.PI); // Random start time for each heart

            // Initialize sphere arrays for each heart
            spheres[h] = new GameObject[numSphere];
            initPos[h] = new Vector3[numSphere];
            startPosition[h] = new Vector3[numSphere];
            endPosition[h] = new Vector3[numSphere];

            // Define target positions for each heart
            for (int i = 0; i < numSphere; i++)
            {
                // Random start positions
                float r = 15f;
                startPosition[h][i] = new Vector3(r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f));

                // Heart shape end position
                float t = i * 2 * Mathf.PI / numSphere;
                endPosition[h][i] = CalculateHeartPosition(t, times[h], heartSizes[h], heartPositions[h]);
            }

            // Create spheres for each heart
            for (int i = 0; i < numSphere; i++)
            {
                spheres[h][i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                spheres[h][i].transform.position = startPosition[h][i];

                // Color
                Renderer sphereRenderer = spheres[h][i].GetComponent<Renderer>();
                float brightness = (float)i / numSphere; // Brightness varies from 0 to 1
                Color color = Color.HSVToRGB(0f, 1f, brightness); // Hue = 0 (red), full saturation, variable brightness
                sphereRenderer.material.color = color;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int h = 0; h < numHearts; h++)
        {
            times[h] += Time.deltaTime;

            for (int i = 0; i < numSphere; i++)
            {
                lerpFractions[h] = Mathf.Sin(times[h]) * 0.5f + 0.5f;

                float t = i * 2 * Mathf.PI / numSphere;
                startPosition[h][i] = new Vector3(10f * Mathf.Sin(t + times[h]), 10f * Mathf.Cos(t + times[h]), 10f * Mathf.Sin(t + times[h]));
                endPosition[h][i] = CalculateHeartPosition(t, times[h], heartSizes[h], heartPositions[h]);

                spheres[h][i].transform.position = Vector3.Lerp(startPosition[h][i], endPosition[h][i], lerpFractions[h]);

                // Color Update over time
                Renderer sphereRenderer = spheres[h][i].GetComponent<Renderer>();
                float brightness = Mathf.Abs(Mathf.Sin((float)i / numSphere + times[h])); // Brightness oscillates over time
                Color color = Color.HSVToRGB(0f, 1f, brightness); // Hue = 0 (red), full saturation, variable brightness
                sphereRenderer.material.color = color;
            }
        }
    }

    Vector3 CalculateHeartPosition(float t, float time, float size, Vector3 heartPosition)
    {
        // Pulsating heart size
        float pulsation = 1f + 0.2f * Mathf.Sin(time * 2f);

        // Calculate heart shape and apply size and position
        return new Vector3(
            size * pulsation * 5f * Mathf.Sqrt(2f) * Mathf.Sin(t) * Mathf.Sin(t) * Mathf.Sin(t) + heartPosition.x,
            size * pulsation * 5f * (-Mathf.Cos(t) * Mathf.Cos(t) * Mathf.Cos(t) - Mathf.Cos(t) * Mathf.Cos(t) + 2 * Mathf.Cos(t)) + 3f + heartPosition.y,
            10f + Mathf.Sin(time) + heartPosition.z);
    }
}