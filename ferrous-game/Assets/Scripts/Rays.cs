using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Rays : MonoBehaviour
{
    [Header("Ray Logic")]
    public LineRenderer magnetRay;
    public Transform raySpawnPoint;
    public Material[] rayMaterials;
    public float maxLength;

    private Camera cam;
    private GameObject outlinedGameObj;
    private int force;

    // Constants
    private Color blueColour = new Color(0, 191, 156); 
    private Color redColour = new Color(191, 0, 0);

    private void Awake()
    {
        magnetRay.enabled = false;
        rayMaterials = magnetRay.materials;
        cam = Camera.main;
    }

    // Turn on magnetic ray
    private void Activate(int force, GameObject obj)
    {
        if (force == -1) return;
        magnetRay.material = rayMaterials[force];
        magnetRay.enabled = true;

        // Show outline on target object
        //ShowOutline(obj, force);
    }

    // Turn off magnetic ray
    private void Deactivate(GameObject obj)
    {
        // Hide outline on target object
        //HideOutline(obj);

        magnetRay.enabled = false;
        magnetRay.SetPosition(0, raySpawnPoint.position);
        magnetRay.SetPosition(1, raySpawnPoint.position);
    }

    // Outline logic for objects
    private void ShowOutline(GameObject obj, int force)
    {
        outlinedGameObj = obj;
        Outline outline = obj.GetComponent<Outline>();
        if (force == 0) outline.OutlineColor = blueColour; else outline.OutlineColor = redColour;
        outline.enabled = true;
    }

    // Hiding outline after being deselected
    private void HideOutline(GameObject obj)
    {
        Outline outline = obj.GetComponent<Outline>();
        outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        force = -1;

        if (Input.GetMouseButton(0) || Input.GetAxisRaw("Fire1") > 0.1f) force = 0;
        else if (Input.GetMouseButton(1) || Input.GetAxisRaw("Fire2") > 0.1f) force = 1;
        else if (!Input.GetMouseButton(0) && Input.GetAxisRaw("Fire1") < 0.1f || !Input.GetMouseButton(1) && Input.GetAxisRaw("Fire2") < 0.1f) Deactivate(outlinedGameObj);
        

        // Calculate ray logic
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        bool cast = Physics.Raycast(ray, out RaycastHit hit, maxLength);
        Vector3 hitPosition = cast ? hit.point : raySpawnPoint.position + raySpawnPoint.forward * maxLength;


        Activate(force, hit.collider.gameObject);

        if (!magnetRay.enabled) return;
        
        // Line renderer set positions
        magnetRay.SetPosition(0, raySpawnPoint.position);
        magnetRay.SetPosition(1, hitPosition);
    }
}
