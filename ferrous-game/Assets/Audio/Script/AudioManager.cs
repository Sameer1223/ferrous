using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioMixer AudioMixer;


    public void SetVolume(float value)
    {
        //Set Exposed Parameter in AudioMixer   
        AudioMixer.SetFloat("vMasterAudio", value);

    }


}
