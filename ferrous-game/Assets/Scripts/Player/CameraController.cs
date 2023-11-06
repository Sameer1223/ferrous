using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    
    [Header("Camera Control")]
    public Transform playerTransform;
    public float rotationPower = 1.0f;
    public Quaternion nextRotation;
    public float rotationLerp = 0.5f;

    [Header("Player Control")]
    public float playerRotationTime = 0.1f;
    private float _turnSmoothVelocity;

    private void Update()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 0.5f, playerTransform.position.z);
    }

    private void LateUpdate()
    {
        if (!PauseMenu.IsPaused)
        {
            CameraControl();
        }
    }

    private void CameraControl()
    {

        Vector2 lookInput = InputManager.instance.LookInput;
        Vector2 moveInput = InputManager.instance.MovementInput;

        #region Follow Transform Rotation

        //Rotate the Follow Target transform based on the input
        transform.rotation *= Quaternion.AngleAxis(lookInput.x * rotationPower, Vector3.up);
        transform.rotation *= Quaternion.AngleAxis(-lookInput.y  * rotationPower, Vector3.right);

        // get angles of rotation
        var angles = transform.localEulerAngles;
        angles.z = 0;

        var angle = transform.localEulerAngles.x;

        //Clamp the Up/Down rotation of the camera so it doesn't flip
        if (angle > 180 && angle < 300)
        {
            angles.x = 300;
        }
        else if (angle < 180 && angle > 75)
        {
            angles.x = 75;
        }

        // set the camera target to its clamped values
        transform.localEulerAngles = angles;
        #endregion

        // determine where the player should rotate to next if they are not moving
        nextRotation = Quaternion.Lerp(transform.rotation, nextRotation, Time.deltaTime * rotationLerp);

        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        // if the player is inputting movement, rotate the player based on the input
        if (moveDir.magnitude > 0)
        {
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + transform.eulerAngles.y;
            float smoothenedAngle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, playerRotationTime);
            playerTransform.rotation = Quaternion.Euler(0f, smoothenedAngle, 0f);
        }
    }
}
