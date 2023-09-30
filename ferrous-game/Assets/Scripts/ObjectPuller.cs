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
                    Debug.Log("Selected Metal");
                }
            }
        }

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) // Change this to your desired input method
        if (Input.GetButton("Fire1") || Input.GetAxisRaw("Fire1") > 0 || Input.GetButton("Fire2") || Input.GetAxisRaw("Fire2") > 0)
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
                    Debug.Log(Input.GetMouseButton(0));
                    if (Input.GetMouseButton(0) || Input.GetAxisRaw("Fire1") > 0.1f) { isPulling = true; }
                    else if (Input.GetMouseButton(1) || Input.GetAxisRaw("Fire2") > 0.1f) { isPushing = true; }
                    interactableObject = hit.transform;
                }
            }
        }

        if (Input.GetAxisRaw("Fire1") < 0.1f && !Input.GetMouseButton(0)) 
        { 
            isPulling = false;
            soundPlayed = false;
        } 
        if (Input.GetAxisRaw("Fire2") < 0.1f && !Input.GetMouseButton(1)) 
        { 
            isPushing = false;
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