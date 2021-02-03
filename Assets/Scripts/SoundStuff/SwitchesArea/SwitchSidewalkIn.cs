using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSidewalkIn : MonoBehaviour
{
    public string SwitchGroup = "GroundMaterial";
    public string Switch = "Sidewalk";
    public GameObject Player;
    public bool Debug_Enabled;

    private void OnTriggerStay(Collider collision)
    {
        if (Debug_Enabled) { Debug.Log(Switch + "switch set"); }
        AkSoundEngine.SetSwitch(SwitchGroup, Switch, Player);
    }
}
