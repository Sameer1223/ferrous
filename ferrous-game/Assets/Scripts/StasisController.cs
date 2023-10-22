using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StasisController : MonoBehaviour
{

    private GameObject frozenObject;
    private Camera mainCamera;

    public float maxSelectDist;
    public float unfreezeDuration;

    private Scene scene;

    // input check
    private Vector2 _mousePos;
    private bool _stasisInput;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        scene = SceneManager.GetActiveScene();

    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        Stasis();
    }

    private void PlayerInput()
    {
        _mousePos = Mouse.current.position.ReadValue();
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
                    if (frozenObject != null)
                    {
                        // compare prev and new selected
                        prevFrozenObject = frozenObject.gameObject;
                        frozenObject = hit.rigidbody.gameObject;
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
                        }
                    }
                    else
                    {
                        // nothing is frozen, freeze what we just looked at
                        frozenObject = hit.rigidbody.gameObject;
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

    }
}
