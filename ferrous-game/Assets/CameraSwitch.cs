using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{

    public Camera Room1Camera;
    
    public Camera Room2Camera;

    public Camera[] CameraList;

    public Transform PlayerTransform;

    public int transitionX = 53;
    // Start is called before the first frame update

    void Start()
    {
        CameraList = new Camera[2];
        Room1Camera = GameObject.Find("First Room Camera").GetComponent<Camera>();
        CameraList[0] = Room1Camera;
        Room2Camera = GameObject.Find("Second Room Camera").GetComponent<Camera>();
        CameraList[1] = Room2Camera;
        PlayerTransform = GameObject.Find("Player").GetComponent<Transform>();
        SwitchToCamera(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") || Input.GetAxisRaw("Fire1") > 0 || Input.GetButton("Fire2") || Input.GetAxisRaw("Fire2") > 0)
        {
            if (PlayerTransform.position.x <= -transitionX)
            {
                SwitchToCamera(Room2Camera);
            }
            else
            {
                SwitchToCamera(Room1Camera); 
            }

        }
        else
        {
            SwitchToCamera(null);
        }

    }

    private void SwitchToCamera(Camera targetCamera)
    {
        foreach (Camera camera in CameraList)
        {
            camera.enabled = (camera == targetCamera);
        }
    }
}
