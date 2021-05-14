using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.1f;

    // Offset variable to control where the camera is, relative to the 'float'
    public Vector3 offset;

    void FixedUpdate() 
    { 
        Vector3 endPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, endPos, smoothSpeed);
    }
}
