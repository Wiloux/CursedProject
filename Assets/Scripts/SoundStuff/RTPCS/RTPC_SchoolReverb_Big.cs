using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTPC_SchoolReverb_Big : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AkSoundEngine.SetRTPCValue("RTPC_Reverb", 1);
            AkSoundEngine.SetRTPCValue("RTPC_Reverb_2", 0);
            AkSoundEngine.SetRTPCValue("RTPC_Reverb_Bathroom", 0);
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AkSoundEngine.SetRTPCValue("RTPC_Reverb", 0);
        }
    }
}
