using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerController : MonoBehaviour
{
    // Inputs
    private float moveVertical;
    private bool jumpInput;
    private Vector2 moveVector;

    // Game Objects
    private Rigidbody rb;
    private Camera gameCamera;

    // Movement variables
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public float groundDrag;
    private bool canJump;
    [SerializeField] private float _fallMultiplier = 1.25f;
    [SerializeField] private float _jumpVelocityFalloff = 1.4f;

    // Ground check variables
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool isGrounded;

    // Audio
    [Header("Sound Effects")] 
    public AudioSource jumpSfx;
    public AudioSource walkSfx;

    [Header("Input")]

    // [TESTING]
    Vector3 lastPosition;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
       
        rb = gameObject.GetComponent<Rigidbody>();
        gameCamera = gameObject.GetComponentInChildren<Camera>();

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
            rb.drag = groundDrag * 0.75f;
    }

    void FixedUpdate()
    {
        // Player movement
        MovePlayer();

    }

    // User input
    private void PlayerInput()
    {
        moveVector = InputManager.instance.MovementInput;
        jumpInput = InputManager.instance.JumpInput;
        if (jumpInput && canJump && isGrounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    // Player movement
    private void MovePlayer()
    {
        Vector3 moveDirection = transform.forward * moveVector.y + transform.right * moveVector.x;

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
        if (isGrounded)
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

        // fall faster
        if (rb.velocity.y < _jumpVelocityFalloff)
        {
            rb.velocity += (Vector3.up * Physics.gravity.y * _fallMultiplier * Time.deltaTime);
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