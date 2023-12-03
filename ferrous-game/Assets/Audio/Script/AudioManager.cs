using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Ferrous.AudioManager
{
    public class AudioManager : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private AudioMixer AudioMixer;
        [SerializeField] private string _audioName = "vMasterAudio";

        private float Remap01ToDB(float x)
        {
            if (x <= 0.0f) x = 0.0001f;
            return Mathf.Log10(x) * 20.0f;
        }

        public void SetVolume(float value)
        {
            //Set Exposed Parameter in AudioMixer
            value = Remap01ToDB(value);
            AudioMixer.SetFloat(_audioName, value);

        }


    }
}

