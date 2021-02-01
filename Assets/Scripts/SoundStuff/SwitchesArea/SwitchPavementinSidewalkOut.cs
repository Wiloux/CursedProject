using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPavementinSidewalkOut : MonoBehaviour
{
    public string SwitchGroup = "GroundMaterial";
    public string Switch = "Pavement";
    public string ExitSwitch = "Sidewalk";
    public GameObject Player;
    public bool Debug_Enabled;

    private void OnTriggerEnter(Collider collision)
    {
        if (Debug_Enabled) { Debug.Log(Switch + "switch set"); }
        AkSoundEngine.SetSwitch(SwitchGroup, Switch, Player);
    }
    private void OnTriggerStay(Collider collision)
    {
        AkSoundEngine.SetSwitch(SwitchGroup, Switch, Player);
    }
    private void OnTriggerExit(Collider collision)
    {
        if (Debug_Enabled) { Debug.Log(ExitSwitch + "switch set"); }
        AkSoundEngine.SetSwitch(SwitchGroup, ExitSwitch, Player);
    }
}
