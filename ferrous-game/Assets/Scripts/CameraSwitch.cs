using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;


public class CameraSwitch : MonoBehaviour
{

    public Camera Room1Camera;
    
    public Camera Room2Camera;

    public Camera[] CameraList;

    public Transform PlayerTransform;

    public int cameraTransformX = -46;
    // Start is called before the first frame update
    
    [Header("InputChecks")]
    private bool _pushInput;
    private bool _pullInput;
    private Vector2 _mousePos;
    private Camera mainCamera;

    void Start()
    {
        CameraList = new Camera[2];
        Room1Camera = GameObject.Find("First Room Camera").GetComponent<Camera>();
        CameraList[0] = Room1Camera;
        Room2Camera = GameObject.Find("Second Room Camera").GetComponent<Camera>();
        CameraList[1] = Room2Camera;
        PlayerTransform = GameObject.Find("Player").GetComponent<Transform>();
        SwitchToCamera(null);
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        if (_pushInput || _pullInput)
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(_mousePos);

            if (Physics.Raycast(ray, out hit, 10f))
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.red);
                if (hit.collider.CompareTag("Metal"))
                {
                    if (PlayerTransform.position.x <= cameraTransformX)
                    {
                        SwitchToCamera(Room2Camera);
                    }
                    else
                    {
                        SwitchToCamera(Room1Camera); 
                    }

                }
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
    
    private void PlayerInput()
    {
        _pushInput = InputManager.instance.PushInput;
        _pullInput = InputManager.instance.PullInput;
        _mousePos = Mouse.current.position.ReadValue();
    }
}
