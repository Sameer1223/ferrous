using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision c)
    {
        // Does the other collider have the tag "Player"?
        if (c.collider.tag == "Player")
        {
            // restart the level (respawn)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("restarted level");
        }
    }
}
