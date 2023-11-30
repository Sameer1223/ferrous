using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioMixer AudioMixer;
    [SerializeField] private string _audioName = "vMasterAudio";


    public void SetVolume(float value)
    {
        //Set Exposed Parameter in AudioMixer   
        AudioMixer.SetFloat(_audioName, value);

    }


}
