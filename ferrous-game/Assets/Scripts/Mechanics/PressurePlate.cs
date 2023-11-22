using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ferrous.Mechanics
{
    public class PressurePlate : BaseSwitch
    {

        public override void ActivateObject()
        {
            Activated = true;
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

