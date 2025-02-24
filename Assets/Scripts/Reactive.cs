using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactive : MonoBehaviour
{
    GameObject[] shapes;
    static int numShapes = 200; 
    float time = 0f;
    Vector3[] initPos;
    Vector3[] startPosition, endPosition;
    float lerpFraction; // Lerp point between 0~1
    float t;

    // Array of primitive types for variety
    PrimitiveType[] shapeTypes = { PrimitiveType.Cube, PrimitiveType.Capsule, PrimitiveType.Cylinder };

    // Start is called before the first frame update
    void Start()
    {
        // Assign proper types and sizes to the variables.
        shapes = new GameObject[numShapes];
        initPos = new Vector3[numShapes]; // Start positions
        startPosition = new Vector3[numShapes]; 
        endPosition = new Vector3[numShapes]; 
        
        // Define target positions. Start = random, End = circular 
        for (int i =0; i < numShapes; i++){
            // Random start positions
            float r = 10f;
            startPosition[i] = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));

            r = 5f; // radius of the circle
            // Circular end position
            endPosition[i] = new Vector3(r * Mathf.Sin(i * 2 * Mathf.PI / numShapes), r * Mathf.Cos(i * 2 * Mathf.PI / numShapes), 0f);
        }
        // Let there be shapes..
        for (int i =0; i < numShapes; i++){
            // Draw primitive elements with random shapes
            PrimitiveType randomShape = shapeTypes[Random.Range(0, shapeTypes.Length)];
            shapes[i] = GameObject.CreatePrimitive(randomShape); 

            // Position
            initPos[i] = startPosition[i];
            shapes[i].transform.position = initPos[i];

            // Color
            // Get the renderer of the shapes and assign colors.
            Renderer shapeRenderer = shapes[i].GetComponent<Renderer>();
            // HSV color space: https://en.wikipedia.org/wiki/HSL_and_HSV
            float hue = (float)i / numShapes; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
            shapeRenderer.material.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Measure Time 
        // Time.deltaTime = The interval in seconds from the last frame to the current one
        // but what if time flows according to the music's amplitude?
        time += Time.deltaTime * AudioSpectrum.audioAmp; 
        // what to update over time?
        for (int i =0; i < numShapes; i++){
            // Lerp : Linearly interpolates between two points.
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.Lerp.html
            // Vector3.Lerp(startPosition, endPosition, lerpFraction)
            
            // lerpFraction variable defines the point between startPosition and endPosition (0~1)
            lerpFraction = Mathf.Sin(time + i * 0.1f) * 0.5f + 0.5f;

            // Lerp logic. Update position       
            t = i* 2 * Mathf.PI / numShapes;
            shapes[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction);
            float scale = 1f + AudioSpectrum.audioAmp * 2f; // Scale more dramatically
            shapes[i].transform.localScale = new Vector3(scale, scale, scale);
            shapes[i].transform.Rotate(AudioSpectrum.audioAmp * 10f, AudioSpectrum.audioAmp * 5f, AudioSpectrum.audioAmp * 2f);
            
            // Color Update over time
            Renderer shapeRenderer = shapes[i].GetComponent<Renderer>();
            float hue = (float)i / numShapes; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(Mathf.Abs(hue * Mathf.Cos(time)), Mathf.Cos(AudioSpectrum.audioAmp / 10f), 2f + Mathf.Cos(time)); // Full saturation and brightness
            shapeRenderer.material.color = color;
        }
    }
}