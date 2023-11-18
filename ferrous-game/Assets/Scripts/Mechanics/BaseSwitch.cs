using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ferrous.Mechanics
{
    public abstract class BaseSwitch : MonoBehaviour
    {
        private bool activated = false;

        public bool Activated             //定义属性，用来操作私有字段id
        {
            get
            {
                return activated;
            }
            set
            {
                activated = value;
            }
        }
        public abstract void ActivateObject();
    }
}

