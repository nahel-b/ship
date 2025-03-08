using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    private float initialOrthoSize;
    private Vector3 initialScale;
    private Vector3 initialPositionFromCamera;
    
    void Start()
    {
        // Store initial values
        initialOrthoSize = Camera.main.orthographicSize;
        initialScale = transform.localScale;
        initialPositionFromCamera = transform.position - Camera.main.transform.position;
    }
    
    void LateUpdate()
    {
        // Calculate scale factor based on zoom level
        float scaleFactor = Camera.main.orthographicSize / initialOrthoSize;
        
        // Apply direct scale to shrink when zooming in and enlarge when zooming out
        transform.localScale = initialScale * scaleFactor;
        
        // Adjust position to maintain relative position from camera
        //Vector3 desiredPositionFromCamera = initialPositionFromCamera;
        //transform.position = Camera.main.transform.position + desiredPositionFromCamera;
    }
} 