using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name == "R1_MetalBox")
        {
            L2_R_Switch.EndCollide2 = true;
            Debug.Log("Collided R1");

        }

    }
}
