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
        sprint,
        menu, 
        inventory, 
        swipeLeftInv, 
        swipeRightInv
    };
    public KeyBindingAction keyBindingAction;
    public KeyCode keyCode;

    private void Awake()
    {
        textDisplayer = GetComponent<TMPro.TMP_Text>();
    }
    private void OnEnable()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        textDisplayer.text = keyCode.ToString();        
    }

    public void GetInputKey()
    {
        StartCoroutine(WaitAndGetForInputKey(
            () => { UpdateVisual(); SetKeyBindingsParameter(); }
        ));
    }
    private IEnumerator WaitAndGetForInputKey(Action onKeyGotten)
    {
        GameHandler.instance.locking = true;

        onKeyGotten += () => { GameHandler.instance.locking = false; StopAllCoroutines(); };

        Array keycodes = Enum.GetValues(typeof(KeyCode));
        while(true){
            Debug.Log(Event.current != null);
            if(Input.anyKeyDown)
            {
                Debug.Log("keydown");
                foreach(KeyCode keycode in keycodes)
                {
                    if (Input.GetKeyDown(keycode))
                    {
                        this.keyCode = keycode;
                        onKeyGotten?.Invoke();
                    }
                }
            }
            yield return null;
        }
    }

    private void SetKeyBindingsParameter()
    {
        OptionsSaver.instance.ChangeAKeyBinding(this);
    }

    public void SetButtonKeyCode(KeyCode keyCode)
    {
        this.keyCode = keyCode;
    }
}