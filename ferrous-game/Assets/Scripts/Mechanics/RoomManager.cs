using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Ferrous.Mechanics
{
    public class RoomManager : BaseSwitch
    {
        [SerializeField] private UnityEvent onAllActivated;
        [SerializeField] private List<BaseSwitch> mustBeActivated = new List<BaseSwitch>();
        private bool allActivated = false;


        void Update()
        {
            ActivateObject();
        }



        public override void ActivateObject()
        {
            foreach (BaseSwitch Switch in mustBeActivated)
            {
                if (Switch.Activated == false)
                {
                    return;
                }
            }
            onAllActivated.Invoke();
            Activated = true;
        }

    }

}
