using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyBindingButton : MonoBehaviour
{
    private TMPro.TMP_Text textDisplayer;
    public enum KeyBindingAction 
    { 
        simpleAttack, 
        secondaryAttack, 
        interact, 
        ability, 
        heal, 
        forward, 
        backwards, 
        menu, 
        inventory, 
        swipeLeftInv, 
        swipeRightInv
    };
    public KeyBindingAction keyBindingAction;
    public KeyBind keybind = new KeyBind(KeyCode.None);

    private void Awake()
    {
        textDisplayer = GetComponent<TMPro.TMP_Text>();
    }
    private void OnEnable()
    {
        if (keybind.type == KeyBind.KeyBindingType.keyboard) textDisplayer.text = keybind.keycode.ToString();
        else textDisplayer.text = keybind.mouseButton;
    }

    public void GetInputKey()
    {
        StartCoroutine(WaitAndGetForInputKey(
            () => SetKey()
        ));
    }
    private IEnumerator WaitAndGetForInputKey(Action onKeyGotten)
    {
        while(true){
            if (Event.current.isKey && Event.current.type == EventType.KeyDown)
            {
                if (Event.current.keyCode == KeyCode.Escape) break;
                else { keybind = new KeyBind(Event.current.keyCode); break; }
            }
            else if (Event.current.isMouse)
            {
                if (Event.current.type == EventType.ContextClick) { keybind = new KeyBind("RMB"); break; }
                else { keybind = new KeyBind("LMB"); break; }
            }
            yield return null;
        }
        onKeyGotten?.Invoke();
    }

    private void SetKey()
    {
        OptionsSaver.instance.ChangeAKeyBinding(this);
    }
}