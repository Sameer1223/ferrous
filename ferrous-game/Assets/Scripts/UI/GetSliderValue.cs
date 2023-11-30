using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ferrous
{
    public class GetSliderValue : MonoBehaviour
    {
        [SerializeField] private Slider _sliderObj;
        [SerializeField] private float _sliderBias;
        [SerializeField] private TextMeshProUGUI _textMeshPro;

        // Update is called once per frame
        void Update()
        {
            if((_sliderObj.value + _sliderBias) == 0)
            {
                _textMeshPro.text = "Muted";
            }
            else
            {
                _textMeshPro.text = ((int)(_sliderObj.value + _sliderBias)).ToString();
            }
        }
    }
}
