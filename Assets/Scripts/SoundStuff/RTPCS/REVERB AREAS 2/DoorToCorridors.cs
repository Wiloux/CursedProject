using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorToCorridors : MonoBehaviour
{
    public void UseDoor(Transform Using)
    {
        AkSoundEngine.SetRTPCValue("RTPC_Reverb", 1);
        AkSoundEngine.SetRTPCValue("RTPC_Reverb_2", 0);
        AkSoundEngine.SetRTPCValue("RTPC_Reverb_Bathroom", 0);
    }
}