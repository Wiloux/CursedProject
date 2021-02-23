using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXVolumeSliderRTPC : MonoBehaviour
{
    public Slider thisSlider2;
    public float sfxVolume;

    public void SetSFXVolume(string whatValue)
    {
        float sliderValue = thisSlider2.value;

        if (whatValue == "SFX Slider")
        {
            sfxVolume = thisSlider2.value;
            AkSoundEngine.SetRTPCValue("SFXSliderVolume", sfxVolume);
        }
    }
}
