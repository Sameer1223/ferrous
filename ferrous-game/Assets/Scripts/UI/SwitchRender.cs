using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SwitchRender : MonoBehaviour
{
    // Start is called before the first frame update
    public Image imgSpeaker;
    public Sprite Speaker;
    public Sprite SpeakerOff;
    public Slider slider;
    private int i = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Detectvalue();
    }

    public void Detectvalue()
    {
        if(slider.value == -80)
        {
            imgSpeaker.sprite = SpeakerOff;
        }
        else
        {
            imgSpeaker.sprite = Speaker;
        }
    }
}
