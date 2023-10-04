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

    public bool useSelect = false; // bool that lets us choose which system we want to use when pushing / pulling

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
        if (useSelect)
        {
            selectObject();

        }
        if (isSelected || !useSelect)
        {
            getPushPullInput();
        }

    }
    private void LateUpdate()
    {
        if ((useSelect && isSelected) || !useSelect)
        {
            if (isPulling)
            {
                Vector3 pullDirection = (_playerTransform.position - selectedObject.position).normalized;
                pullDirection = new Vector3(pullDirection.x, pullDirection.y, pullDirection.z);
                selectedObject.AddForce(pullDirection * pullForce);
                SetBlue();
            }
            else if (isPushing)
            {
                Vector3 pushDirection = -(mainCamera.transform.position - selectedObject.position).normalized;
                Debug.DrawRay(mainCamera.transform.position, pushDirection, Color.red);
                pushDirection = new Vector3(pushDirection.x, 0f, pushDirection.z);
                selectedObject.AddForce(pushDirection * pullForce);
                SetRed();
            }
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
            // if using look to push / pull then do the raycast logic
            if (!useSelect)
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(transform.position, transform.forward, Color.green);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Metal"))
                    {
                        Debug.Log(Input.GetMouseButton(0));
                        if (Input.GetMouseButton(0) || Input.GetAxisRaw("Fire1") > 0.1f)
                        {
                            isPulling = true;
                            SetBlue();
                        }
                        else if (Input.GetMouseButton(1) || Input.GetAxisRaw("Fire2") > 0.1f)
                        {
                            isPushing = true;
                            SetRed();
                        }
                        selectedObject = hit.rigidbody;
                    }
                }
            }
            else
            {
                if (Input.GetMouseButton(0) || Input.GetAxisRaw("Fire1") > 0.1f)
                {
                    isPulling = true;
                    SetBlue();
                }
                else if (Input.GetMouseButton(1) || Input.GetAxisRaw("Fire2") > 0.1f)
                {
                    isPushing = true;
                    SetRed();
                }
            }

            
        }
        // determine if no longer inputting
        if (Input.GetAxisRaw("Fire1") < 0.1f && !Input.GetMouseButton(0))
        {
            Debug.Log("not pulling");
            isPulling = false;
            soundPlayed = false;
            SetWhite();
        }
        if (Input.GetAxisRaw("Fire2") < 0.1f && !Input.GetMouseButton(1))
        {
            Debug.Log("not pushing");
            isPushing = false;
            soundPlayed = false;
            SetWhite();
        }
    }

    public void SetBlue()
    {
        GameObject body = GameObject.FindGameObjectWithTag("body");
        GameObject face = GameObject.FindGameObjectWithTag("face");
        GameObject hand = GameObject.FindGameObjectWithTag("hand");
        GameObject helmet = GameObject.FindGameObjectWithTag("helmet");
        Renderer faceRend = face.GetComponent<Renderer>();
        faceRend.material.SetColor("_EmissionColor", Color.cyan);
        
        Renderer handRend = hand.GetComponent<Renderer>();
        handRend.material.SetColor("_EmissionColor", Color.cyan);
        
        Renderer bodyRend = body.GetComponent<Renderer>();
        bodyRend.material.SetColor("_EmissionColor", Color.cyan);
        
        Renderer helmetRend = helmet.GetComponent<Renderer>();
        helmetRend.material.SetColor("_EmissionColor", Color.cyan);
    }
    
    public void SetRed()
    {
        GameObject body = GameObject.FindGameObjectWithTag("body");
        GameObject face = GameObject.FindGameObjectWithTag("face");
        GameObject hand = GameObject.FindGameObjectWithTag("hand");
        GameObject helmet = GameObject.FindGameObjectWithTag("helmet");
        
        Renderer faceRend = face.GetComponent<Renderer>();
        faceRend.material.SetColor("_EmissionColor", Color.red);
        
        Renderer handRend = hand.GetComponent<Renderer>();
        handRend.material.SetColor("_EmissionColor", Color.red);
        
        Renderer bodyRend = body.GetComponent<Renderer>();
        bodyRend.material.SetColor("_EmissionColor", Color.red);
        
        Renderer helmetRend = helmet.GetComponent<Renderer>();
        helmetRend.material.SetColor("_EmissionColor", Color.red);
    }
    
    public void SetWhite()
    {
        GameObject body = GameObject.FindGameObjectWithTag("body");
        GameObject face = GameObject.FindGameObjectWithTag("face");
        GameObject hand = GameObject.FindGameObjectWithTag("hand");
        GameObject helmet = GameObject.FindGameObjectWithTag("helmet");
        
        Renderer faceRend = face.GetComponent<Renderer>();
        faceRend.material.SetColor("_EmissionColor", Color.white);
        
        Renderer handRend = hand.GetComponent<Renderer>();
        handRend.material.SetColor("_EmissionColor", Color.white);
        
        Renderer bodyRend = body.GetComponent<Renderer>();
        bodyRend.material.SetColor("_EmissionColor", Color.white);
        
        Renderer helmetRend = helmet.GetComponent<Renderer>();
        helmetRend.material.SetColor("_EmissionColor", Color.white);
    }
}