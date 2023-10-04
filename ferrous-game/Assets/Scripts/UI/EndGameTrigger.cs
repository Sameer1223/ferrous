using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanelControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject EngTrigger;
    public GameObject BackPanel;
    public GameObject Player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        BackPanel.SetActive(true) ;
        Player.GetComponent<CameraController>().enabled = false;
        Player.GetComponent<PlayerController>().enabled = false;
        Cursor.lockState = CursorLockMode.None; // Lock the cursor to the center of the screen
        Cursor.visible = true; //
    }

}
