using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateCheck : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject R1_Door_L;
    public GameObject R1_Door_R;
    private Vector3 direction = Vector3.forward;
    private float speed = 1f;
    private Transform movableBlockTransform;
    private Transform linkedBlockTransform;
    void Start()
    {
        movableBlockTransform = transform.GetChild(0).GetComponent<Transform>();
        linkedBlockTransform = transform.GetChild(1).GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if( (movableBlockTransform.localPosition.x >= 12 && movableBlockTransform.localPosition.x <= 14)  
            && (movableBlockTransform.localPosition.z >= 8 && movableBlockTransform.localPosition.z <= 10.5)
            && (linkedBlockTransform.localPosition.x >= 6.5 && linkedBlockTransform.localPosition.x <= 10.5)  
            && (linkedBlockTransform.localPosition.z >= -8.9 && linkedBlockTransform.localPosition.z <= -4.5)
            && R1_Door_L.transform.localPosition.z> -4.5f && R1_Door_R.transform.localPosition.z<3.1f)
        {   
            R1_Door_L.transform.position -= direction * speed * Time.deltaTime;
            R1_Door_R.transform.position += direction * speed * Time.deltaTime;
        }
    }
}
