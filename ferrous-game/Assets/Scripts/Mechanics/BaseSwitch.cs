using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ferrous.Mechanics
{
    public abstract class BaseSwitch : MonoBehaviour
    {
        private bool activated = false;

        public bool Activated             //�������ԣ���������˽���ֶ�id
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

        public virtual void DeactivateObject()
        {
            throw new System.NotImplementedException();
        }
    }
}

