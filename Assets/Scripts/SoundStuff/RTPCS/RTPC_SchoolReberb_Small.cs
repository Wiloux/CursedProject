using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTPC_SchoolReberb_Small : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AkSoundEngine.SetRTPCValue("RTPC_Reverb_2", 1);
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AkSoundEngine.SetRTPCValue("RTPC_Reverb_2", 0);
        }
    }
}
