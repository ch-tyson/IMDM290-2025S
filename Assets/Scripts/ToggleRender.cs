using UnityEngine;

public class ToggleRenderer : MonoBehaviour
{
    public float delay = 20.0f; // Time in seconds before toggling the renderer

    private Renderer objectRenderer;
    private bool isRendering = true;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("No Renderer component found on the object!");
            return;
        }

        // Start the repeated invocation of ToggleRendering method
        InvokeRepeating("ToggleRendering", delay, delay);
    }

    private void ToggleRendering()
    {
        isRendering = !isRendering;
        objectRenderer.enabled = isRendering;
    }
}
