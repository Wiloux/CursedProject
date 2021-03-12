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

    [SerializeField] private List<KeyBindingButton> keyBindingButtons = new List<KeyBindingButton>();

    [SerializeField] private GameObject generalOptionsMenu;
    [SerializeField] private GameObject keyBindingsMenu;


    private void OnEnable()
    {
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
                keyBindings.simpleAttackKey = keyBindingButton.keyCode;
                break;
            case KeyBindingButton.KeyBindingAction.secondaryAttack:
                keyBindings.secondaryAttackKey = keyBindingButton.keyCode;
                break;
            case KeyBindingButton.KeyBindingAction.interact:
                keyBindings.interactKey = keyBindingButton.keyCode;
                break;
            case KeyBindingButton.KeyBindingAction.ability:
                keyBindings.abilityKey = keyBindingButton.keyCode;
                break;
            case KeyBindingButton.KeyBindingAction.heal:
                keyBindings.healKey = keyBindingButton.keyCode;
                break;
            case KeyBindingButton.KeyBindingAction.forward:
                keyBindings.forwardKey = keyBindingButton.keyCode;
                break;
            case KeyBindingButton.KeyBindingAction.backwards:
                keyBindings.backwardKey = keyBindingButton.keyCode;
                break;
            case KeyBindingButton.KeyBindingAction.menu:
                keyBindings.menuKey = keyBindingButton.keyCode;
                break;
            case KeyBindingButton.KeyBindingAction.inventory:
                keyBindings.inventoryKey = keyBindingButton.keyCode;
                break;
            case KeyBindingButton.KeyBindingAction.swipeLeftInv:
                keyBindings.swipeLeftInventoryKey = keyBindingButton.keyCode;
                break;
            case KeyBindingButton.KeyBindingAction.swipeRightInv:
                keyBindings.swipeRightInventoryKey = keyBindingButton.keyCode;
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
        foreach (KeyBindingButton keyBindingButton in keyBindingButtons)
        {
            switch (keyBindingButton.keyBindingAction)
            {
                case KeyBindingButton.KeyBindingAction.simpleAttack:
                    keyBindingButton.keyCode = keyBindings.simpleAttackKey;
                    break;
                case KeyBindingButton.KeyBindingAction.secondaryAttack:
                    keyBindingButton.keyCode = keyBindings.secondaryAttackKey;
                    break;
                case KeyBindingButton.KeyBindingAction.interact:
                    keyBindingButton.keyCode = keyBindings.interactKey;
                    break;
                case KeyBindingButton.KeyBindingAction.ability:
                    keyBindingButton.keyCode = keyBindings.abilityKey;
                    break;
                case KeyBindingButton.KeyBindingAction.heal:
                    keyBindingButton.keyCode = keyBindings.healKey;
                    break;
                case KeyBindingButton.KeyBindingAction.forward:
                    keyBindingButton.keyCode = keyBindings.forwardKey;
                    break;
                case KeyBindingButton.KeyBindingAction.backwards:
                    keyBindingButton.keyCode = keyBindings.backwardKey;
                    break;
                case KeyBindingButton.KeyBindingAction.menu:
                    keyBindingButton.keyCode = keyBindings.menuKey;
                    break;
                case KeyBindingButton.KeyBindingAction.inventory:
                    keyBindingButton.keyCode = keyBindings.inventoryKey;
                    break;
                case KeyBindingButton.KeyBindingAction.swipeLeftInv:
                    keyBindingButton.keyCode = keyBindings.swipeLeftInventoryKey;
                    break;
                case KeyBindingButton.KeyBindingAction.swipeRightInv:
                    keyBindingButton.keyCode = keyBindings.swipeRightInventoryKey;
                    break;
            }
        }
    }
    #endregion
}
