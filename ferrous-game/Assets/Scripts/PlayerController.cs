using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Inputs
    private float moveHorizontal;
    private float moveVertical;
    private bool jumpInput;

    // Game Objects
    private Rigidbody rb;
    private Camera camera;
    private Animator animator;
    private Transform character;

    // Movement variables
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public float groundDrag;
    private bool canJump;

    // Ground check variables
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool isGrounded;

    // Audio
    [Header("Sound Effects")] 
    public AudioSource jumpSfx;
    public AudioSource walkSfx;

    // [TESTING]
    Vector3 lastPosition;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
       
        rb = gameObject.GetComponent<Rigidbody>();
        camera = gameObject.GetComponentInChildren<Camera>();
        animator = gameObject.GetComponentInChildren<Animator>();
        character = transform.Find("Character");

        canJump = true;
        lastPosition = transform.position;
    }

    void Update()
    {
        PlayerInput();
        SpeedLimiter();
        IsGrounded();

        // Drag
        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    void FixedUpdate()
    {
        // Player movement
        MovePlayer();
    }
    
    // User input
    private void PlayerInput()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        animator.SetFloat("horizontalMovement", moveHorizontal);
        animator.SetFloat("verticalMovement", moveVertical);

        if (Input.GetButton("Jump") && canJump && isGrounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    // Player movement
    private void MovePlayer()
    {
        Vector3 moveDirection = transform.forward * moveVertical + transform.right * moveHorizontal;

        if (isGrounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if (!isGrounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // Music
        if (moveDirection != Vector3.zero && !walkSfx.isPlaying)
        {
            walkSfx.Play();
        }
    }

    // Limiting function for speed
    private void SpeedLimiter()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    // Jump handler
    private void Jump()
    {
        // Set rb y velocity to 0 so jump remains consistent
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        
        jumpSfx.Play();
    }

    private void ResetJump()
    {
        canJump = true;
    }

    // Ground check
    private void IsGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
    }
    
}