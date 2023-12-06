using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ferrous
{
    public class CameraZoneCollision : MonoBehaviour
    {
        public bool isCollided = false;

        // Start is called before the first frame updates
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    
        void OnTriggerEnter(Collider other){
            if (other.tag == "Player"){
                isCollided = true;
               
            }
        }

        void OnTriggerExit(Collider other){
            if (other.tag == "Player"){
                isCollided = false;
            }
        }
    }
}
