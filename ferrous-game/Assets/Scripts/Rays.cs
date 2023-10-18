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
    
    // Keeps track of force being applied to object
    private Force force;

    // Constants
    private Color blueColour = new Color(0, 191, 156); 
    private Color redColour = new Color(191, 0, 0);

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
        rayMaterials = magnetRay.materials;
        cam = Camera.main;
    }

    // Turn on magnetic ray
    private void Activate(Force force, GameObject obj)
    {
        if (force == Force.None) return;
        magnetRay.material = rayMaterials[(int) force];
        magnetRay.enabled = true;

        // Show outline on target object
        ShowOutline(obj, force);
    }

    // Turn off magnetic ray
    private void Deactivate(GameObject obj)
    {
        if (obj == null) return;

        // Hide outline on target object
        HideOutline(obj);

        magnetRay.enabled = false;
        magnetRay.SetPosition(0, raySpawnPoint.position);
        magnetRay.SetPosition(1, raySpawnPoint.position);
    }

    // Outline logic for objects
    private void ShowOutline(GameObject obj, Force force)
    {
        if(obj == null) return;

        outlinedGameObj = obj;
        Outline outline = obj.GetComponent<Outline>();
        if (force == Force.Pull) outline.OutlineColor = blueColour; else outline.OutlineColor = redColour;
    }

    // Hiding outline after being deselected
    private void HideOutline(GameObject obj)
    {
        if (obj == null) return;

        Outline outline = obj.GetComponent<Outline>();
        outline.OutlineColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.IsPaused)
        {
            // Integer to track force push or pull. Pull = 0, Push = 1, No Force = -1
            force = Force.None;

            if (Input.GetMouseButton(0) || Input.GetAxisRaw("Fire1") > 0.1f) force = Force.Pull;
            else if (Input.GetMouseButton(1) || Input.GetAxisRaw("Fire2") > 0.1f) force = Force.Push;
            else if (!Input.GetMouseButton(0) && Input.GetAxisRaw("Fire1") < 0.1f || !Input.GetMouseButton(1) && Input.GetAxisRaw("Fire2") < 0.1f) Deactivate(outlinedGameObj);


            // Calculate ray logic
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            bool cast = Physics.Raycast(ray, out RaycastHit hit, maxLength);
            Vector3 hitPosition = cast ? hit.point : raySpawnPoint.position + raySpawnPoint.forward * maxLength;

            if (hit.collider.gameObject != null && !hit.collider.gameObject.CompareTag("Metal"))
            {
                Deactivate(outlinedGameObj);
                return;
            }

            // Activate ray with force integer (either push or pull)
            Activate(force, hit.collider.gameObject);

            if (!magnetRay.enabled) return;

            // Line renderer set positions
            magnetRay.SetPosition(0, raySpawnPoint.position);
            magnetRay.SetPosition(1, hitPosition);
        }
       
    }
}
