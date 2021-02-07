using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTPC_Occlusion_Test : MonoBehaviour
{
    public GameObject AudioListener;
    public string RTPC_Volume = "RTPC_Occlusion_Volume";
    public string RTPC_LoPass = "RTPC_Occlusion_LoPass";



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Direction = AudioListener.transform.position - transform.position;
        float Distance = Vector3.Distance(transform.position, AudioListener.transform.position);
        RaycastHit outInfo;
        int mask = 1 << LayerMask.NameToLayer("Ignore_Occlude");
        mask |= 1 << LayerMask.NameToLayer("Occlude");
        bool hit = Physics.Raycast(transform.position, Direction, out outInfo, Distance, mask);

        if (hit)
        {
            if (outInfo.collider.gameObject.layer == LayerMask.NameToLayer("Occlude"))
            {
                Debug.DrawRay(transform.position, Direction, Color.blue);
                print("Is being occluded");
                AkSoundEngine.SetRTPCValue("RTPC_Occlusion_Volume", 1, gameObject);
                AkSoundEngine.SetRTPCValue("RTPC_Occlusion_LoPass", 1, gameObject);
            }
            if (outInfo.collider.gameObject.layer == LayerMask.NameToLayer("Ignore_Occlude"))
            {
                Debug.DrawRay(transform.position, Direction, Color.green);
                AkSoundEngine.SetRTPCValue("RTPC_Occlusion_Volume", 0, gameObject);
                AkSoundEngine.SetRTPCValue("RTPC_Occlusion_LoPass", 0, gameObject);
            }
        }     
        else
        {
            Debug.DrawRay(transform.position, Direction, Color.red);
            AkSoundEngine.SetRTPCValue("RTPC_Occlusion_Volume", 0, gameObject);
            AkSoundEngine.SetRTPCValue("RTPC_Occlusion_LoPass", 0, gameObject);
        }
    }
}
