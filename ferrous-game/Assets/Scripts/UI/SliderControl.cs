using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Ferrous
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

        [SerializeField] private float _sliderGap = 0.3f;
        private bool wasDpadDownPressed = false;
        private bool wasDpadUpPressed = false;
        void Start()
        {
            _sliderAmounts = _soundSliders.Count;
        }

        // Update is called once per frame
        void Update()
        {
            var controllers = Input.GetJoystickNames();
            if (controllers.Length > 0)
            {
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
            for(int i =0; i<  _sliderAmounts; i++)
            {
                if(i == _sliderSelect)
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
                _currentSoundSlider.value = -80;
            }
        }
    }
}
