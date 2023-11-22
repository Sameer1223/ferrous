using Ferrous.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ferrous.Blocks
{
    public class PowerBlocks : MonoBehaviour
    {

        private bool isPulling = false;
        private bool isPushing = false;

        private Transform playerTransform;

        [Header("Push/Pull")]
        private float maxDist = 40f;
        private float minDist = 5.0f;
        public float pullForce = 50.0f;
        private Rigidbody selectedObject;
        private float distToPlayer;
        private bool magnetismInput;
        private Vector3 objectDirection;

        [Header("InputChecks")]
        private bool _pushInput;
        private bool _pullInput;
        private Vector2 _mousePos;
        private bool _selectInput;

        private void Start()
        {
            playerTransform = GameObject.Find("Player").transform;
        }


        void Update()
        {
            if (!PauseMenu.IsPaused)
            {
                PlayerInput();

                // determine if the player is inputting a push / pull
                GetMagnetismInput();
                if (magnetismInput)
                {
                    LookMagnetism();
                }
            }
        }
        private void LateUpdate()
        {
            ApplyMagnesis();
        }

        private void GetMagnetismInput()
        {
            if (_pushInput || _pullInput)
            {
                magnetismInput = true;
            }
            else
            {
                magnetismInput = false;
            }

            // determine which input to turn off
            if (!_pullInput)
            {
                isPulling = false;
            }
            if (!_pushInput)
            {
                isPushing = false;
                if (selectedObject)
                {
                    selectedObject.useGravity = true;
                }
            }
        }

        private void PlayerInput()
        {
            _pushInput = InputManager.instance.PushInput;
            _pullInput = InputManager.instance.PullInput;
            _mousePos = Mouse.current.position.ReadValue();
        }

        private void LookMagnetism()
        {
            // function for look push / pull ability
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit))
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.red);
                if (hit.collider.CompareTag("Metal"))
                {
                    selectedObject = hit.rigidbody;
                    PushOrPull();
                }
            }
        }

        private void PushOrPull()
        {
            if (_pullInput)
            {
                isPulling = true;
            }
            else if (_pushInput)
            {
                isPushing = true;
            }
        }

        private void ApplyMagnesis()
        {
            if (magnetismInput && selectedObject)
            {
                // calculate a multipler based on how far away the selected object is from the player
                float pullMultiplier = Mathf.Lerp(0.3f, 2.75f, (maxDist - distToPlayer) / (maxDist - minDist));
                float pushMultiplier = Mathf.Lerp(0.2f, 1.2f, (maxDist - distToPlayer) / (maxDist - minDist));

                if (distToPlayer <= minDist && !isPushing)
                {
                    selectedObject.velocity = Vector3.zero;
                }
                if (isPulling)
                {
                    Debug.DrawRay(transform.position, ObjectPuller.objectDirection * 10, Color.red);
                    Vector3 pullDirection = -ObjectPuller.objectDirection;
                    pullDirection = new Vector3(pullDirection.x, pullDirection.y, pullDirection.z);

                    selectedObject.AddForce(pullDirection * pullForce);
                }
                else if (isPushing)
                {
                    selectedObject.useGravity = false;
                    
                    Debug.DrawRay(transform.position, ObjectPuller.objectDirection * 10, Color.red);
                    Vector3 pushDirection = ObjectPuller.objectDirection;
                    pushDirection = new Vector3(pushDirection.x, pushDirection.y, pushDirection.z);

                    selectedObject.AddForce(pushDirection * pullForce);

                }
            }
        }
    }
}