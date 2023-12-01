using Ferrous.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ferrous.Blocks
{
    public class PowerBlocks : MonoBehaviour
    {
        
        public GameObject inputMetal;
        private bool isPulling = false;
        private bool isPushing = false;

        private Transform playerTransform;

        [Header("Push/Pull")]
        private float maxDist = 40f;
        private float minDist = 5.0f;
        public float pullForce = 50.0f;
        private Rigidbody selectedObject;
        private float distToPowerBlock;
        private bool magnetismInput;
        private Vector3 objectDirection;

        [Header("InputChecks")]
        private bool _pushInput;
        private bool _pullInput;
        private Vector2 _mousePos;
        private bool _selectInput;
        private bool inputMetalHit;
        private bool inputMetalForceActedUpon;
        private Transform inputMetalTransform;

        private float distance;
        private Vector3 heading;
        private Vector3 direction;



        private void Start()
        {
            playerTransform = GameObject.Find("Player").transform;
            inputMetalForceActedUpon = inputMetal.GetComponent<Outline>().forceActedUpon;
            inputMetalTransform = inputMetal.GetComponent<Transform>();
        }


        void Update()
        {
            inputMetalForceActedUpon = inputMetal.GetComponent<Outline>().forceActedUpon;
            if (!PauseMenu.IsPaused)
            {
                
                PlayerInput();
                // determine if the player is inputting a push / pull
                GetMagnetismInput();
                if (magnetismInput && inputMetalHit)
                {
                    LookMagnetism();
                }
            }
        }

        private void GetMagnetismInput()
        {
            if (_pushInput || _pullInput)
            {
                magnetismInput = true;
                if (inputMetalForceActedUpon) {
                    inputMetalHit = true;
                }
                else {
                    inputMetalHit = false;
                }
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
            heading = inputMetalTransform.position - playerTransform.position;
            distance = heading.magnitude;
            direction = heading / distance; // This is now the normalized direction.
            direction = GetInteractDirectionNormalized(direction);
            Debug.DrawRay(transform.position, direction * 10, Color.red);
            if (Physics.Raycast(transform.position, direction, out hit))
            {   
                if (hit.collider.CompareTag("Metal"))
                {
                    selectedObject = hit.rigidbody;
                    CalculateDistFromPlayer(selectedObject);
                    PushOrPull();
                    ApplyMagnesis();
                } else {
                    selectedObject = null;
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
                float pullMultiplier = Mathf.Lerp(0.3f, 2.75f, (maxDist - distToPowerBlock) / (maxDist - minDist));
                float pushMultiplier = Mathf.Lerp(0.2f, 1.2f, (maxDist - distToPowerBlock) / (maxDist - minDist));

                if (distToPowerBlock <= minDist && !isPushing)
                {
                    selectedObject.velocity = Vector3.zero;
                }
                if (isPulling)
                {
                    Vector3 pullDirection = -direction;
                    pullDirection = new Vector3(pullDirection.x, pullDirection.y, pullDirection.z);

                    selectedObject.AddForce(pullDirection * pullForce);
                }
                else if (isPushing)
                {
                    selectedObject.useGravity = false;
                    Vector3 pushDirection = direction;
                    pushDirection = new Vector3(pushDirection.x, pushDirection.y, pushDirection.z);

                    selectedObject.AddForce(pushDirection * pullForce);

                }
                selectedObject = null;
            }
        }

        private Vector3 GetInteractDirectionNormalized(Vector3 direction)
        {
            float dotX = Vector3.Dot(direction, Vector3.right);
            float dotZ = Vector3.Dot(direction, Vector3.forward);

            Vector3 nearestAxisDirection;

            if (Mathf.Abs(dotZ) > Mathf.Abs(dotX))
                nearestAxisDirection = dotZ > 0 ? Vector3.forward : Vector3.back;
            else
                nearestAxisDirection = dotX > 0 ? Vector3.right : Vector3.left;

            RaycastHit hit;
            Ray ray = new Ray(playerTransform.position, Vector3.down);
            if (Physics.Raycast(ray, out hit, 2 * 0.5f + 0.2f))
            {
                if (hit.collider.CompareTag("Metal"))
                {
                    nearestAxisDirection = Vector3.down;
                }

            }
            return nearestAxisDirection;
        }

         private void CalculateDistFromPlayer(Rigidbody secondObject)
        {
            distToPowerBlock = Vector3.Distance(transform.position, secondObject.position);
            distToPowerBlock = Mathf.Clamp(distToPowerBlock, minDist, maxDist);
        }
    }
}