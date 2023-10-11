using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class ObjectPuller : MonoBehaviour
{
    private bool isPulling = false;
    private bool isPushing = false;
    public AudioSource GunActiveSFX;

    private Rigidbody rb;
    private Camera mainCamera;
    private Transform _playerTransform;
    private bool soundPlayed;

    public bool useSelect = false; // bool that lets us choose which system we want to use when pushing / pulling

    [Header("Select")]
    public float rayDist;

    [Header("Push/Pull")]
    private float maxDist = 40f;
    private float minDist = 3.0f;
    public float pullForce = 50.0f;
    private Rigidbody selectedObject;
    private bool isSelected;
    private float distToPlayer;

    [Header("Stasis")]
    private GameObject frozenObject;

    // Player model colour changing variables
    List<Renderer> modelRenderers = new List<Renderer>();

    private Scene scene;

    private void Awake()
    {
        GameObject model = GameObject.Find("Robot_withUV");

        foreach (Renderer renderer in model.GetComponentsInChildren<Renderer>())
        {
            modelRenderers.Add(renderer);
        }
    }

    void Start()
    {
        scene = SceneManager.GetActiveScene();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        _playerTransform = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (useSelect)
        {
            selectObject();
        }
        if (isSelected || !useSelect)
        {
            getPushPullInput();
        }
        Stasis();

    }
    private void LateUpdate()
    {
        if (selectedObject)
        {
            if ((useSelect && isSelected) || !useSelect)
            {
                // calculate a multipler based on how far away the selected object is from the player
                float pullMultiplier = Mathf.Lerp(0.3f, 2.75f, (maxDist - distToPlayer) / (maxDist - minDist));
                float pushMultiplier = Mathf.Lerp(0.2f, 1.2f, (maxDist - distToPlayer) / (maxDist - minDist));

                if (distToPlayer <= 4.0f && !isPushing)
                {
                    selectedObject.velocity = Vector3.zero;
                }
                if (isPulling && distToPlayer > 4.0f)
                {
                    Vector3 pullDirection = (_playerTransform.position - selectedObject.position).normalized;
                    pullDirection = new Vector3(pullDirection.x, pullDirection.y, pullDirection.z);
                    selectedObject.AddForce(pullDirection * pullForce * pullMultiplier);
                    SetModelColour(Color.cyan);
                }
                else if (isPushing)
                {
                    Vector3 pushDirection = -(mainCamera.transform.position - selectedObject.position).normalized;
                    pushDirection = new Vector3(pushDirection.x, pushDirection.y, pushDirection.z);
                    selectedObject.AddForce(pushDirection * pullForce * pushMultiplier);
                    SetModelColour(Color.red);
                }
            }
        }
    }
    private void getPushPullInput()
    {
        if (Input.GetButton("Fire1") || Input.GetAxisRaw("Fire1") > 0 || Input.GetButton("Fire2") || Input.GetAxisRaw("Fire2") > 0)
        {
            // get the distance between the player and the selected object
            if (selectedObject)
            {
                distToPlayer = Vector3.Distance(_playerTransform.position, selectedObject.position);
                distToPlayer = Mathf.Clamp(distToPlayer, minDist, maxDist);
            }
            // if using look to push / pull then do the raycast logic
            if (!useSelect)
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Metal"))
                    {
                        if (Input.GetMouseButton(0) || Input.GetAxisRaw("Fire1") > 0.1f)
                        {
                            isPulling = true;
                            SetModelColour(Color.cyan);
                        }
                        else if (Input.GetMouseButton(1) || Input.GetAxisRaw("Fire2") > 0.1f)
                        {
                            isPushing = true;
                            SetModelColour(Color.red);
                        }
                        selectedObject = hit.rigidbody;
                    }
                }
            }
            else
            {
                if (Input.GetMouseButton(0) || Input.GetAxisRaw("Fire1") > 0.1f)
                {
                    isPulling = true;
                    SetModelColour(Color.cyan);
                }
                else if (Input.GetMouseButton(1) || Input.GetAxisRaw("Fire2") > 0.1f)
                {
                    isPushing = true;
                    SetModelColour(Color.red);
                }
            }


        }
        // determine if no longer inputting
        if (Input.GetAxisRaw("Fire1") < 0.1f && !Input.GetMouseButton(0))
        {
            isPulling = false;
            soundPlayed = false;
            SetModelColour(Color.white);
        }
        if (Input.GetAxisRaw("Fire2") < 0.1f && !Input.GetMouseButton(1))
        {
            isPushing = false;
            soundPlayed = false;
            SetModelColour(Color.white);
        }
    }

    private void Stasis()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RaycastHit hit;
            // generates a ray in the look direction
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            // instead of origin -> destination, use the defined ray
            if (Physics.Raycast(ray, out hit, rayDist))
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
        yield return new WaitForSeconds(0.3f);
        toUnfreeze.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        if (scene.name == "puzzle-structure-complete" || scene.name == "Varun Level")
        {
            toUnfreeze.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
        }

    }


    private void selectObject()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            // generates a ray in the look direction
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            // instead of origin -> destination, use the defined ray
            if (Physics.Raycast(ray, out hit, rayDist))
            {
                if (hit.collider.CompareTag("Metal"))
                {
                    GameObject prevSelectedObject;
                    if (selectedObject != null)
                    {
                        // compare prev and new selected
                        prevSelectedObject = selectedObject.gameObject;
                        selectedObject = hit.rigidbody;
                        if (GameObject.ReferenceEquals(prevSelectedObject, selectedObject.gameObject))
                        {
                            // de-select the object
                            selectedObject.useGravity = true;
                            selectedObject = null;
                            isSelected = false;
                        }
                        else
                        {
                            // turn gravity back on for the previous object
                            prevSelectedObject.GetComponent<Rigidbody>().useGravity = true;
                            // make the current object hover
                            selectedObject.useGravity = false;
                            selectedObject.AddForce(Vector3.up * 75f);
                        }
                    }
                    else
                    {
                        // no object was previously selected, select the current one
                        selectedObject = hit.rigidbody;
                        selectedObject.useGravity = false;
                        selectedObject.AddForce(Vector3.up * 75f);
                        isSelected = true;
                    }


                }
            }
        }
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