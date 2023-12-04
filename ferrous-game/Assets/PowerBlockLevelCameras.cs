using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Ferrous.Player;

namespace Ferrous
{
    public class PowerBlockLevelCameras : MonoBehaviour
    {
        private GameObject PlayerCamera;

        public GameObject[] CameraList;

        private Transform PlayerTransform;

        public GameObject CenterCross;

        public bool inRoomCamera = false;

        public GameObject ChangeCameraHintText;
        public GameObject ChangeCameraHintImage;
        public bool cameraChange;
        private GameObject switchableCamera;
        // Start is called before the first frame update
    
        [Header("InputChecks")]
        private bool _cameraSwitchInput;
        private Vector2 _mousePos;

        void Start()
        {
            PlayerTransform = GameObject.Find("Player").GetComponent<Transform>();

            SwitchToCamera(null);
        }

        // Update is called once per frame
        void Update()
        {   
            cameraChange = false;
            switchableCamera = null;
            foreach (GameObject camera in CameraList){
                    if (camera.transform.parent.gameObject.GetComponent<CameraZoneCollision>().isCollided){
                        cameraChange = true;
                        switchableCamera = camera;
                    }
                }
            if (cameraChange){
                EnableChangeCameraHint();
                PlayerInput();
                if (_cameraSwitchInput && !inRoomCamera)
                {
                    DisableCross();
                    SwitchToCamera(switchableCamera);
                    inRoomCamera = true;
                }
            
                else if (_cameraSwitchInput && inRoomCamera)
                {
                    EnableCross();
                    SwitchToCamera(null);
                    inRoomCamera = false;
                    
                }
                else if (!_cameraSwitchInput && inRoomCamera) {
                    // in camera view but if player moves to other room, we need to switch to other room
                SwitchToCamera(switchableCamera);
                }
            } else {
                DisableChangeCameraHint();
                if (inRoomCamera){
                EnableCross();
                SwitchToCamera(null);
                inRoomCamera = false;
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

        private void EnableCross(){
            CenterCross.SetActive(true);
            
        }

        private void DisableCross(){
            CenterCross.SetActive(false);
            
        }

        private void EnableChangeCameraHint(){
            ChangeCameraHintText.SetActive(true);
            ChangeCameraHintImage.SetActive(true);
            
        }

        private void DisableChangeCameraHint(){
            ChangeCameraHintText.SetActive(false);
            ChangeCameraHintImage.SetActive(false);
            
        }
    }
}