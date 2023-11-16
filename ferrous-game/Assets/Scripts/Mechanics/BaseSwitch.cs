using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSwitch : MonoBehaviour
{
    public bool activated = false;
    public bool subRoomActivated = false;

    // 当对象被激活时调用这个方法
    public virtual void ActivateObject()
    {
        if (!activated)
        {
            activated = true;
        }
    }
}
