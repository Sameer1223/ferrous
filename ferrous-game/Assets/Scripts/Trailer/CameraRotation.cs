using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Transform target;  // The object around which the camera will rotate
    public float rotationSpeed = 5.0f;  // Speed of rotation

    void Update()
    {
        // Check if the target object is assigned
        if (target == null)
        {
            Debug.LogError("Target not assigned for camera rotation!");
            return;
        }

        // Calculate the desired rotation angle
        float angle = Time.deltaTime * rotationSpeed;

        // Rotate the camera around the target
        transform.RotateAround(target.position, Vector3.up, angle);
    }
}