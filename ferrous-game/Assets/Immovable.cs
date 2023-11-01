using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Immovable : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody _rigidbody;
    RaycastHit hit;

    [Header("InputChecks")]
    private bool _pushInput;
    private bool _pullInput;
    private Vector2 _mousePos;
    private Camera mainCamera;



    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        mainCamera = Camera.main;

    }



    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        if (_rigidbody.isKinematic)
        {
            if (_pushInput || _pullInput)
            {
                Debug.Log("getting input");
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(_mousePos);

                if (Physics.Raycast(ray, out hit, 10f))
                {
                    //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.red);
                    if (hit.collider.CompareTag("Metal"))
                    {
                        Debug.Log("hit metal");
                        _rigidbody.isKinematic = false;
                    }
                }
            }
        }
    }

    private void PlayerInput()
    {
        _pushInput = InputManager.instance.PushInput;
        _pullInput = InputManager.instance.PullInput;
        _mousePos = Mouse.current.position.ReadValue();
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            _rigidbody.isKinematic = true;
        }
        
    }

    private void OnCollisionExit(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            
            _rigidbody.isKinematic = false;
        }

    }
}
