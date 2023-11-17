using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Ferrous.Mechanics
{
    public class SwitchManager : BaseSwitch
    {
        [SerializeField] private UnityEvent onAllSwitchesActivated;
        [SerializeField] private List<BaseSwitch> activatedSwitches = new List<BaseSwitch>();
        private bool allActivated = false;


        void Update()
        {
            DetectActive(activatedSwitches);
            if (allActivated)
            {
                onAllSwitchesActivated.Invoke();
            }

        }

        
        public void DetectActive(List<BaseSwitch> ActivatedSwitches)
        {
            foreach (BaseSwitch Switch in ActivatedSwitches)
            {
                if (Switch.activated == false)
                {
                    allActivated = false;
                    return;
                }
            }
            allActivated = true;
            activated = true;
        }

    }

}
