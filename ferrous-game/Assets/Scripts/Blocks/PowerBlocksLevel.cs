using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using static Rays;

public class PowerBlocksLevel : MonoBehaviour
{

    private bool isPulling = false;
    private bool isPushing = false;

    private Transform playerTransform;

    [Header("PowerObjects")]
    public Rigidbody HitObject;

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
            PushOrPull();
            if (magnetismInput)
            {

                ApplyMagnesis();
            }
        }
    }
    private void LateUpdate()
    {
        //ApplyMagnesis();
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

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.red);
            if (hit.collider.CompareTag("Metal"))
            {
                selectedObject = hit.rigidbody;
                Debug.Log(selectedObject.name);
                PushOrPull();
            }
        }
    }

    private void PushOrPull()
    {
        if (_pullInput)
        {
            isPulling = true;
            Debug.Log("ispulling");
        }
        else if (_pushInput)
        {
            isPushing = true;
        }
    }

    private void ApplyMagnesis()
    {
        if (magnetismInput)
        {

            // calculate a multipler based on how far away the selected object is from the player
            if (isPulling)
            {
                Debug.Log("working");
                //Debug.DrawRay(transform.position, ObjectPuller.objectDirection * 10, Color.red);
                //Vector3 pullDirection = -ObjectPuller.objectDirection;
                Vector3 pullDirection = new Vector3(1f, 0f, 0f);

                HitObject.AddForce(pullDirection * pullForce);
                
            }
            else if (isPushing)
            {
                //Debug.DrawRay(transform.position, ObjectPuller.objectDirection * 10, Color.red);
                //Vector3 pushDirection = ObjectPuller.objectDirection;
                Vector3 pushDirection = new Vector3(-1f, 0f, 0f);

                HitObject.AddForce(pushDirection * pullForce);

            }
        }
    }
}