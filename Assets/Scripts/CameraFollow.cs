using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The character to follow
    public Vector3 offset = new Vector3(0f, 5f, -10f); // Camera offset from the target
    public float smoothSpeed = 0.125f; // Smoothing speed for camera movement

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("CameraFollow: No target assigned.");
            return;
        }

        // Calculate the desired position for the camera
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate between the current camera position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;

        // Make the camera look at the target
        transform.LookAt(target);
    }
}