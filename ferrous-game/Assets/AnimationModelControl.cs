using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationModelControl : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject idle_animation;
    GameObject forward_animation;
    GameObject backward_animation;
    GameObject left_animation;
    GameObject right_animation;
    GameObject idle_handraise_animation;
    GameObject forward_handraise_animation;
    GameObject backward_handraise_animation;
    GameObject left_handraise_animation;
    GameObject right_handraise_animation;
    GameObject forward_jump_animation;
    GameObject left_jump_animation ;
    GameObject right_jump_animation ;
    void Start()
    {
         idle_animation = gameObject.transform.GetChild(0).gameObject;
        forward_animation = gameObject.transform.GetChild(1).gameObject;
        backward_animation = gameObject.transform.GetChild(2).gameObject;
        left_animation = gameObject.transform.GetChild(3).gameObject;
        right_animation = gameObject.transform.GetChild(4).gameObject;
        idle_handraise_animation = gameObject.transform.GetChild(5).gameObject;
        forward_handraise_animation = gameObject.transform.GetChild(6).gameObject;
       backward_handraise_animation = gameObject.transform.GetChild(7).gameObject;
        left_handraise_animation = gameObject.transform.GetChild(8).gameObject;
         right_handraise_animation = gameObject.transform.GetChild(9).gameObject;
        forward_jump_animation = gameObject.transform.GetChild(10).gameObject;
        left_jump_animation = gameObject.transform.GetChild(11).gameObject;
        right_jump_animation = gameObject.transform.GetChild(12).gameObject;
        idle_animation.SetActive(true);
        forward_animation.SetActive(false);
        backward_animation.SetActive(false);
        left_animation.SetActive(false);
        right_animation.SetActive(false);
        idle_handraise_animation.SetActive(false);
        forward_handraise_animation.SetActive(false);
        backward_handraise_animation.SetActive(false);
        left_handraise_animation.SetActive(false);
        right_handraise_animation.SetActive(false);
        forward_jump_animation.SetActive(false);
        left_jump_animation.SetActive(false);
        right_jump_animation.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //right jump
        if (Input.GetAxis("Horizontal") > 0.1 && Input.GetButton("Jump"))
        {
            idle_animation.SetActive(false);
            forward_animation.SetActive(false);
            backward_animation.SetActive(false);
            left_animation.SetActive(false);
            right_animation.SetActive(false);
            idle_handraise_animation.SetActive(false);
            forward_handraise_animation.SetActive(false);
            backward_handraise_animation.SetActive(false);
            left_handraise_animation.SetActive(false);
            right_handraise_animation.SetActive(false);
            forward_jump_animation.SetActive(false);
            left_jump_animation.SetActive(false);
            right_jump_animation.SetActive(true);
        }
        //left jump
        else if (Input.GetAxis("Horizontal") < -0.01 && Input.GetButton("Jump"))
        {
            idle_animation.SetActive(false);
            forward_animation.SetActive(false);
            backward_animation.SetActive(false);
            left_animation.SetActive(false);
            right_animation.SetActive(false);
            idle_handraise_animation.SetActive(false);
            forward_handraise_animation.SetActive(false);
            backward_handraise_animation.SetActive(false);
            left_handraise_animation.SetActive(false);
            right_handraise_animation.SetActive(false);
            forward_jump_animation.SetActive(false);
            left_jump_animation.SetActive(true);
            right_jump_animation.SetActive(false);
        }
        //forward jump
        else if (Input.GetAxis("Vertical") > 0.1 && Input.GetButton("Jump"))
        {
            idle_animation.SetActive(false);
            forward_animation.SetActive(false);
            backward_animation.SetActive(false);
            left_animation.SetActive(false);
            right_animation.SetActive(false);
            idle_handraise_animation.SetActive(false);
            forward_handraise_animation.SetActive(false);
            backward_handraise_animation.SetActive(false);
            left_handraise_animation.SetActive(false);
            right_handraise_animation.SetActive(false);
            forward_jump_animation.SetActive(true);
            left_jump_animation.SetActive(false);
            right_jump_animation.SetActive(false);
        }
        //backward jump (JUST A FORWARD Jump)
        else if (Input.GetAxis("Vertical") < -0.01 && Input.GetButton("Jump"))
        {
            idle_animation.SetActive(false);
            forward_animation.SetActive(false);
            backward_animation.SetActive(false);
            left_animation.SetActive(false);
            right_animation.SetActive(false);
            idle_handraise_animation.SetActive(false);
            forward_handraise_animation.SetActive(false);
            backward_handraise_animation.SetActive(false);
            left_handraise_animation.SetActive(false);
            right_handraise_animation.SetActive(false);
            forward_jump_animation.SetActive(true);
            left_jump_animation.SetActive(false);
            right_jump_animation.SetActive(false);
        }
        
        //right hand raise
        else if (Input.GetAxis("Horizontal") > 0.1 && (Input.GetButton("Fire1") || Input.GetAxisRaw("Fire1") > 0 || Input.GetButton("Fire2") || Input.GetAxisRaw("Fire2") > 0))
        {
            idle_animation.SetActive(false);
            forward_animation.SetActive(false);
            backward_animation.SetActive(false);
            left_animation.SetActive(false);
            right_animation.SetActive(false);
            idle_handraise_animation.SetActive(false);
            forward_handraise_animation.SetActive(false);
            backward_handraise_animation.SetActive(false);
            left_handraise_animation.SetActive(false);
            right_handraise_animation.SetActive(true);
            forward_jump_animation.SetActive(false);
            left_jump_animation.SetActive(false);
            right_jump_animation.SetActive(false);
        }
        //left hand raise
        else if (Input.GetAxis("Horizontal") < -0.01 && (Input.GetButton("Fire1") || Input.GetAxisRaw("Fire1") > 0 || Input.GetButton("Fire2") || Input.GetAxisRaw("Fire2") > 0))
        {
            idle_animation.SetActive(false);
            forward_animation.SetActive(false);
            backward_animation.SetActive(false);
            left_animation.SetActive(false);
            right_animation.SetActive(false);
            idle_handraise_animation.SetActive(false);
            forward_handraise_animation.SetActive(false);
            backward_handraise_animation.SetActive(false);
            left_handraise_animation.SetActive(true);
            right_handraise_animation.SetActive(false);
            forward_jump_animation.SetActive(false);
            left_jump_animation.SetActive(false);
            right_jump_animation.SetActive(false);
        }
        //forward hand raise
        else if (Input.GetAxis("Vertical") > 0.1 && (Input.GetButton("Fire1") || Input.GetAxisRaw("Fire1") > 0 || Input.GetButton("Fire2") || Input.GetAxisRaw("Fire2") > 0))
        {
            idle_animation.SetActive(false);
            forward_animation.SetActive(false);
            backward_animation.SetActive(false);
            left_animation.SetActive(false);
            right_animation.SetActive(false);
            idle_handraise_animation.SetActive(false);
            forward_handraise_animation.SetActive(true);
            backward_handraise_animation.SetActive(false);
            left_handraise_animation.SetActive(false);
            right_handraise_animation.SetActive(false);
            forward_jump_animation.SetActive(false);
            left_jump_animation.SetActive(false);
            right_jump_animation.SetActive(false);
        }
        //backward hand raise
        else if (Input.GetAxis("Vertical") < -0.01 && (Input.GetButton("Fire1") || Input.GetAxisRaw("Fire1") > 0 || Input.GetButton("Fire2") || Input.GetAxisRaw("Fire2") > 0))
        {
            idle_animation.SetActive(false);
            forward_animation.SetActive(false);
            backward_animation.SetActive(false);
            left_animation.SetActive(false);
            right_animation.SetActive(false);
            idle_handraise_animation.SetActive(false);
            forward_handraise_animation.SetActive(false);
            backward_handraise_animation.SetActive(true);
            left_handraise_animation.SetActive(false);
            right_handraise_animation.SetActive(false);
            forward_jump_animation.SetActive(false);
            left_jump_animation.SetActive(false);
            right_jump_animation.SetActive(false);
        }
        //idle hand raise
        else if (Input.GetButton("Fire1") || Input.GetAxisRaw("Fire1") > 0 || Input.GetButton("Fire2") || Input.GetAxisRaw("Fire2") > 0) {
            idle_animation.SetActive(false);
            forward_animation.SetActive(false);
            backward_animation.SetActive(false);
            left_animation.SetActive(false);
            right_animation.SetActive(false);
            idle_handraise_animation.SetActive(true);
            forward_handraise_animation.SetActive(false);
            backward_handraise_animation.SetActive(false);
            left_handraise_animation.SetActive(false);
            right_handraise_animation.SetActive(false);
            forward_jump_animation.SetActive(false);
            left_jump_animation.SetActive(false);
            right_jump_animation.SetActive(false);
        }
        //right walk
        else if (Input.GetAxis("Horizontal") > 0.1)
        {
            idle_animation.SetActive(false);
            forward_animation.SetActive(false);
            backward_animation.SetActive(false);
            left_animation.SetActive(false);
            right_animation.SetActive(true);
            idle_handraise_animation.SetActive(false);
            forward_handraise_animation.SetActive(false);
            backward_handraise_animation.SetActive(false);
            left_handraise_animation.SetActive(false);
            right_handraise_animation.SetActive(false);
            forward_jump_animation.SetActive(false);
            left_jump_animation.SetActive(false);
            right_jump_animation.SetActive(false);
        }
        //left walk
        else if (Input.GetAxis("Horizontal") < -0.01)
        {
            idle_animation.SetActive(false);
            forward_animation.SetActive(false);
            backward_animation.SetActive(false);
            left_animation.SetActive(true);
            right_animation.SetActive(false);
            idle_handraise_animation.SetActive(false);
            forward_handraise_animation.SetActive(false);
            backward_handraise_animation.SetActive(false);
            left_handraise_animation.SetActive(false);
            right_handraise_animation.SetActive(false);
            forward_jump_animation.SetActive(false);
            left_jump_animation.SetActive(false);
            right_jump_animation.SetActive(false);
        }
        //forward walk
        else if (Input.GetAxis("Vertical") > 0.1)
        {
            idle_animation.SetActive(false);
            forward_animation.SetActive(true);
            backward_animation.SetActive(false);
            left_animation.SetActive(false);
            right_animation.SetActive(false);
            idle_handraise_animation.SetActive(false);
            forward_handraise_animation.SetActive(false);
            backward_handraise_animation.SetActive(false);
            left_handraise_animation.SetActive(false);
            right_handraise_animation.SetActive(false);
            forward_jump_animation.SetActive(false);
            left_jump_animation.SetActive(false);
            right_jump_animation.SetActive(false);
        }
        //backward walk
        else if (Input.GetAxis("Vertical") < -0.01)
        {
            idle_animation.SetActive(false);
            forward_animation.SetActive(false);
            backward_animation.SetActive(true);
            left_animation.SetActive(false);
            right_animation.SetActive(false);
            idle_handraise_animation.SetActive(false);
            forward_handraise_animation.SetActive(false);
            backward_handraise_animation.SetActive(false);
            left_handraise_animation.SetActive(false);
            right_handraise_animation.SetActive(false);
            forward_jump_animation.SetActive(false);
            left_jump_animation.SetActive(false);
            right_jump_animation.SetActive(false);
        }
        //idle
        else {
            idle_animation.SetActive(true);
            forward_animation.SetActive(false);
            backward_animation.SetActive(false);
            left_animation.SetActive(false);
            right_animation.SetActive(false);
            idle_handraise_animation.SetActive(false);
            forward_handraise_animation.SetActive(false);
            backward_handraise_animation.SetActive(false);
            left_handraise_animation.SetActive(false);
            right_handraise_animation.SetActive(false);
            forward_jump_animation.SetActive(false);
            left_jump_animation.SetActive(false);
            right_jump_animation.SetActive(false);
        }
    }
}
