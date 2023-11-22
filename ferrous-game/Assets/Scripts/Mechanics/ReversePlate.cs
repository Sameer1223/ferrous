using Ferrous.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ferrous
{
    public class ReversePlate : BaseSwitch
    {
        public override void ActivateObject()
        {
            Activated = true;
        }
        void OnCollisionExit(Collision collision)
        {

                ActivateObject();
                Debug.Log("Activated");
            

        }
    }
}
