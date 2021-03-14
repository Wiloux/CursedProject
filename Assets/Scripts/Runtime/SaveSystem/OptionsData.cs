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
    public KeyBind simpleAttackKey = new KeyBind("LMB");
    public KeyBind secondaryAttackKey = new KeyBind("RMB");

    public KeyBind interactKey = new KeyBind(KeyCode.E);
    public KeyBind abilityKey = new KeyBind(KeyCode.F);
    public KeyBind healKey = new KeyBind(KeyCode.H);

    public KeyBind forwardKey = new KeyBind(KeyCode.W);
    public KeyBind backwardKey = new KeyBind(KeyCode.S);

    public KeyBind menuKey = new KeyBind(KeyCode.Escape);
    public KeyBind inventoryKey = new KeyBind(KeyCode.Tab);

    public KeyBind swipeLeftInventoryKey = new KeyBind(KeyCode.A);
    public KeyBind swipeRightInventoryKey = new KeyBind(KeyCode.D);
}
[System.Serializable] public class KeyBind
{
    public enum KeyBindingType { mouse, keyboard};
    public KeyBindingType type = KeyBindingType.keyboard;
    public KeyCode keycode;
    public string mouseButton;

    public KeyBind(KeyCode keyCode = KeyCode.None)
    {
        this.type = KeyBindingType.keyboard;
        this.keycode = keyCode;
        this.mouseButton = "";
    }
    public KeyBind(string mouseButton)
    {
        this.type = KeyBindingType.mouse;
        this.keycode = KeyCode.None;
        this.mouseButton = mouseButton;
    }
    public KeyBind(KeyBind keybind)
    {
        this.type = keybind.type;
        this.keycode = keybind.keycode;
        this.mouseButton = keybind.mouseButton;
    }
    public void CopyFrom(KeyBind keybind)
    {
        if (keybind.type == KeyBindingType.keyboard) this.type = KeyBindingType.keyboard;
        else this.type = KeyBindingType.mouse;

        this.keycode = keybind.keycode;
        this.mouseButton = keybind.mouseButton;
    }

    new public string ToString()
    {
        string message = "Type: " + type.ToString() + "\t|| ";
        if (type == KeyBindingType.keyboard) { message += "Keycode: " + keycode.ToString(); }
        else message += "Mouse button: " + mouseButton;
        return message;
    }
}
