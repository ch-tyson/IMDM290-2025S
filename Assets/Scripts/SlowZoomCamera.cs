using UnityEngine;

public class SlowZoomCamera : MonoBehaviour
{
    private Camera mainCamera;
    public float speed = 0.05f;
    
    // Starting and target positions for smooth interpolation
    public Vector3 startPosition;
    public Vector3 targetPosition;
    
    // Interpolation time
    private float currentLerpTime = 0f;
    public float lerpDuration = 30f;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        startPosition = transform.position;
        
        // Set target position behind the current position
        targetPosition = transform.position + (Vector3.back * 10f);
    }

    void Update()
    {
        currentLerpTime += Time.deltaTime;
        
        // Calculate interpolation value
        float t = currentLerpTime / lerpDuration;
        
        // Smooth the interpolation using a sine wave
        t = Mathf.Sin(t * Mathf.PI * 0.5f);
        
        // Move camera smoothly between start and target positions
        transform.position = Vector3.Lerp(startPosition, targetPosition, t);
    }
}