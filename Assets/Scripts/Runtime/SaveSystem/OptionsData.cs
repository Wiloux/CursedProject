using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionsData
{
    public float sfxVolume;
    public float musicVolume;

    public float mouseSensitivity;

    public KeyBindings keyBindings;

    public OptionsData(OptionsSaver progress){

    }
}
[System.Serializable]
public class KeyBindings
{
    public KeyCode simpleAttackKey;
    public KeyCode secondaryAttackKey;

    public KeyCode interactKey = KeyCode.E;
    public KeyCode abilityKey = KeyCode.F;
    public KeyCode healKey = KeyCode.H;

    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;

    public KeyCode menuKey = KeyCode.Escape;
    public KeyCode inventoryKey = KeyCode.Tab;

    public KeyCode swipeLeftInventoryKey = KeyCode.A;
    public KeyCode swipeRightInventoryKey = KeyCode.D;
}
