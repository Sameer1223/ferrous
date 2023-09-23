using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPuller : MonoBehaviour
{
    private bool isPulling = false;
    private Rigidbody rb;
    private Camera mainCamera;
    private Transform pulledObject;

    public float pullForce = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // Change this to your desired input method
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(transform.position, transform.forward, Color.green);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hit");
                if (hit.collider.CompareTag("metal"))
                {
                    isPulling = true;
                    pulledObject = hit.transform;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isPulling = false;
        }

        if (isPulling)
        {
            Debug.Log("Pulling");
            Vector3 pullDirection = (mainCamera.transform.position - pulledObject.position).normalized;
            pulledObject.GetComponent<Rigidbody>().AddForce(pullDirection * pullForce);
        }
    }
}