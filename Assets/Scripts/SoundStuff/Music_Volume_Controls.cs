using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music_Volume_Controls : MonoBehaviour
{
    public Slider thisSlider;
    public float musicVolume;

    public void SetMusicVolume(string whatValue)
    {
        float sliderValue = thisSlider.value;

        if (whatValue == "Music Slider")
        {
            musicVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("MusicVolume", musicVolume);
        }
    }
}
