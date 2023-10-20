using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDemoCode : MonoBehaviour
{
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            Debug.Log("B");
            rb.AddForce(100.0f * transform.up);
        }
    }
}
