using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    [Header("Camera Control")]
    public Transform CameraTarget;
    public float rotationPower = 1.0f;
    public Quaternion nextRotation;
    public float rotationLerp = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void LateUpdate()
    {
        CameraControl();
    }

    private void CameraControl()
    {
        #region Player Based Rotation
        
        //Move the player based on the X input on the controller
        transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationPower, Vector3.up);

        #endregion

        #region Follow Transform Rotation

        //Rotate the Follow Target transform based on the input
        CameraTarget.transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X")  * rotationPower, Vector3.up);
        CameraTarget.transform.rotation *= Quaternion.AngleAxis(-Input.GetAxis("Mouse Y")  * rotationPower, Vector3.right);

        // get angles of rotation
        var angles = CameraTarget.transform.localEulerAngles;
        angles.z = 0;

        var angle = CameraTarget.transform.localEulerAngles.x;

        //Clamp the Up/Down rotation of the camera so it doesn't flip
        if (angle > 180 && angle < 300)
        {
            angles.x = 300;
        }
        else if (angle < 180 && angle > 75)
        {
            angles.x = 75;
        }
        // set the camera target to our clamped thing
        CameraTarget.transform.localEulerAngles = angles;

        #endregion
        // determine where the player should rotate to next if they are not moving
        nextRotation = Quaternion.Lerp(CameraTarget.transform.rotation, nextRotation, rotationLerp); 

        //Set the player rotation based on the look transform
        transform.rotation = Quaternion.Euler(0, CameraTarget.transform.rotation.eulerAngles.y, 0);
        //reset the y rotation of the look transform
        CameraTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);


    }
}
