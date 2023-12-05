using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Ferrous
{
    public class StasisController : MonoBehaviour
    {

        private GameObject frozenObject;
        private Camera mainCamera;
        [SerializeField] private GameObject initialFrozenObject;

        public float maxSelectDist;
        public float unfreezeDuration;
        public static bool stasisColorOn = false;

        private Color purpleColour = new Color(10, 0, 191);


        private Scene scene;

        // input check
        private Vector2 _mousePos;
        private bool _stasisInput;

        [Header("SFX")] [SerializeField] private AudioSource stasisSFX;

        // Player model colour changing variables
        List<Renderer> modelRenderers = new List<Renderer>();

        // Start is called before the first frame update
        void Start()
        {
            mainCamera = Camera.main;
            scene = SceneManager.GetActiveScene();
            if (initialFrozenObject)
            {
                frozenObject = initialFrozenObject;
            }

            GameObject model = GameObject.Find("Robot");
            foreach (Renderer renderer in model.GetComponentsInChildren<Renderer>())
            {
                modelRenderers.Add(renderer);
            }
        }

        // Update is called once per frame
        void Update()
        {
            PlayerInput();
            Stasis();
        }

        private void PlayerInput()
        {
            _mousePos = Pointer.current.position.ReadValue();
            _stasisInput = InputManager.instance.StasisInput;
        }

        private void Stasis()
        {
            if (_stasisInput)
            {
                RaycastHit hit;
                // generates a ray in the look direction
                Ray ray = mainCamera.ScreenPointToRay(_mousePos);
                // instead of origin -> destination, use the defined ray
                if (Physics.Raycast(ray, out hit, maxSelectDist))
                {
                    if (hit.collider.CompareTag("Metal"))
                    {

                        if (!stasisSFX.isPlaying)
                        {
                            stasisSFX.Play();
                        }
                        GameObject prevFrozenObject;
                        // turn the player model to purple
                        StartCoroutine(SetPlayerStasisColour());

                        if (frozenObject != null)
                        {
                            // compare prev and new selected
                            prevFrozenObject = frozenObject.gameObject;
                            frozenObject = hit.rigidbody.gameObject;

                            // linked object
                            LinkedObjectManager.DisableLinkLine(frozenObject);
                            GameObject linkedObj = LinkedObjectManager.GetLinkedObject(frozenObject);
                            GameObject prevLinked = LinkedObjectManager.GetLinkedObject(prevFrozenObject);

                            // unstasis if you stasis an already frozen object (prevFrozen / prevLinked) 
                            if (ReferenceEquals(prevFrozenObject, frozenObject) || (prevLinked && ReferenceEquals(prevLinked, frozenObject)))
                            {
                                // turn off stasis of the current object after 2 seconds
                                StartCoroutine(UnFreeze(frozenObject));
                                prevFrozenObject = null;
                                frozenObject = null;
                                // turn off stasis for the link if it exists
                                if (linkedObj)
                                {
                                    StartCoroutine(UnFreeze(linkedObj));
                                    prevLinked = null;
                                    linkedObj = null;
                                }
                            }
                            else
                            {
                                // turn off stasis of the previous object & link after 2 secs
                                StartCoroutine("UnFreeze", prevFrozenObject);
                                if (prevLinked)
                                {
                                    StartCoroutine("UnFreeze", prevLinked);
                                }

                                // freeze the current object & linked

                                frozenObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                                SetOutlineColor(frozenObject, purpleColour);
                                if (linkedObj)
                                {
                                    linkedObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                                    SetOutlineColor(linkedObj, purpleColour);
                                }
                            }
                        }
                        else
                        {
                            // nothing is frozen, freeze what we just looked
                            frozenObject = hit.rigidbody.gameObject;
                            SetOutlineColor(frozenObject, purpleColour);
                            frozenObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                            LinkedObjectManager.LightenLinkLine(frozenObject);

                            // and freeze the linked object
                            GameObject linkedObj = LinkedObjectManager.GetLinkedObject(frozenObject);
                            if (linkedObj)
                            {
                                SetOutlineColor(linkedObj, purpleColour);
                                linkedObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                            }
                        }
                    }
                }
            }
        }

        private IEnumerator UnFreeze(GameObject toUnfreeze)
        {
            yield return new WaitForSeconds(unfreezeDuration);
            Rigidbody toUnfreezeRb = toUnfreeze.GetComponent<Rigidbody>();
            toUnfreezeRb.constraints = RigidbodyConstraints.FreezeRotation;
            if (toUnfreeze.name == "platforming block 1" || toUnfreeze.name == "platforming block 2")
            {
                toUnfreezeRb.constraints = RigidbodyConstraints.FreezeRotation |
                                           RigidbodyConstraints.FreezePositionZ;
            }

            // set colour back to white TODO: make this flash / some effect
            SetOutlineColor(toUnfreeze, toUnfreeze.GetComponent<Outline>().DefaultOutlineColor);

        }

        private IEnumerator SetPlayerStasisColour()
        {
            // Set player model colour to purple
            SetModelColour(purpleColour);
            stasisColorOn = true;

            yield return new WaitForSeconds(0.4f);
            stasisColorOn = false;
            SetModelColour(Color.white);
        }

        // Set player model colour
        private void SetModelColour(Color color)
        {
            foreach (Renderer item in modelRenderers)
            {
                item.material.SetColor("_EmissionColor", color);
            }
        }

        // set object outline colour
        private void SetOutlineColor(GameObject obj, Color color)
        {
            if (obj == null) return;
            Outline outline = obj.GetComponent<Outline>();
            outline.OutlineColor = color;
        }
    }
}
