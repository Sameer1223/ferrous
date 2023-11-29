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
        [SerializeField] private Slider _soundSlider;
        [SerializeField] private float _sliderGap;
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if(Gamepad.current.rightStick.right.value>0)
            {
                _soundSlider.value += _sliderGap;
            }
            if (Gamepad.current.rightStick.left.value > 0)
            {
                _soundSlider.value -= _sliderGap;
            }
            if (Gamepad.current.buttonNorth.isPressed || Input.GetKeyDown(KeyCode.Y))
            {
                _soundSlider.value = -80;
            }
        }
    }
}
