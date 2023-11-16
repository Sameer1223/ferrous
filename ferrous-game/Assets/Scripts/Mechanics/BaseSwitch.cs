using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSwitch : MonoBehaviour
{
    public bool activated = false;
    public bool subRoomActivated = false;

    // �����󱻼���ʱ�����������
    public virtual void ActivateObject()
    {
        if (!activated)
        {
            activated = true;
        }
    }
}
