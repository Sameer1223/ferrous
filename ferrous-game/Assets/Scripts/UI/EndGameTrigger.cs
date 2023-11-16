using Ferrous.Player;
using UnityEngine;

namespace Ferrous.UI
{
    public class GamePanelControl : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject EndTrigger;
        public GameObject BackPanel;
        public GameObject PlayerContainer;
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public void Continue()
        {
            BackPanel.SetActive(false);
            PlayerContainer.GetComponentInChildren<CameraController>().enabled = true;
            PlayerContainer.GetComponentInChildren<PlayerController>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
            Cursor.visible = false; //
            transform.gameObject.SetActive(false);
        }

        public void OnTriggerEnter(Collider other)
        {
            BackPanel.SetActive(true) ;
            PlayerContainer.GetComponentInChildren<CameraController>().enabled = false;
            PlayerContainer.GetComponentInChildren<PlayerController>().enabled = false;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None; // Lock the cursor to the center of the screen
            Cursor.visible = true; //
        }
    

    }
}
