using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Ferrous.UI
{
    public class SliderControl : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private List<Slider> _soundSliders;
        [SerializeField] private List<Image> _handlers;
        private Slider _currentSoundSlider;
        private int _sliderAmounts;
        private int _previousSliderSelect;
        private int _sliderSelect = 0;

        [SerializeField] private float _sliderGap = 0.03f;
        private bool wasDpadDownPressed = false;
        private bool wasDpadUpPressed = false;

        [SerializeField]private PlayerInput _playerInput;
        void Start()
        {
            _sliderAmounts = _soundSliders.Count;

            Debug.Log(_playerInput);
        }

        // Update is called once per frame
        void Update()
        {
            string _currentControlScheme = _playerInput.currentControlScheme;
            //string[] joystickNames = Input.GetJoystickNames();
            //var controllers = Input.GetJoystickNames();
            //Debug.Log(joystickNames[0]);
            if (/*joystickNames.Length>0&&joystickNames[0] != ""*/_currentControlScheme == "Gamepad"&& Gamepad.current != null)
            {
                //Debug.Log(Input.GetJoystickNames());
                SelectSlider();
                ControlVolume();
            }
        }

        private void SelectSlider()
        {
            if (Gamepad.current.dpad.down.isPressed && !wasDpadDownPressed && _sliderSelect < (_sliderAmounts - 1))
            {
                _sliderSelect += 1;
            }
            else if (Gamepad.current.dpad.down.isPressed && !wasDpadDownPressed && _sliderSelect >= (_sliderAmounts - 1))
            {
                _sliderSelect = 0;
            }
            else if (Gamepad.current.dpad.up.isPressed && !wasDpadUpPressed && _sliderSelect > 0)
            {
                _sliderSelect -= 1;
            }
            else if (Gamepad.current.dpad.up.isPressed && !wasDpadUpPressed && _sliderSelect <= 0)
            {
                _sliderSelect = (_sliderAmounts - 1);
            }

            wasDpadDownPressed = Gamepad.current.dpad.down.isPressed;
            wasDpadUpPressed = Gamepad.current.dpad.up.isPressed;

            Debug.Log(_previousSliderSelect);
            _currentSoundSlider = _soundSliders[_sliderSelect];
            for (int i = 0; i < _sliderAmounts; i++)
            {
                if (i == _sliderSelect)
                {
                    _handlers[i].color = Color.white;
                }
                else
                {
                    _handlers[i].color = Color.gray;
                }
            }
        }


        private void ControlVolume()
        {
            if (Gamepad.current.dpad.right.isPressed)
            {
                _currentSoundSlider.value += _sliderGap;
            }
            if (Gamepad.current.dpad.left.isPressed)
            {
                _currentSoundSlider.value -= _sliderGap;
            }
            if (Gamepad.current.buttonNorth.isPressed || Input.GetKeyDown(KeyCode.Y))
            {
                _currentSoundSlider.value = 0;
            }
        }
    }
}
