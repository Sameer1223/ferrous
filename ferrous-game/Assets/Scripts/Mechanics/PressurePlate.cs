using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ferrous.Mechanics
{
    public class PressurePlate : BaseSwitch
    {
        public bool Activated; 
        [SerializeField] private UnityEvent onActivation;
        public override void ActivateObject()
        {
            Activated = true;
            onActivation.Invoke();
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

