using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Pool;

public class PowerBlocks : MonoBehaviour
{

    private bool isPulling = false;
    private bool isPushing = false;

    [Header("Push/Pull")]
    private float maxDist = 40f;
    private float minDist = 5.0f;
    public float pullForce = 50.0f;
    private Rigidbody selectedObject;
    private float distToPlayer;
    private bool magnetismInput;
    private Vector3 objectDirection;

    void Update()
    {
        if (!PauseMenu.IsPaused)
        {
            // determine if the player is inputting a push / pull
            GetMagnetismInput();
            if (magnetismInput)
            {
                LookMagnetism();
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
        }
        else
        {
            magnetismInput = false;
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

        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            Debug.DrawLine(transform.position, -transform.up * 10);
            if (hit.collider.CompareTag("Metal"))
            {
                selectedObject = hit.rigidbody;
                PushOrPull();
            }
        }
    }

    private void PushOrPull()
    {
        if (Input.GetMouseButton(0) || Input.GetAxisRaw("Fire1") > 0.1f)
        {
            isPulling = true;
        }
        else if (Input.GetMouseButton(1) || Input.GetAxisRaw("Fire2") > 0.1f)
        {
            isPushing = true;
        }
    }

    private void ApplyMagnesis()
    {
        if (magnetismInput && selectedObject)
        {
            // calculate a multipler based on how far away the selected object is from the player
            float pullMultiplier = Mathf.Lerp(0.3f, 2.75f, (maxDist - distToPlayer) / (maxDist - minDist));
            float pushMultiplier = Mathf.Lerp(0.2f, 1.2f, (maxDist - distToPlayer) / (maxDist - minDist));

            float dotX = Vector3.Dot(transform.forward, Vector3.right);
            float dotZ = Vector3.Dot(transform.forward, Vector3.forward);

            if (Mathf.Abs(dotZ) > Mathf.Abs(dotX))
                objectDirection = dotZ > 0 ? Vector3.forward : Vector3.back;
            else
                objectDirection = dotX > 0 ? Vector3.right : Vector3.left;

            if (distToPlayer <= minDist && !isPushing)
            {
                selectedObject.velocity = Vector3.zero;
            }
            if (isPulling)
            {
                Vector3 pullDirection = objectDirection;
                pullDirection = new Vector3(pullDirection.x, pullDirection.y, pullDirection.z);

                selectedObject.AddForce(pullDirection * pullForce);
            }
            else if (isPushing)
            {
                Vector3 pushDirection = -objectDirection;
                pushDirection = new Vector3(pushDirection.x, pushDirection.y, pushDirection.z);

                selectedObject.AddForce(pushDirection * pullForce);

            }
        }
    }
}