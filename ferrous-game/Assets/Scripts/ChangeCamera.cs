using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Camera;
    public GameObject Player;
    public float showDistance = 6;

    private bool _pushInput;
    private bool _pullInput;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        float sqrLenght = (Player.transform.position - transform.position).sqrMagnitude;
        if (sqrLenght < showDistance * showDistance && (_pushInput||_pullInput))
        {
            Camera.SetActive(true);
        }
        else
        {
            Camera.SetActive(false);
        }
    }

    private void PlayerInput()
    {
        _pushInput = InputManager.instance.PushInput;
        _pullInput = InputManager.instance.PullInput;
    }
}
