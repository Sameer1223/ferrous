using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(EndCollide1 == true && EndCollide2 == true && R1_Door_L.transform.position.z> -4.5f && R1_Door_R.transform.position.z<3.1f)
        {
            R1_Door_L.transform.position -= direction * speed * Time.deltaTime;
            R1_Door_R.transform.position += direction * speed * Time.deltaTime;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.name == "R2_MetalBox")
        {
            EndCollide1 = true;

        }
        
    }
    void OnCollisionExit(Collision collision)
    {
        

    }

    public void SetEndCollided2True()
    {
        EndCollide2 = true;
    }
}
