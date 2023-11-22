using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ferrous.Mechanics
{
    public class PressurePlate : BaseSwitch
    {
        public bool Activated;
        public override void ActivateObject()
        {
            Activated = true;
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

