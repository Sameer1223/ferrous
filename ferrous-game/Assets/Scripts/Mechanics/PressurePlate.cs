using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ferrous.Mechanics
{
    public class PressurePlate : BaseSwitch
    {
        [SerializeField] private UnityEvent onActivation;
        public override void ActivateObject()
        {
            Activated = true;
            onActivation.Invoke();
        }

        public override void DeactivateObject()
        {
            Activated = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Metal")
            {
                ActivateObject();
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.tag == "Metal")
            {
                DeactivateObject();
            }
        }

    }
}

