using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ferrous.Mechanics
{
    public class TriggerSwitch : BaseSwitch
    {
        void Update()
        {

        }

        // ʵ�ֳ��󷽷�
        public override void ActivateObject()
        {
            activated = true;
            Debug.Log("Trigger Plate Activated");
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Metal")
            {
                ActivateObject();
            }
        }

    }
}

