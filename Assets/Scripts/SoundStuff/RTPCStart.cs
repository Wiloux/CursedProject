using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTPCStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.SetRTPCValue("MusicVolume", 8);
        AkSoundEngine.SetRTPCValue("SFXVolume", 8);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
