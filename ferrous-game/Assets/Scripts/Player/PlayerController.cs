using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

namespace Ferrous.Player
{
    public class PlayerController : MonoBehaviour
    {
        // Inputs
        private float moveVertical;
        private bool jumpInput;
        private Vector2 moveVector;

        // Game Objects
        private Rigidbody rb;
        [SerializeField] private Camera gameCamera;
        private Animator animator;

        // Movement variables
        [Header("Movement")]
        public float moveSpeed;
        public float jumpForce;
        public float airMultiplier;
        public float groundDrag;
        private Vector3 moveDirection;
        [SerializeField] private float _fallMultiplier = 1.25f;
        [SerializeField] private float _jumpVelocityFalloff = 1.4f;
        

        // Ground check variables
        [Header("Ground Check")]
        public float playerHeight;
        public LayerMask whatIsGround;
        public bool isGrounded;
        
        [Header("Coyote Time")]
        [SerializeField] private float _coyoteTime = 0.2f;
        private float _coyoteTimeCounter;

        [Header("Jump Buffering")]
        [SerializeField] private float _jumpBufferTime = 0.2f;
        private float _jumpBufferCounter;

        [Header("Slope Handling")] [SerializeField]
        private float maxSlopeAngle;
        public bool onSlope;
        private RaycastHit slopeHit;
        private bool exitingSlope;

        // Audio
        [Header("Sound Effects")]
        [SerializeField] private AudioSource jumpSfx;
        [SerializeField] private AudioSource walkSfx;

  

        // [TESTING]
        Vector3 lastPosition;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
            Cursor.visible = false; // Hide the cursor
       
            rb = gameObject.GetComponent<Rigidbody>();
            animator = gameObject.GetComponentInChildren<Animator>();

            lastPosition = transform.position;
            
        }

        void Update()
        {
            PlayerInput();
            SpeedLimiter();
            IsGrounded();


            // Drag & coyote time
            if (isGrounded)
            {
                rb.drag = groundDrag;
                _coyoteTimeCounter = _coyoteTime;
            }
            else
            {
                rb.drag = groundDrag * 0.9f;
                _coyoteTimeCounter -= Time.deltaTime;
            }
            

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

            animator.SetFloat("horizontalMovement", moveVector.x);
            animator.SetFloat("verticalMovement", moveVector.y);

            if (jumpInput)
            {
                _jumpBufferCounter = _jumpBufferTime;
            }
            else
            {
                _jumpBufferCounter -= Time.deltaTime;
            }
            if (_jumpBufferCounter > 0 && _coyoteTimeCounter > 0f)
            {
                Jump();
                _coyoteTimeCounter = 0f;
                _jumpBufferCounter = 0f;
            }

        }

        // Player movement
        private void MovePlayer()
        {
            Vector3 moveDirection = gameCamera.transform.forward * moveVector.y + gameCamera.transform.right * moveVector.x;
            moveDirection = new Vector3(moveDirection.x, 0f, moveDirection.z);

            moveDirection.Normalize();

            onSlope = OnSlope();
            if (onSlope && !exitingSlope)
            {
                rb.AddForce(GetSlopeMoveDirection() * (moveSpeed * 20f), ForceMode.Force);
                // keep player on slope
                if (rb.velocity.y > 0)
                {
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                }
            }

            if (isGrounded)
                rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);

            else if (!isGrounded)
                rb.AddForce(moveDirection * moveSpeed * 10f * airMultiplier, ForceMode.Force);


            // turn off gravity when the player is on a slope
            rb.useGravity = !onSlope;

            // walkSfx
            if (moveDirection != Vector3.zero && !walkSfx.isPlaying && isGrounded)
            {
                walkSfx.Play();
            }
        }
        // Limiting function for speed
        private void SpeedLimiter()
        {
            if (onSlope && !exitingSlope)
            {
                if (rb.velocity.magnitude > moveSpeed)
                {
                    rb.velocity = rb.velocity.normalized * moveSpeed;
                }
            }
            else if (isGrounded)
            {
                Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                if (flatVel.magnitude > moveSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * moveSpeed;
                    rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
                }
            }

            // fall faster
            if (rb.velocity.y < _jumpVelocityFalloff && !onSlope)
            {
                rb.velocity += (Vector3.up * Physics.gravity.y * _fallMultiplier * Time.deltaTime);
            }

        }

        // Jump handler
        private void Jump()
        {
            exitingSlope = true;
            Debug.Log("jump");
        
            // Set rb y velocity to 0 so jump remains consistent
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpSfx.Play();
        }

        // Ground check
        private void IsGrounded()
        {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        }

        private bool OnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.2f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }

            return false;
        }

        private Vector3 GetSlopeMoveDirection()
        {
            return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
        }
        
    }
}