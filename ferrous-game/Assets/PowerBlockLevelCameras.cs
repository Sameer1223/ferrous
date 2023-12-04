using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Ferrous.Player;

namespace Ferrous
{
    public class PowerBlockLevelCameras : MonoBehaviour
    {
        private GameObject PlayerCamera;

        private GameObject Room1Camera;
    
        private  GameObject Room2Camera;

        private GameObject[] CameraList;

        private Transform PlayerTransform;

        public GameObject CenterCross;

        private bool inRoomCamera = false;

        private int cameraTransformX = -61;
        // Start is called before the first frame update
    
        [Header("InputChecks")]
        private bool _cameraSwitchInput;
        private Vector2 _mousePos;

        void Start()
        {
            CameraList = new GameObject[2];
            Room1Camera = GameObject.Find("Room 1 Cam");
            CameraList[0] = Room1Camera;
            Room2Camera = GameObject.Find("Room 2 Cam");
            CameraList[1] = Room2Camera;
            PlayerTransform = GameObject.Find("Player").GetComponent<Transform>();
            SwitchToCamera(null);
        }

        // Update is called once per frame
        void Update()
        {
            PlayerInput();
            if (_cameraSwitchInput && !inRoomCamera)
            {
                DisablePlayer();
                if (PlayerTransform.position.x <= cameraTransformX)
                {
                    SwitchToCamera(Room2Camera);
                }
                else
                {
                    SwitchToCamera(Room1Camera); 
                }
                inRoomCamera = true;
           
            }
             else if (_cameraSwitchInput && inRoomCamera)
            {
                EnablePlayer();
                SwitchToCamera(null);
                inRoomCamera = false;
                
            }
            else if (!_cameraSwitchInput && inRoomCamera) {
                // in camera view but if player moves to other room, we need to switch to other room
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

        private void SwitchToCamera(GameObject targetCamera)
        {
            foreach (GameObject camera in CameraList)
            {
                if (camera == targetCamera){
                     camera.SetActive(true);

                } else {
                     camera.SetActive(false);
                }
               
            }
        }
    
        private void PlayerInput()
        {
            _cameraSwitchInput = InputManager.instance.CameraSwitchInput;
            _mousePos = Mouse.current.position.ReadValue();
        }

        private void EnablePlayer(){
            CenterCross.SetActive(true);
            
        }

        private void DisablePlayer(){
            CenterCross.SetActive(false);
            
        }
    }
}