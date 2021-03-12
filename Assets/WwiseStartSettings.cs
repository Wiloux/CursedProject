using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseStartSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.SetRTPCValue("RTPC_Reverb_Bathroom", 1);
        AkSoundEngine.SetRTPCValue("RTPC_Reverb", 0);
        AkSoundEngine.SetRTPCValue("RTPC_Reverb_2", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
