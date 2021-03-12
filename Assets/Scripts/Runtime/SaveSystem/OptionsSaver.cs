using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsSaver : MonoBehaviour
{
    public static OptionsSaver instance;
    private void Awake()
    {
        if (instance != null) Destroy(this);
        else
        {
            instance = this;

            LoadFromOptionsSaveFile();
        }
    }

    private float sfxVolume = 7.5f;
    private float musicVolume = 7.5f;

    private float mouseSensitivity;

    private KeyBindings keyBindings;

    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    [SerializeField] private TMPro.TMP_InputField mouseSensitivitySlider;

    [SerializeField] private Transform keyBindingButtonsParent;

    [SerializeField] private GameObject generalOptionsMenu;
    [SerializeField] private GameObject keyBindingsMenu;


    private void OnEnable()
    {
        Debug.Log(SaveSystem.optionsSavePath);
        SetButtonsKey();
    }

    #region Save / Load / Change
    private void LoadFromOptionsSaveFile()
    {
        OptionsData data = SaveSystem.LoadOptionsData();
        if (data != null)
        {
            sfxVolume = data.sfxVolume;
            musicVolume = data.musicVolume;
            mouseSensitivity = data.mouseSensitivity;
            keyBindings = data.keyBindings;
        }
        else SaveSystem.SaveOptionsData(this);
    }

    public void UpdateChanges()
    {
        sfxVolume = sfxVolumeSlider.value;
        musicVolume = musicVolumeSlider.value;

        mouseSensitivity = System.Convert.ToInt32(mouseSensitivitySlider.text);

        SaveSystem.SaveOptionsData(this);
    }

    public void ChangeAKeyBinding(KeyBindingButton keyBindingButton)
    {
        switch (keyBindingButton.keyBindingAction)
        {
            case KeyBindingButton.KeyBindingAction.simpleAttack:
                keyBindings.simpleAttackKey = keyBindingButton.keybind;
                break;
            case KeyBindingButton.KeyBindingAction.secondaryAttack:
                keyBindings.secondaryAttackKey = keyBindingButton.keybind;
                break;
            case KeyBindingButton.KeyBindingAction.interact:
                keyBindings.interactKey = keyBindingButton.keybind;
                break;
            case KeyBindingButton.KeyBindingAction.ability:
                keyBindings.abilityKey = keyBindingButton.keybind;
                break;
            case KeyBindingButton.KeyBindingAction.heal:
                keyBindings.healKey = keyBindingButton.keybind;
                break;
            case KeyBindingButton.KeyBindingAction.forward:
                keyBindings.forwardKey = keyBindingButton.keybind;
                break;
            case KeyBindingButton.KeyBindingAction.backwards:
                keyBindings.backwardKey = keyBindingButton.keybind;
                break;
            case KeyBindingButton.KeyBindingAction.menu:
                keyBindings.menuKey = keyBindingButton.keybind;
                break;
            case KeyBindingButton.KeyBindingAction.inventory:
                keyBindings.inventoryKey = keyBindingButton.keybind;
                break;
            case KeyBindingButton.KeyBindingAction.swipeLeftInv:
                keyBindings.swipeLeftInventoryKey = keyBindingButton.keybind;
                break;
            case KeyBindingButton.KeyBindingAction.swipeRightInv:
                keyBindings.swipeRightInventoryKey = keyBindingButton.keybind;
                break;
        }
    }
    #endregion

    #region UI gestion
    public void ToggleKeyBindingsAndGeneralMenu()
    {
        generalOptionsMenu.SetActive(!generalOptionsMenu.activeSelf);
        keyBindingsMenu.SetActive(!keyBindingsMenu.activeSelf);
    }
    private void SetButtonsKey()
    {
        for(int i = 0; i < keyBindingButtonsParent.childCount; i++)
        {
            KeyBindingButton keyBindingButton = keyBindingButtonsParent.GetChild(i).GetComponent<KeyBindingButton>();
            if(keyBindingButton != null)
            {
                Debug.Log(keyBindingButton.keybind.keycode.ToString());
                if(keyBindingButton.keybind == null) { Debug.Log("cyke"); }
                switch (keyBindingButton.keyBindingAction)
                {
                    case KeyBindingButton.KeyBindingAction.simpleAttack:
                        keyBindingButton.keybind = 
                            new KeyBind(keyBindings.simpleAttackKey);
                        break;
                    case KeyBindingButton.KeyBindingAction.secondaryAttack:
                        keyBindingButton.keybind = keyBindings.secondaryAttackKey;
                        break;
                    case KeyBindingButton.KeyBindingAction.interact:
                        keyBindingButton.keybind = keyBindings.interactKey;
                        break;
                    case KeyBindingButton.KeyBindingAction.ability:
                        keyBindingButton.keybind = keyBindings.abilityKey;
                        break;
                    case KeyBindingButton.KeyBindingAction.heal:
                        keyBindingButton.keybind = keyBindings.healKey;
                        break;
                    case KeyBindingButton.KeyBindingAction.forward:
                        keyBindingButton.keybind = keyBindings.forwardKey;
                        break;
                    case KeyBindingButton.KeyBindingAction.backwards:
                        keyBindingButton.keybind = keyBindings.backwardKey;
                        break;
                    case KeyBindingButton.KeyBindingAction.menu:
                        keyBindingButton.keybind = keyBindings.menuKey;
                        break;
                    case KeyBindingButton.KeyBindingAction.inventory:
                        keyBindingButton.keybind = keyBindings.inventoryKey;
                        break;
                    case KeyBindingButton.KeyBindingAction.swipeLeftInv:
                        keyBindingButton.keybind = keyBindings.swipeLeftInventoryKey;
                        break;
                    case KeyBindingButton.KeyBindingAction.swipeRightInv:
                        keyBindingButton.keybind = keyBindings.swipeRightInventoryKey;
                        break;
                }
            }
        }
    }
    #endregion
}
