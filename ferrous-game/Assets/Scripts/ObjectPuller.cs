using System.Collections;
using System.Collections.Generic;
using Ferrous.Blocks;
using Ferrous.UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Ferrous
{
    public class ObjectPuller : MonoBehaviour
    {
        private bool isPulling = false;
        private bool isPushing = false;

        private Rigidbody rb;
        private Camera mainCamera;
        private Transform _playerTransform;
        private Animator animator;


        [Header("Select")]
        private float rayDist;
        public bool useSelect = false; // bool that lets us choose which system we want to use when pushing / pulling


        [Header("Push/Pull")]
        public float maxDist;
        private float minDist = 0.5f;
        private float stopPullingDist;
        public float pullForce = 50.0f;
        private Rigidbody selectedObject;
        private Vector3 selectedObjectSize;
        private float distToPlayer;
        private bool magnetismInput;
        private LinkedObject linked;
        public static Vector3 objectDirection;

        [Header("InputChecks")]
        private bool _pushInput;
        private bool _pullInput;
        private Vector2 _mousePos;
        private bool _selectInput;
        
        [Header("SFX")]
        [SerializeField] private AudioSource gunActiveSfx;
        [SerializeField] private AudioSource pullSfx;
        [SerializeField] private AudioSource pushSfx;
        public float increaseVolDelay;
        public float magnesisTargetVol;
        public float magnesisStartVol;
        private bool pushSfxPlayed;
        private bool pullSfxPlayed;
        public float increaseSpeed = 0.075f;
        private IEnumerator runningCoroutine;
        private float volBeforePausing;

     
        // Player model colour changing variables
        List<Renderer> modelRenderers = new List<Renderer>();

        private void Awake()
        {
            GameObject model = GameObject.Find("Robot");

            foreach (Renderer renderer in model.GetComponentsInChildren<Renderer>())
            {
                modelRenderers.Add(renderer);
            }
        }

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            mainCamera = Camera.main;
            GameObject player = GameObject.Find("Player");
            _playerTransform = player.transform;
            animator = player.GetComponentInChildren<Animator>();
            magnetismInput = false;
            pullSfx.volume = magnesisStartVol;
            pushSfx.volume = magnesisStartVol;
            volBeforePausing = 0f;
        }

        void Update()
        {
            if (!PauseMenu.IsPaused)
            {
                PlayerInput();
                
                // unpause any sfx
                pullSfx.UnPause();
                pushSfx.UnPause();

                // determine if the player is inputting a push / pull
                GetMagnetismInput();
                // if the player has selected an object / currently push pulling a target
                if (selectedObject)
                {
                    CalculateDistFromPlayer(selectedObject);
                }
                if (useSelect)
                {
                    {
                        //selectObject();
                        // if the player is inputting push/pull, push / pull the selected object
                        if (magnetismInput)
                        {
                            SelectMagnetism();
                        }
                    }
                }
                else
                {
                    if (magnetismInput)
                    {
                        LookMagnetism();
                    }
                }
            }
            else
            {
                // pause any sfx that might be playing
                if (pullSfx.isPlaying)
                {
                    pullSfx.Pause();
                } else if (pushSfx.isPlaying)
                {
                    pushSfx.Pause();
                }
            }
        }
        private void FixedUpdate()
        {
            if (!PauseMenu.IsPaused)
            {
                ApplyMagnesis();
            }
        }
    

        private void PlayerInput()
        {
            _pushInput = InputManager.instance.PushInput;
            _pullInput = InputManager.instance.PullInput;
            _mousePos = Mouse.current.position.ReadValue();
            if (useSelect) { _selectInput = InputManager.instance.SelectInput; }
        }

        private void GetMagnetismInput()
        {
            if (_pushInput || _pullInput)
            {
                magnetismInput = true;
            } else
            {
                magnetismInput = false;
                selectedObjectSize = Vector3.zero;
                if (!StasisController.stasisColorOn)
                {
                    SetModelColour(Color.white);
                }
            }

            animator.SetBool("isInteracting", magnetismInput);

            // determine which input to turn off
            if (!_pullInput)
            {
                StopCoroutine(PlayMagnesisSfx(pullSfx));
                StopCoroutine(IncreaseVolumeOverTime(pullSfx));
                isPulling = false;
                // reset pull sfx
                pullSfx.Stop();
                pullSfx.volume = magnesisStartVol;
                pullSfxPlayed = false;
            }
            if (!_pushInput)
            {
                StopCoroutine(PlayMagnesisSfx(pushSfx));
                StopCoroutine(IncreaseVolumeOverTime(pushSfx));
                isPushing = false;
                // reset push sfx
                pushSfx.Stop();
                pushSfx.volume = magnesisStartVol;

                if (selectedObject)
                {
                    selectedObject.useGravity = true;
                }

                pushSfxPlayed = false;
            }
        }

        private void LookMagnetism()
        {
            // function for look push / pull ability
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(_mousePos);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Metal"))
                {
                    selectedObject = hit.rigidbody;
                    if (selectedObjectSize == Vector3.zero)
                    {
                        selectedObjectSize = hit.rigidbody.GetComponent<Collider>().bounds.size;
                        stopPullingDist = selectedObjectSize.x >= selectedObjectSize.z
                            ? selectedObjectSize.x 
                            : selectedObjectSize.z;
                    }
                    
                    LinkedObjectManager.LightenLinkLine(selectedObject.gameObject);
                    PushOrPull();
                }
            }
        }

        private void PushOrPull()
        {
            if (_pullInput)
            {
                isPulling = true;
                SetModelColour(Color.cyan);
                if (!pullSfxPlayed)
                {
                    StopAllCoroutines();
                    pullSfxPlayed = true;
                    gunActiveSfx.Play();
                    runningCoroutine = PlayMagnesisSfx(pullSfx);
                    StartCoroutine(runningCoroutine) ;
                }
                

            }
            else if (_pushInput)
            {
                isPushing = true;
                SetModelColour(Color.red);
                if (!pushSfxPlayed)
                {
                    StopAllCoroutines();
                    pushSfxPlayed = true;
                    gunActiveSfx.Play();
                    runningCoroutine = PlayMagnesisSfx(pushSfx);
                    StartCoroutine(runningCoroutine);
                }
            }
        }

        private IEnumerator PlayMagnesisSfx(AudioSource magnesisSfx)
        {
            // wait for the activate sound to play
            yield return new WaitForSeconds(increaseVolDelay);
            // start playing the audio source
            magnesisSfx.Play();
            runningCoroutine = IncreaseVolumeOverTime(magnesisSfx);
            StartCoroutine(runningCoroutine);
        }

        private IEnumerator IncreaseVolumeOverTime(AudioSource magnesisSfx)
        {
            while (magnesisSfx.volume < magnesisTargetVol)
            {
                // increase volume
                magnesisSfx.volume += increaseSpeed * Time.deltaTime;

                yield return null;
            }
            // avoid rounding issues
            magnesisSfx.volume = magnesisTargetVol;

        }
        

        private void CalculateDistFromPlayer(Rigidbody secondObject)
        {
            distToPlayer = Vector3.Distance(_playerTransform.position, secondObject.position);
            distToPlayer = Mathf.Clamp(distToPlayer, minDist, maxDist);
        }


        private void SelectMagnetism()
        {
            PushOrPull();
        }

        //private void selectObject()
        //{
        //    if (_selectInput)
        //    {
        //        RaycastHit hit;
        //        // generates a ray in the look direction
        //        Ray ray = mainCamera.ScreenPointToRay(_mousePos);
        //        // instead of origin -> destination, use the defined ray
        //        if (Physics.Raycast(ray, out hit, rayDist))
        //        {
        //            if (hit.collider.CompareTag("Metal"))
        //            {
        //                GameObject prevSelectedObject;
        //                if (selectedObject != null)
        //                {
        //                    // compare prev and new selected
        //                    prevSelectedObject = selectedObject.gameObject;
        //                    selectedObject = hit.rigidbody;
        //                    if (GameObject.ReferenceEquals(prevSelectedObject, selectedObject.gameObject))
        //                    {
        //                        // de-select the object
        //                        selectedObject.useGravity = true;
        //                        selectedObject = null;
        //                    }
        //                    else
        //                    {
        //                        // turn gravity back on for the previous object
        //                        prevSelectedObject.GetComponent<Rigidbody>().useGravity = true;
        //                        // make the current object hover
        //                        selectedObject.useGravity = false;
        //                        selectedObject.AddForce(Vector3.up * 75f);
        //                    }
        //                }
        //                else
        //                {
        //                    // no object was previously selected, select the current one
        //                    selectedObject = hit.rigidbody;
        //                    selectedObject.useGravity = false;
        //                    selectedObject.AddForce(Vector3.up * 75f);
        //                }
        //            }
        //        }
        //    }
        //}


        /// <summary>
        /// Function that applies the magnetic force to the selectedObject.
        /// </summary>
        private void ApplyMagnesis()
        {
            if (magnetismInput && selectedObject)
            {
                // calculate a multipler based on how far away the selected object is from the player
                float pullMultiplier = Mathf.Lerp(0.3f, 2.5f, (maxDist - distToPlayer) / (maxDist - minDist));
                float pushMultiplier = Mathf.Lerp(0.2f, 1.5f, (maxDist - distToPlayer) / (maxDist - minDist));

                GameObject linkedObj = LinkedObjectManager.GetLinkedObject(selectedObject.gameObject);

                /* One axis movement code
            float dotX = Vector3.Dot(_playerTransform.forward, Vector3.right);
            float dotZ = Vector3.Dot(_playerTransform.forward, Vector3.forward);


            if (Mathf.Abs(dotZ) > Mathf.Abs(dotX))
                objectDirection = dotZ > 0 ? Vector3.forward : Vector3.back;
            else
                objectDirection = dotX > 0 ? Vector3.right : Vector3.left;
            */

                // if (distToPlayer <= minDist && !isPushing)
                // {
                //     selectedObject.velocity = Vector3.zero;
                // }
                if (isPulling && distToPlayer < maxDist)
                {
                    /* Retiring this for multi axis movement
                Vector3 pullDirection = objectDirection;
                */
                    Vector3 pullDirection = (_playerTransform.position - selectedObject.position).normalized;
                    pullDirection = new Vector3(pullDirection.x, pullDirection.y, pullDirection.z);
                    if (distToPlayer <= stopPullingDist)
                    {
                        selectedObject.velocity = Vector3.zero;
                    } else
                    {
                        selectedObject.AddForce(pullDirection * pullForce * pullMultiplier);

                    }

                    GetInteractDirectionNormalized(pullDirection);
                    if (linkedObj != null)
                    {
                        linkedObj.GetComponent<Rigidbody>().AddForce(pullDirection * pullForce * pullMultiplier);
                    }

                    SetModelColour(Color.cyan);
                }
                else if (isPushing && distToPlayer < maxDist)
                {
                    // make the object not fall while pushing
                    selectedObject.useGravity = false;
                    
                    /* Retiring this for one axis movement
                Vector3 pushDirection = -objectDirection;
                */

                    Vector3 pushDirection = -(mainCamera.transform.position - selectedObject.position).normalized;
                    pushDirection = new Vector3(pushDirection.x, pushDirection.y, pushDirection.z);

                    GetInteractDirectionNormalized(pushDirection);

                    selectedObject.AddForce(pushDirection * pullForce * pushMultiplier);
                    if (linkedObj != null)
                    {
                        linkedObj.GetComponent<Rigidbody>().AddForce(pushDirection * pullForce * pushMultiplier);
                    }
                    SetModelColour(Color.red);
                }
            }
        }

        private void GetInteractDirectionNormalized(Vector3 direction)
        {
            float dotX = Vector3.Dot(direction, Vector3.right);
            float dotZ = Vector3.Dot(direction, Vector3.forward);

            Vector3 nearestAxisDirection;

            if (Mathf.Abs(dotZ) > Mathf.Abs(dotX))
                nearestAxisDirection = dotZ > 0 ? Vector3.forward : Vector3.back;
            else
                nearestAxisDirection = dotX > 0 ? Vector3.right : Vector3.left;

            RaycastHit hit;
            Ray ray = new Ray(_playerTransform.position, Vector3.down);
            if (Physics.Raycast(ray, out hit, 2 * 0.5f + 0.2f))
            {
                if (hit.collider.CompareTag("Metal"))
                {
                    nearestAxisDirection = Vector3.down;
                }

            }
            objectDirection = nearestAxisDirection;
        }

        // Set player model colour
        private void SetModelColour(Color color)
        {
            foreach (Renderer item in modelRenderers)
            {
                item.material.SetColor("_EmissionColor", color);
            }
        }
    }
}