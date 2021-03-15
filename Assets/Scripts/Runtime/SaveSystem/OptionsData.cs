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

    public OptionsData(OptionsSaver saver){
        this.sfxVolume = saver.sfxVolume;
        this.musicVolume = saver.musicVolume;
        this.mouseSensitivity = saver.mouseSensitivity;
        this.keyBindings = saver.keyBindings;
    }

    public new string ToString()
    {
        return "Sfx volume = " + sfxVolume.ToString() + " || music volume = " + musicVolume.ToString() + " || mouseSensitivity = " + mouseSensitivity.ToString();
    }
}
[System.Serializable]
public class KeyBindings
{
    public KeyCode simpleAttackKey = KeyCode.Mouse0;
    public KeyCode secondaryAttackKey = KeyCode.Mouse1;
    public KeyCode interactKey = KeyCode.E;
    public KeyCode abilityKey = KeyCode.F;
    public KeyCode healKey = KeyCode.H;
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode menuKey = KeyCode.Escape;
    public KeyCode inventoryKey = KeyCode.Tab;
    public KeyCode swipeLeftInventoryKey = KeyCode.A;
    public KeyCode swipeRightInventoryKey = KeyCode.D;


    public KeyBindings()
    {
        this.simpleAttackKey = KeyCode.Mouse0;
        this.secondaryAttackKey = KeyCode.Mouse1;

        this.interactKey = KeyCode.E;
        this.abilityKey = KeyCode.F;
        this.healKey = KeyCode.H;

        this.forwardKey = KeyCode.W;
        this.backwardKey = KeyCode.S;
        this.sprintKey = KeyCode.LeftShift;

        this.menuKey = KeyCode.Escape;
        this.inventoryKey = KeyCode.Tab;

        this.swipeLeftInventoryKey = KeyCode.A;
        this.swipeRightInventoryKey = KeyCode.D;
    }

    public KeyBindings(KeyCode simpleAttackKey, KeyCode secondaryAttackKey, KeyCode interactKey, KeyCode abilityKey, KeyCode healKey, KeyCode forwardKey, KeyCode backwardKey, KeyCode sprintKey,KeyCode menuKey, KeyCode inventoryKey, KeyCode swipeLeftInventoryKey, KeyCode swipeRightInventoryKey)
    {
        this.simpleAttackKey = simpleAttackKey;
        this.secondaryAttackKey = secondaryAttackKey;
        this.interactKey = interactKey;
        this.abilityKey = abilityKey;
        this.healKey = healKey;
        this.forwardKey = forwardKey;
        this.backwardKey = backwardKey;
        this.sprintKey = sprintKey;
        this.menuKey = menuKey;
        this.inventoryKey = inventoryKey;
        this.swipeLeftInventoryKey = swipeLeftInventoryKey;
        this.swipeRightInventoryKey = swipeRightInventoryKey;
    }
}
