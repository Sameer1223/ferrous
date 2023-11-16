using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubroomSwitchManager : BaseSwitch
{
    [SerializeField] private List<BaseSwitch> activatedSwitches = new List<BaseSwitch>();
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;
    private bool allActivated = false;

    private Vector3 initialPositionLeft;
    private Vector3 initialPositionRight;
    private Vector3 targetPositionLeft;
    private Vector3 targetPositionRight;
    [SerializeField] float openSpeed = 1.0f;


    void Start()
    {
        initialPositionLeft = leftDoor.transform.position;
        initialPositionRight = rightDoor.transform.position;
        targetPositionLeft = initialPositionLeft + leftDoor.transform.right * 3f;
        targetPositionRight = initialPositionRight - rightDoor.transform.right * 3f;

    }


    void Update()
    {
        DetectActive(activatedSwitches);
        if (allActivated)
        {
            OpenDoor();
        }

    }

    public void OpenDoor()
    {
        rightDoor.transform.position = Vector3.MoveTowards(rightDoor.transform.position, targetPositionRight, openSpeed * Time.deltaTime);
        leftDoor.transform.position = Vector3.MoveTowards(leftDoor.transform.position, targetPositionLeft, openSpeed * Time.deltaTime);
        subRoomActivated = true;
    }

    public void DetectActive(List<BaseSwitch> ActivatedSwitches)
    {
        foreach(BaseSwitch Switch in ActivatedSwitches)
        {
            if (Switch.activated == false)
            {
                allActivated = false;
                return;
            }
        }
        allActivated = true;
    }

}
