using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float mouseSensitivity = 2.0f;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor

        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Player Movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical) * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        Debug.Log(isGrounded);
        // Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }   
}