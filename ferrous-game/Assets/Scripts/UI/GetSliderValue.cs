using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ferrous.UI
{
    public class GetSliderValue : MonoBehaviour
    {
        [SerializeField] private Slider _sliderObj;
        [SerializeField] private float _sliderBias;
        [SerializeField] private TextMeshProUGUI _textMeshPro;

        private int _showValue;

        // Update is called once per frame
        void Update()
        {
            _showValue = (int)(100 * _sliderObj.value);
            if((_showValue + _sliderBias) <= 0)
            {
                _textMeshPro.text = "Muted";
            }
            else
            {
                _textMeshPro.text = ((int)(_showValue + _sliderBias)).ToString();
            }
        }
    }
}
