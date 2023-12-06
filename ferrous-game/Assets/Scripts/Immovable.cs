using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ferrous
{
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
        
        [Header("grounded check")]
        private float height;
        [SerializeField] private bool isGrounded;
        [SerializeField] LayerMask whatIsGround;

        
        private Color stasisColor = new Color(10, 0, 191);
        



        void Start()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            mainCamera = Camera.main;
            height = gameObject.GetComponent<Collider>().bounds.size.y;

        }



        // Update is called once per frame
        void Update()
        {
            PlayerInput();
            IsGrounded();


            if ((_pushInput) && !_isAbove)
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(_mousePos);
            
                if (Physics.Raycast(ray, out hit, 10f))
                {
                    if (hit.collider.CompareTag("Metal"))
                    {
                        if (gameObject.GetComponent<Outline>().OutlineColor != stasisColor)
                        {
                            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                            if (gameObject.name == "platforming block 1" || gameObject.name == "platforming block 2")
                            {
                                _rigidbody.constraints = RigidbodyConstraints.FreezeRotation |
                                                         RigidbodyConstraints.FreezePositionZ;
                            }
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
        
        private void IsGrounded()
        {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, height * 0.5f + 0.2f, whatIsGround);
        }

        private void OnCollisionEnter(Collision c)
        {
            if (c.gameObject.tag == "Player")
            {
                FreezeBlock();
            }
        }

        private void OnCollisionStay(Collision c)
        {
            if (c.gameObject.tag == "Player")
            {
                FreezeBlock();
            }
        }

        private void OnCollisionExit(Collision c)
        {
            if (c.gameObject.tag == "Player")
            {
                UnfreezeBlock();
            }

        }

        private void OnTriggerEnter(Collider c)
        {
            if (c.gameObject.tag == "Player")
            {
                _isAbove = true;
            }
        }

        private void OnTriggerStay(Collider c)
        {
            if (c.gameObject.tag == "Player")
            {
                FreezeBlock();
            }
        }

        private void OnTriggerExit(Collider c)
        {
            if (c.gameObject.tag == "Player")
            {
                UnfreezeBlock();
                _isAbove = false;
        
            }
        }

        void FreezeBlock()
        {
            if (isGrounded)
            {
                _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
            else if (!isGrounded && gameObject.GetComponent<Outline>().OutlineColor != stasisColor)
            {
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotation |
                                         RigidbodyConstraints.FreezePositionX |
                                         RigidbodyConstraints.FreezePositionZ;
            }
        }

        void UnfreezeBlock()
        {
            Debug.Log("un freeze");
            if (gameObject.GetComponent<Outline>().OutlineColor != stasisColor)
            {
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                if (gameObject.name == "platforming block 1" || gameObject.name == "platforming block 2")
                {
                    _rigidbody.constraints = RigidbodyConstraints.FreezeRotation |
                                             RigidbodyConstraints.FreezePositionZ;
                }
            }
        }
    }
}