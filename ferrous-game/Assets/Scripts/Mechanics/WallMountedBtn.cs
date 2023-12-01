using UnityEngine;
using UnityEngine.Events;

namespace Ferrous.Mechanics
{
    public class WallMountedBtn : BaseSwitch
    {
        [Header("Button Activation")]
        [SerializeField] private UnityEvent onActivation;

        private bool _canPress;
        private bool _interactInput;
        private bool _pressed = false;

        private void Update()
        {
            PlayerInput();

            if (_canPress && _interactInput && !_pressed)
            {
                ActivateObject();
                _pressed = true;
            }
        }
        
        public override void ActivateObject()
        {
            Activated = true;
            onActivation.Invoke();
        }
        
        private void OnTriggerStay(Collider collision)
        {
            if (collision.tag == "Player")
            {
                _canPress = true;
            }
        }

        private void PlayerInput()
        {
            _interactInput = InputManager.instance.InteractInput;
        }
    }
}
