using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    public Transform followTarget;

    private Rigidbody rb;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        Debug.Log(horizontalInput);
        Debug.Log(verticalInput);

    }

    private void FixedUpdate()
    {
        
        moveDirection = followTarget.forward * verticalInput + followTarget.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

    }
}
