using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Rays;

public class StasisController : MonoBehaviour
{

    private GameObject frozenObject;
    private Camera mainCamera;

    public float maxSelectDist;
    public float unfreezeDuration;
    public static bool stasisColorOn = false;

    private Color purpleColour = new Color(10, 0, 191);


    private Scene scene;

    // input check
    private Vector2 _mousePos;
    private bool _stasisInput;

    // Player model colour changing variables
    List<Renderer> modelRenderers = new List<Renderer>();

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        scene = SceneManager.GetActiveScene();

        GameObject model = GameObject.Find("Robot_withUV");
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
                    GameObject prevFrozenObject;
                    // turn the player model to purple
                    StartCoroutine(SetPlayerStasisColour());

                    if (frozenObject != null)
                    {
                        // compare prev and new selected
                        prevFrozenObject = frozenObject.gameObject;
                        frozenObject = hit.rigidbody.gameObject;
                        LinkedObjectManager.DisableLinkLine(hit.rigidbody);
                        if (GameObject.ReferenceEquals(prevFrozenObject, frozenObject))
                        {
                            // turn off stasis of the current object after 2 seconds
                            StartCoroutine(UnFreeze(frozenObject));
                            prevFrozenObject = null;
                            frozenObject = null;
                        }
                        else
                        {
                            // turn off stasis of the previous object after 2 secs
                            StartCoroutine("UnFreeze", prevFrozenObject);
                            // freeze the current object
                            frozenObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                            SetOutlineColor(frozenObject, purpleColour);
                        }
                    }
                    else
                    {
                        // nothing is frozen, freeze what we just looked at
                        frozenObject = hit.rigidbody.gameObject;
                        LinkedObjectManager.DisableLinkLine(hit.rigidbody);
                        SetOutlineColor(frozenObject, purpleColour);
                        frozenObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    }
                }
            }
        }
    }

    private IEnumerator UnFreeze(GameObject toUnfreeze)
    {
        yield return new WaitForSeconds(unfreezeDuration);
        toUnfreeze.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        if (scene.name == "Varun Level"  || scene.name == "puzzle-structure-complete")
        {
            toUnfreeze.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
        }

        // set colour back to white TODO: make this flash / some effect
        SetOutlineColor(toUnfreeze, Color.white);

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
