using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchNormalStay : MonoBehaviour
{
    public string SwitchGroup = "GroundMaterial";
    public string Switch = "Normal";
    public GameObject Player;
    public bool Debug_Enabled;

    private void OnTriggerStay(Collider collision)
    {
        if (Debug_Enabled) { Debug.Log(Switch + "switch set"); }
        AkSoundEngine.SetSwitch(SwitchGroup, Switch, Player);
    }
}
