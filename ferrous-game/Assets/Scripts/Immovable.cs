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
    public bool _isAbove;

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

        if ((_pushInput) && !_isAbove)
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(_mousePos);

            if (Physics.Raycast(ray, out hit, 10f))
            {
                if (hit.collider.CompareTag("Metal"))
                {
                    Debug.Log("fwapojqwopfj");

                    _rigidbody.isKinematic = false;
                    //_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;


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
            Debug.Log("enter player collision");
            _rigidbody.isKinematic = true;

            //c.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

    }

    private void OnCollisionExit(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            Debug.Log("exit player collision");
            _rigidbody.isKinematic = false;

            //_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        }

    }

    //private void OnTriggerEnter(Collider c)
    //{
    //    if (c.gameObject.tag == "Player")
    //    {
    //        _isAbove = true;

    //    }
    //}

    //private void OnTriggerExit(Collider c)
    //{
    //    if (c.gameObject.tag == "Player")
    //    {
    //        _isAbove = false;

    //    }
    //}
}