using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ferrous
{
    public class RampAid : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        private Collider playerCollider;

        public float aidForce;
        // Start is called before the first frame update
        void Start()
        {
           playerCollider = player.GetComponent<Collider>();
           Physics.IgnoreCollision(GetComponent<Collider>(), playerCollider);
        }

        private void OnCollisionStay(Collision other)
        {
            if (other.gameObject.tag == "Metal")
            {
                Debug.Log("Asdadsad");
                other.gameObject.GetComponent<Rigidbody>().AddForce(0, 0, -aidForce);
            }
        }
        
    }
}
