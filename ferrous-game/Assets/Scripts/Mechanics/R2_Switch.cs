using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R2_Switch : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject R1_Door_L;
    public GameObject R1_Door_R;
    private Vector3 direction = Vector3.forward;
    private float speed = 2f;
    private bool EndCollide = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(EndCollide == true && R1_Door_L.transform.position.z> 14 && R1_Door_R.transform.position.z<29)
        {
            R1_Door_L.transform.position -= direction * speed * Time.deltaTime;
            R1_Door_R.transform.position += direction * speed * Time.deltaTime;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.tag == "Metal")
        {
            ;
        }
        
    }
    void OnCollisionExit(Collision collision)
    {
        EndCollide = true;

    }
}
