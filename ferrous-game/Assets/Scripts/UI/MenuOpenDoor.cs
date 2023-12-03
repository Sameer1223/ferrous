using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ferrous
{
    public class MenuOpenDoor : MonoBehaviour
    {
        [SerializeField] private GameObject R1_Door_L;
        [SerializeField] private GameObject R1_Door_R;
        private bool _collision = false;
        private Vector3 direction = Vector3.left;
        private float speed = 1f;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (_collision == true )
            {
                R1_Door_L.transform.position -= direction * speed * Time.deltaTime;
                R1_Door_R.transform.position += direction * speed * Time.deltaTime;
            }
        }

        private void OnTriggerEnter(Collider other)
        {

            if(other.tag == "MainCamera")
            {
                _collision = true;
                //Debug.Log(_collision);
                
            }
        }
    }
}
