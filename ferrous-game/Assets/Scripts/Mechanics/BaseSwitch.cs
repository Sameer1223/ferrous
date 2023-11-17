using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ferrous.Mechanics
{
    public abstract class BaseSwitch : MonoBehaviour
    {
        public bool activated = false;

        public virtual void ActivateObject()
        {
            if (!activated)
            {
                activated = true;
            }
        }
    }
}

