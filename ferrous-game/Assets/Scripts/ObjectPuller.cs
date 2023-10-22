using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPuller : MonoBehaviour
{
    private bool isPulling = false;
    private bool isPushing = false;
    public AudioSource GunActiveSFX;

    private Rigidbody rb;
    private Camera mainCamera;
    private Transform _playerTransform;
    private bool soundPlayed;


    [Header("Select")]
    public float rayDist;
    public bool useSelect = false; // bool that lets us choose which system we want to use when pushing / pulling


    [Header("Push/Pull")]
    private float maxDist = 40f;
    private float minDist = 5.0f;
    public float pullForce = 50.0f;
    private Rigidbody selectedObject;
    private float distToPlayer;
    private bool magnetismInput;
    private LinkedObject linked;
    private Vector3 objectDirection;

    // Player model colour changing variables
    List<Renderer> modelRenderers = new List<Renderer>();


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
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        _playerTransform = GameObject.Find("Player").transform;
        magnetismInput = false;
    }

    void Update()
    {
        if (!PauseMenu.IsPaused)
        {
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
                    selectObject();
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
    }
    private void LateUpdate()
    {
        ApplyMagnesis();
    }

    private void GetMagnetismInput()
    {
        if (Input.GetButton("Fire1") || Input.GetAxisRaw("Fire1") > 0 || Input.GetButton("Fire2") || Input.GetAxisRaw("Fire2") > 0)
        {
            magnetismInput = true;
        } else
        {
            magnetismInput = false;
            soundPlayed = false;
            if (!StasisController.stasisColorOn)
            {
                SetModelColour(Color.white);
            }
        }

        // determine which input to turn off
        if (Input.GetAxisRaw("Fire1") < 0.1f && !Input.GetMouseButton(0))
        {
            isPulling = false;
        }
        if (Input.GetAxisRaw("Fire2") < 0.1f && !Input.GetMouseButton(1))
        {
            isPushing = false;
        }
    }

    private void LookMagnetism()
    {
        // function for look push / pull ability
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Metal"))
            {
                selectedObject = hit.rigidbody;
                PushOrPull();
            }
        }
    }

    private void SelectMagnetism()
    {
        PushOrPull();
    }

    private void PushOrPull()
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

    private void CalculateDistFromPlayer(Rigidbody secondObject)
    {
        distToPlayer = Vector3.Distance(_playerTransform.position, secondObject.position);
        distToPlayer = Mathf.Clamp(distToPlayer, minDist, maxDist);
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
                    }
                }
            }
        }
    }

    private void ApplyMagnesis()
    {
        if (magnetismInput && selectedObject)
        {
            // calculate a multipler based on how far away the selected object is from the player
            float pullMultiplier = Mathf.Lerp(0.3f, 2.75f, (maxDist - distToPlayer) / (maxDist - minDist));
            float pushMultiplier = Mathf.Lerp(0.2f, 1.2f, (maxDist - distToPlayer) / (maxDist - minDist));

            float dotX = Vector3.Dot(_playerTransform.forward, Vector3.right);
            float dotZ = Vector3.Dot(_playerTransform.forward, Vector3.forward);

            linked = selectedObject.GetComponent<LinkedObject>();

            if (Mathf.Abs(dotZ) > Mathf.Abs(dotX))
                objectDirection = dotZ > 0 ? Vector3.forward : Vector3.back;
            else
                objectDirection = dotX > 0 ? Vector3.right : Vector3.left;

            if (distToPlayer <= minDist && !isPushing)
            {
                selectedObject.velocity = Vector3.zero;
            }
            if (isPulling && distToPlayer > minDist)
            {
                /* Retiring this for one axis movement
                Vector3 pullDirection = (_playerTransform.position - selectedObject.position).normalized;
                */

                Vector3 pullDirection = objectDirection;
                pullDirection = new Vector3(pullDirection.x, pullDirection.y, pullDirection.z);
                if (linked != null)
                {
                    linked.linkedObj.AddForce(-pullDirection * pullForce * pullMultiplier);
                }
                selectedObject.AddForce(-pullDirection * pullForce * pullMultiplier);
                SetModelColour(Color.cyan);
            }
            else if (isPushing)
            {
                /* Retiring this for one axis movement
                Vector3 pushDirection = -(mainCamera.transform.position - selectedObject.position).normalized;
                */

                Vector3 pushDirection = -objectDirection;
                pushDirection = new Vector3(pushDirection.x, pushDirection.y, pushDirection.z);
                if (linked != null)
                {
                    linked.linkedObj.AddForce(-pushDirection * pullForce * pushMultiplier);
                }
                selectedObject.AddForce(-pushDirection * pullForce * pushMultiplier);
                SetModelColour(Color.red);
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