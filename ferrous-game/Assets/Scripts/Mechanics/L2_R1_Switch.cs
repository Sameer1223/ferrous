using UnityEngine;

namespace Ferrous.Mechanics
{
    public class L2_R1_Switch : MonoBehaviour
    {
        // Start is called before the first frame update
        public L2_R_Switch L2_R_Switch;
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

    
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "Metal")
            {
                L2_R_Switch.EndCollide2 = true;
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            L2_R_Switch.EndCollide2 = false;
        }
    }
}
