using Ferrous.Player;
using UnityEngine;

namespace Ferrous
{
    public class DoorCollision : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject BackPanel;
        public GameObject Player;
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void OnCollisionEnter(Collision c)
        {
            if (c.gameObject.tag == "Player")
            {
                BackPanel.SetActive(true) ;
                Player.GetComponent<CameraController>().enabled = false;
                Player.GetComponent<PlayerController>().enabled = false;
                Cursor.lockState = CursorLockMode.None; // Lock the cursor to the center of the screen
                Cursor.visible = true; //
            }
        }
    }
}
