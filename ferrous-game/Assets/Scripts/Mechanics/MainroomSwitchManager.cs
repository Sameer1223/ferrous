using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainroomSwitchManager : BaseSwitch
{
    [SerializeField] private List<BaseSwitch> subIsOpen = new List<BaseSwitch>();
    private bool allSubActivated = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectSubActive(subIsOpen);
        if (allSubActivated)
        {
            Debug.Log("all subrooms activated");
        }
    }

    public void DetectSubActive(List<BaseSwitch> AllSubIsOpen)
    {
        foreach (SubroomSwitchManager Switch in AllSubIsOpen)
        {
            if (Switch.subRoomActivated == false)
            {
                allSubActivated = false;
                return;
            }
        }
        allSubActivated = true;
    }
}
