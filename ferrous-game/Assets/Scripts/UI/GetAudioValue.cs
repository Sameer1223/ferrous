using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Ferrous.UI
{
    public class GetAudioValue : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private string _audioMixerGroup;

        private float valueDB;


        // Start is called before the first frame update
        void Start()
        {
            _audioMixer.GetFloat(_audioMixerGroup, out valueDB);
            _slider.value = Mathf.Pow(10.0f, valueDB / 20.0f);
            //DontDestroyOnLoad(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            _audioMixer.GetFloat(_audioMixerGroup, out valueDB);
            Debug.Log(valueDB);
        }


    }
}
