using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Immovable : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody _rigidbody;
    RaycastHit hit;
    void Start()
    {
    _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_rigidbody.isKinematic)
        {
            if (Input.GetButton("Fire1") || Input.GetAxisRaw("Fire1") > 0 || Input.GetButton("Fire2") || Input.GetAxisRaw("Fire2") > 0)
            {
                
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
                {
                    //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.red);
                    if (hit.collider.CompareTag("Metal"))
                    {
                        _rigidbody.isKinematic = false;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            _rigidbody.isKinematic = true;
        }
        
    }

    private void OnCollisionExit(Collision other)
    {
        if (c.gameObject.tag == "Player")
        {
            
            _rigidbody.isKinematic = false;
        }

    }
}
