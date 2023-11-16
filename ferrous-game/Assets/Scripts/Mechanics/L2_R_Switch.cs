using UnityEngine;

namespace Ferrous.Mechanics
{
    public class L2_R_Switch : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject R1_Door_L;
        public GameObject R1_Door_R;
        public bool EndCollide2 = false;
        private Vector3 direction = Vector3.forward;
        private float speed = 1f;
        private bool EndCollide1 = false;
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if(EndCollide1 && EndCollide2 && R1_Door_L.transform.position.z> -4.5f && R1_Door_R.transform.position.z<3.1f)
            {
                R1_Door_L.transform.position -= direction * speed * Time.deltaTime;
                R1_Door_R.transform.position += direction * speed * Time.deltaTime;
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "Metal")
            {
                EndCollide1 = true;
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            EndCollide1 = false;
        }

        public void SetEndCollided2True()
        {
            EndCollide2 = true;
        }
    }
}
