using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PowerBlockRays : MonoBehaviour
{
    [Header("Ray Logic")]
    public LineRenderer magnetRay;
    public Transform raySpawnPoint;
    public float maxLength;

    private Camera cam;
    private GameObject outlinedGameObj;

    // Keeps track of force being applied to object
    private Force force;

    [Header("Constants")]
    private Color blueColour = new Color(0, 191, 156);
    private Color redColour = new Color(191, 0, 0);
    private Color purpleColour = new Color(10, 0, 191);

    [Header("Materials")]
    public Material blueMaterial;
    public Material redMaterial;
    public Material purpleMaterial;

    [Header("InputChecks")]
    private bool _pushInput;
    private bool _pullInput;
    private Vector2 _mousePos;
    private bool _stasisInput;

    // Force enum to keep track of force modes
    public enum Force
    {
        Pull, //0
        Push, //1
        Stasis, //2
        None //3
    }

    private void Awake()
    {
        magnetRay.enabled = false;
        cam = Camera.main;
    }

    private void PlayerInput()
    {
        _pushInput = InputManager.instance.PushInput;
        _pullInput = InputManager.instance.PullInput;
        _stasisInput = InputManager.instance.StasisInput;
    }

    // Turn on magnetic ray
    private void Activate(Force force)
    {
        Debug.Log("Activates");

        if (force == Force.None) return;

        if (force == Force.Pull) magnetRay.material = blueMaterial;
        else if (force == Force.Push) magnetRay.material = redMaterial;
        else if (force == Force.Stasis) magnetRay.material = purpleMaterial;

        magnetRay.enabled = true;
    }

    // Turn off magnetic ray
    private void Deactivate()
    {
        magnetRay.enabled = false;
        magnetRay.SetPosition(0, raySpawnPoint.position);
        magnetRay.SetPosition(1, raySpawnPoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.IsPaused)
        {
            PlayerInput();
            // Integer to track force push or pull. Pull = 0, Push = 1, No Force = -1
            force = Force.None;

            if (_pullInput) force = Force.Push;
            else if (_pushInput) force = Force.Pull;
            else if (_stasisInput) force = Force.Stasis;
            else if (!_pullInput && !_pushInput && !_stasisInput) Deactivate();

            // Calculate ray logic
            RaycastHit hit;
            bool cast = Physics.Raycast(transform.position, ObjectPuller.objectDirection, out hit);
            Vector3 hitPosition = cast ? hit.point : raySpawnPoint.position + raySpawnPoint.forward * maxLength;

            if (hit.collider != null && !hit.collider.gameObject.CompareTag("Metal"))
            {
                Deactivate();
                return;
            }
            // Activate ray with force integer (push / pull / stasis)
            Activate(force);

            if (!magnetRay.enabled) return;

            Debug.Log("Rays here");

            // Line renderer set positions
            magnetRay.SetPosition(0, raySpawnPoint.position);
            magnetRay.SetPosition(1, hitPosition);
        }
        // TODO: MAKE THE STASIS RAY LAST LONGER INSTEAD OF DOING DOWN
    }
}
