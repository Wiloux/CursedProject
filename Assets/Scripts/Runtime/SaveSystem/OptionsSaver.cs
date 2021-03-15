using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class OptionsSaver : MonoBehaviour
{
    public static OptionsSaver instance;
    private void Awake()
    {
        keyBindings = new KeyBindings();

        LoadFromOptionsSaveFile();

        if (instance != null) Destroy(this);
        else
        {
            instance = this;
            generalOptionsMenu.SetActive(true);
            keyBindingsMenu.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    //private void Start()
    //{
    //    keyBindings = new KeyBindings();
    //}

    public float sfxVolume = 7.5f;
    public float musicVolume = 7.5f;

    public float mouseSensitivity = 1f;

    public KeyBindings keyBindings = new KeyBindings();

    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    [SerializeField] private TMPro.TMP_InputField mouseSensitivityInputField;

    [SerializeField] private Transform keyBindingButtonsParent;

    [SerializeField] private GameObject generalOptionsMenu;
    [SerializeField] private GameObject keyBindingsMenu;


    private void OnEnable()
    {
        if (keyBindings == null) keyBindings = new KeyBindings();

        generalOptionsMenu.SetActive(true);
        keyBindingsMenu.SetActive(false);

        SetParametersValueUI();
    }

    #region Save / Load / Change
    public void CreateStandartOptionsSave()
    {
        sfxVolume = 7.5f;
        musicVolume = 7.5f;
        mouseSensitivity = 1f;
        keyBindings = new KeyBindings();

        SaveSystem.SaveOptionsData(this);
        Debug.Log("Standard options save created.");
    }
    public void LoadFromOptionsSaveFile()
    {
        OptionsData data = SaveSystem.LoadOptionsData();
        if(data == null)
        {
            CreateStandartOptionsSave();
            data = SaveSystem.LoadOptionsData();
        }

        LoadFromData(data);

        SaveSystem.SaveOptionsData(this);
        Debug.Log(SaveSystem.LoadOptionsData().ToString());
        GameHandler.instance.UseCurrentOptions(this);
        Debug.Log("Options loaded from save file.");
    }
    private void LoadFromData(OptionsData data)
    {
        sfxVolume = data.sfxVolume;
        musicVolume = data.musicVolume;
        mouseSensitivity = data.mouseSensitivity;
        keyBindings = data.keyBindings;
    }

    public void SaveCurrentOptionsInUI()
    {
        sfxVolume = sfxVolumeSlider.value;
        musicVolume = musicVolumeSlider.value;

        mouseSensitivity = System.Convert.ToInt32(mouseSensitivityInputField.text);

        GameHandler.instance.UseCurrentOptions(this);
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
            case KeyBindingButton.KeyBindingAction.sprint:
                keyBindings.sprintKey = keyBindingButton.keyCode;
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
        SaveCurrentOptionsInUI();
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
                switch (keyBindingButton.keyBindingAction)
                {
                    case KeyBindingButton.KeyBindingAction.simpleAttack:
                        keyBindingButton.SetButtonKeyCode(keyBindings.simpleAttackKey);
                        break;
                    case KeyBindingButton.KeyBindingAction.secondaryAttack:
                        keyBindingButton.SetButtonKeyCode(keyBindings.secondaryAttackKey);
                        break;
                    case KeyBindingButton.KeyBindingAction.interact:
                        keyBindingButton.SetButtonKeyCode(keyBindings.interactKey);
                        break;
                    case KeyBindingButton.KeyBindingAction.ability:
                        keyBindingButton.SetButtonKeyCode(keyBindings.abilityKey);
                        break;
                    case KeyBindingButton.KeyBindingAction.heal:
                        keyBindingButton.SetButtonKeyCode(keyBindings.healKey);
                        break;
                    case KeyBindingButton.KeyBindingAction.forward:
                        keyBindingButton.SetButtonKeyCode(keyBindings.forwardKey);
                        break;
                    case KeyBindingButton.KeyBindingAction.backwards:
                        keyBindingButton.SetButtonKeyCode(keyBindings.backwardKey);
                        break;
                    case KeyBindingButton.KeyBindingAction.sprint:
                        keyBindingButton.SetButtonKeyCode(keyBindings.sprintKey);
                        break;
                    case KeyBindingButton.KeyBindingAction.menu:
                        keyBindingButton.SetButtonKeyCode(keyBindings.menuKey);
                        break;
                    case KeyBindingButton.KeyBindingAction.inventory:
                        keyBindingButton.SetButtonKeyCode(keyBindings.inventoryKey);
                        break;
                    case KeyBindingButton.KeyBindingAction.swipeLeftInv:
                        keyBindingButton.SetButtonKeyCode(keyBindings.swipeLeftInventoryKey);
                        break;
                    case KeyBindingButton.KeyBindingAction.swipeRightInv:
                        keyBindingButton.SetButtonKeyCode(keyBindings.swipeRightInventoryKey);
                        break;
                }
            }
        }
    }

    private void SetParametersValueUI()
    {
        OptionsData data = new OptionsData(this);

        sfxVolumeSlider.value = data.sfxVolume;
        musicVolumeSlider.value = data.musicVolume;
        mouseSensitivityInputField.text = data.mouseSensitivity.ToString();

        SetButtonsKey();

        LoadFromData(data);
    }
    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(OptionsSaver))] public class OptionsSaverInspector: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        OptionsSaver saver = target as OptionsSaver;

        serializedObject.Update();

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Load options"))
        {
            saver.LoadFromOptionsSaveFile();
        }
        if(GUILayout.Button("Delete options"))
        {
            SaveSystem.DeleteOptionsData();
        }
        GUILayout.EndHorizontal();

        if(GUILayout.Button("Create new options")) 
        {
            saver.CreateStandartOptionsSave();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif