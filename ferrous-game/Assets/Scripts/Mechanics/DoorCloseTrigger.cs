using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloseTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject R1_Door_L;
    public GameObject R1_Door_R;
    public bool EndCollide2 = false;
    private Vector3 direction = Vector3.forward;
    private float speed = 1f;
    private bool EndCollide1 = false;
    public GameObject PreviousTrigger;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (EndCollide1 == true && R1_Door_L.transform.position.z < -0.79f && R1_Door_R.transform.position.z > -0.79f)
        {
            R1_Door_L.transform.position += direction * speed * Time.deltaTime;
            R1_Door_R.transform.position -= direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EndCollide1 = true;
            PreviousTrigger.SetActive(false);
        }
    }
}
