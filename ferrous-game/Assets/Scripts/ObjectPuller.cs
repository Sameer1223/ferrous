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
    private Transform interactableObject;
    private bool soundPlayed;

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
            if (!soundPlayed)
            {
                GunActiveSFX.Play();
                soundPlayed = true;
            }
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(transform.position, transform.forward, Color.green);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Metal"))
                {
                    if (Input.GetMouseButton(0)) { isPulling = true; }
                    else if (Input.GetMouseButton(1)) { isPushing = true;  }
                    interactableObject = hit.transform;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) { isPulling = false;
            soundPlayed = false;
        }
        if (Input.GetMouseButtonUp(1)) { isPushing = false;
            soundPlayed = false;
        }

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