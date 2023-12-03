using UnityEngine;

namespace Ferrous.Mechanics
{
    public class Door_Trigger : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject R1_Door_L;
        public GameObject R1_Door_R;
        public bool EndCollide2 = false;
        private Vector3 direction = Vector3.forward;
        private float speed = 1f;
        private bool EndCollide1 = false;
        [SerializeField] private AudioSource doorOpenSfx;
        private bool doorOpened;
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (EndCollide1 == true && R1_Door_L.transform.position.z > -4.5f && R1_Door_R.transform.position.z < 3.1f)
            {
                R1_Door_L.transform.position -= direction * speed * Time.deltaTime;
                R1_Door_R.transform.position += direction * speed * Time.deltaTime;

            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.name == "Player")
            {
                if (!doorOpened && !doorOpenSfx.isPlaying)
                {
                    doorOpenSfx.Play();
                }
                EndCollide1 = true;
                doorOpened = true;
            }
        }
    }
}
