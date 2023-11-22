using System.Collections;
using System.Collections.Generic;
using Ferrous.Mechanics;
using UnityEngine;
using UnityEngine.Events;

namespace Ferrous
{
    public class PlayerPlate : BaseSwitch
    {
        
        [SerializeField] private UnityEvent onActivation;
        private bool _playerOnTop;

        // Update is called once per frame
        void Update()
        {
            if (_playerOnTop)
            {
                ActivateObject();
            }
        }
        
        public override void ActivateObject()
        {
            Activated = true;
            onActivation.Invoke();
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Player")
            {
                _playerOnTop = true;
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.tag == "Player")
            {
                _playerOnTop = false;
            }
        }
    }
}
