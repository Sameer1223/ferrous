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

    [Header("Push/Pull")]
    public float pullForce = 10f;
    private Rigidbody selectedObject;
    private bool isSelected;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        _playerTransform = GameObject.Find("Player").transform;
    }

    void Update()
    {
        selectObject();
        if (isSelected)
        {
            getPushPullInput();
        }

    }
    private void LateUpdate()
    {
        if (isPulling && isSelected)
        {
            Debug.Log("Pulling");
            Vector3 pullDirection = (_playerTransform.position - selectedObject.position).normalized;
            pullDirection = new Vector3(pullDirection.x, 0f, pullDirection.z);
            selectedObject.GetComponent<Rigidbody>().AddForce(pullDirection * pullForce);
        }
        else if (isPushing && isSelected)
        {
            Debug.Log("Pushing");
            Vector3 pushDirection = -(mainCamera.transform.position - selectedObject.position).normalized;
            Debug.Log(pushDirection);
            Debug.DrawRay(mainCamera.transform.position, pushDirection, Color.red);
            pushDirection = new Vector3(pushDirection.x, 0f, pushDirection.z);
            selectedObject.GetComponent<Rigidbody>().AddForce(pushDirection * pullForce);
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
                        if (Object.ReferenceEquals(prevSelectedObject, selectedObject.gameObject))
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

    private void getPushPullInput()
    {
        if (Input.GetButton("Fire1") || Input.GetAxisRaw("Fire1") > 0 || Input.GetButton("Fire2") || Input.GetAxisRaw("Fire2") > 0)
        {
            if (Input.GetMouseButton(0) || Input.GetAxisRaw("Fire1") > 0.1f) { isPulling = true; Debug.Log("pulling"); }
            else if (Input.GetMouseButton(1) || Input.GetAxisRaw("Fire2") > 0.1f) { isPushing = true; }

            
        }

        if (Input.GetAxisRaw("Fire1") < 0.1f && !Input.GetMouseButton(0))
        {
            Debug.Log("not pulling");
            isPulling = false;
            soundPlayed = false;
        }
        if (Input.GetAxisRaw("Fire2") < 0.1f && !Input.GetMouseButton(1))
        {
            Debug.Log("not pushing");
            isPushing = false;
            soundPlayed = false;
        }
    }
}