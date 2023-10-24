using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
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

    [Header("InputChecks")]
    private bool _pushInput;
    private bool _pullInput;
    private Vector2 _mousePos;
    private bool _selectInput;
     

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
            PlayerInput();

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
            soundPlayed = false;
            if (!StasisController.stasisColorOn)
            {
                SetModelColour(Color.white);
            }
        }

        // determine which input to turn off
        if (!_pullInput)
        {
            isPulling = false;
        }
        if (!_pushInput)
        {
            isPushing = false;
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
        if (_pullInput)
        {
            isPulling = true;
            SetModelColour(Color.cyan);
        }
        else if (_pushInput)
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
        if (_selectInput)
        {
            RaycastHit hit;
            // generates a ray in the look direction
            Ray ray = mainCamera.ScreenPointToRay(_mousePos);
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

            linked = selectedObject.GetComponent<LinkedObject>();
            
            /* One axis movement code
            float dotX = Vector3.Dot(_playerTransform.forward, Vector3.right);
            float dotZ = Vector3.Dot(_playerTransform.forward, Vector3.forward);


            if (Mathf.Abs(dotZ) > Mathf.Abs(dotX))
                objectDirection = dotZ > 0 ? Vector3.forward : Vector3.back;
            else
                objectDirection = dotX > 0 ? Vector3.right : Vector3.left;
            */

            if (distToPlayer <= minDist && !isPushing)
            {
                selectedObject.velocity = Vector3.zero;
            }
            if (isPulling && distToPlayer > minDist)
            {
                /* Retiring this for multi axis movement
                Vector3 pullDirection = objectDirection;
                */

                Vector3 pullDirection = (_playerTransform.position - selectedObject.position).normalized;
                pullDirection = new Vector3(pullDirection.x, pullDirection.y, pullDirection.z);
                if (linked != null)
                {
                    linked.linkedObj.AddForce(pullDirection * pullForce * pullMultiplier);
                }
                selectedObject.AddForce(pullDirection * pullForce * pullMultiplier);
                SetModelColour(Color.cyan);
            }
            else if (isPushing)
            {
                /* Retiring this for one axis movement
                Vector3 pushDirection = -objectDirection;
                */

                Vector3 pushDirection = -(mainCamera.transform.position - selectedObject.position).normalized;
                pushDirection = new Vector3(pushDirection.x, pushDirection.y, pushDirection.z);
                if (linked != null)
                {
                    linked.linkedObj.AddForce(pushDirection * pullForce * pushMultiplier);
                }
                selectedObject.AddForce(pushDirection * pullForce * pushMultiplier);
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