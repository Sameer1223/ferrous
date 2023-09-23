using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPuller : MonoBehaviour
{
    private bool isPulling = false;
    private bool isPushing = false;

    private Rigidbody rb;
    private Camera mainCamera;
    private Transform interactableObject;

    public float pullForce = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) // Change this to your desired input method
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(transform.position, transform.forward, Color.green);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hit");
                if (hit.collider.CompareTag("Metal"))
                {
                    if (Input.GetMouseButton(0)) { isPulling = true; }
                    else if (Input.GetMouseButton(1)) { isPushing = true;  }
                    interactableObject = hit.transform;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) { isPulling = false; }
        if (Input.GetMouseButtonUp(1)) { isPushing = false; }

        if (isPulling)
        {
            Debug.Log("Pulling");
            Vector3 pullDirection = (mainCamera.transform.position - interactableObject.position).normalized;
            interactableObject.GetComponent<Rigidbody>().AddForce(pullDirection * pullForce);
        }   
        else if (isPushing)
        {
            Debug.Log("Pushing");
            Vector3 pullDirection = -(mainCamera.transform.position - interactableObject.position).normalized;
            interactableObject.GetComponent<Rigidbody>().AddForce(pullDirection * pullForce);
        }
    }
}