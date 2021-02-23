using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSliderRTPC : MonoBehaviour
{
    public Slider thisSlider;
    public float musicVolume;

    public void SetSpecificVolume(string whatValue)
    {
        float sliderValue = thisSlider.value;

        if (whatValue == "Music Slider")
        {
            musicVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("MusicSliderVolume", musicVolume);
        }
    }
}
