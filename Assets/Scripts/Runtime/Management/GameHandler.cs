using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;

    // Sanity vars
    #region Sanity vars
    public enum OxygenState { high, medium, low};
    public OxygenState oxygenState;
    public OxygenState GetOxygenState() { return oxygenState; }
    public bool IsOxgenLow() { return oxygenState == OxygenState.low; }
    private bool interactibleHallucinationsSpawned = false;

    private float sanity = 0f;
    public float Sanity
    {
        get => sanity;
        set { sanity = value; sanityDecreaseTimer = timeToDecreaseSanity; }
    }

    private float sanityDecreaseTimer;
    public float sanityDecreaseRate;
    public float timeToDecreaseSanity;

    #endregion

    // UI Vars
    #region UI vars
    public bool isPaused;
    private bool isSaveMenuOpen;
    private bool isInventoryMenuOpen;

    [Space(5)]
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject saveMenu;
    public GameObject inventoryMenu;

    public GameObject facelessGirlDamageIndicator;
    private float damageIndicatorTimer;
    private IEnumerator gradualIncreaseCoroutine;

    public TMPro.TMP_Text messageDisplayer;
    private IEnumerator messageDisplayerFadeCoroutine;
    public string pickupItemsMessageColor = "green";

    private Material DmgIndMat;
    private float DmgIndMax;

    #endregion
    public bool locking;

    // Wwise vars
    #region Wwise events
    private GameObject listener;
    public AK.Wwise.Event openingSavePauseMenu;
    public AK.Wwise.Event closingSavePauseMenu;
    #endregion

    // Monobehaviours methods
    #region Monobehaviours methods
    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
        listener = Camera.main.gameObject;
    }

    //private void Start()
    //{
    //    Time.timeScale = 0.1f;
    //}

    // Update is called once per frame
    void Update()
    {

        if (!locking)
        {
            #region Sanity gestion
            if (sanity > 0)
            {
                if (sanityDecreaseTimer > 0)
                {
                    sanityDecreaseTimer -= Time.deltaTime;
                    if (sanity >= 150) PlayerHelper.instance.Die();
                }
                else
                {
                    sanity -= sanityDecreaseRate * Time.deltaTime;
                }
            }

            if(sanity > 99) 
            { 
                oxygenState = OxygenState.low;
                if (!interactibleHallucinationsSpawned)
                {
                    // Spawn interactible hallucinations object
                    SpawnInteractibleHallucinations();
                    interactibleHallucinationsSpawned = true;
                }
            }
            else if(sanity > 50) 
            {
                if (interactibleHallucinationsSpawned) { interactibleHallucinationsSpawned = false; DespawnInteractibleHallucinations(); } // despawn hallucinations objects
                oxygenState = OxygenState.medium; 
            }
            else{ oxygenState = OxygenState.high; }

            AkSoundEngine.SetRTPCValue("HallucinationsRTPC", Mathf.Clamp(sanity, 0f, 100f), gameObject); // Here Wwise stuffs
        #endregion

            #region UI gestion
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
                if (isSaveMenuOpen) { ToggleSaveMenu(); }
                else if (isInventoryMenuOpen) ToggleInventoryMenu();
                else { TogglePauseMenu(); }
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                TogglePause();
                ToggleInventoryMenu();
            }

            if (damageIndicatorTimer > 0)
            {
                damageIndicatorTimer -= Time.deltaTime;

                if (DmgIndMat != null) DmgIndMat.SetFloat("Vector1_98c453588a654653b7765100bbc55cf4", Mathf.Lerp(0, DmgIndMax, damageIndicatorTimer));

                if (damageIndicatorTimer < 0)
                {
                    facelessGirlDamageIndicator.SetActive(false); // here wiloux, l'image s'éteind
                }
            }
            #endregion
        }
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 50;
        style.normal.textColor = Color.white;

        //GUILayout.Label("Sanity : " + sanity.ToString(), style);
        //GUILayout.Label("Faceless girl DamageIndicatorTimer : " + damageIndicatorTimer.ToString(), style);
        //GUILayout.Label("Color : " + messageDisplayer.color.a, style);

        // on s'en blc c pour le debug
    }
    #endregion

    public bool IsPaused() { return isPaused; }

    #region Pause functions
    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused) Time.timeScale = 0f;
        else Time.timeScale = 1f;

        MouseManagement.instance.SetMouseLock(!pauseMenu);
        //isPaused = !isPaused;
        //GameObject[] objects = SceneManager.GetActiveScene().GetRootGameObjects();
        //foreach(GameObject go in objects)
        //{
        //    switch (go.tag)
        //    {
        //        case "Enemy":
        //            EnemyBase enemy = go.GetComponent<EnemyBase>();
        //            if(enemy != null){EnemyHelper.TogglePause(enemy);}
        //            else{
        //                for(int i = 0; i < go.transform.childCount;i ++)
        //                {
        //                    enemy = go.transform.GetChild(i).GetComponent<EnemyBase>();
        //                    if(enemy != null) { EnemyHelper.TogglePause(enemy); }
        //                }
        //            }
        //            continue;
        //        case "Player":
        //            PlayerHelper.instance.ToggleControls();
        //            continue;
        //        case "Mirror":
        //            SquareMirror mirroCam = go.GetComponent<SquareMirror>();
        //            if (mirroCam != null) mirroCam.enabled = !mirroCam.enabled;
        //            continue;

        //    }
        //    Shard shard = go.GetComponent<Shard>();
        //    if (shard != null) { shard.TogglePause(); continue; }
        //}
    }
    public void SetPause(bool pause) { isPaused = pause; if (pause) Time.timeScale = 0; else Time.timeScale = 1; MouseManagement.instance.SetMouseLock(!pause); }

    #endregion

    #region UI functions
        #region Toggle menus
    public void TogglePauseMenu() { PlayCloseOrOpenMenuSound(pauseMenu); pauseMenu.SetActive(!pauseMenu.activeSelf); }
    public void ToggleSaveMenu() { PlayCloseOrOpenMenuSound(pauseMenu); saveMenu.SetActive(!saveMenu.activeSelf); isSaveMenuOpen = !isSaveMenuOpen; }
    public void ToggleInventoryMenu() { inventoryMenu.SetActive(!inventoryMenu.activeSelf); isInventoryMenuOpen = !isInventoryMenuOpen; }
        #endregion

        #region Func for UI buttons
    public void LeftPauseMenu() { SetPause(false); TogglePauseMenu(); }
    public void QuitApplication() { Application.Quit(); }

    public void OpenOptionsFromPauseMenu() { pauseMenu.SetActive(false); optionsMenu.SetActive(true); }
    public void OpenPauseMenuFromOptions() { pauseMenu.SetActive(true); optionsMenu.SetActive(false); }
        #endregion

        #region Faceless Girl Damage Indicator functions
    public void DisplayFacelessGirlDamageIndicator(Material _DmgIndMat, float _MaxDmgInd)
    {
        DmgIndMat = _DmgIndMat;
        DmgIndMax = _MaxDmgInd;
        facelessGirlDamageIndicator.SetActive(true);
        if (damageIndicatorTimer >= 0.45f) { damageIndicatorTimer = 0.5f; StopCoroutine(gradualIncreaseCoroutine); }
        else
        {
            if (gradualIncreaseCoroutine == null) gradualIncreaseCoroutine = IncreaseGraduallyTimer();
            StopCoroutine(gradualIncreaseCoroutine);
            StartCoroutine(gradualIncreaseCoroutine);
        }
    }
    private IEnumerator IncreaseGraduallyTimer()
    {
        while (damageIndicatorTimer < 0.7f)
        {
            damageIndicatorTimer += 2 * Time.deltaTime;
            yield return null;
        }
    }
        #endregion

        #region Custom Message Displayer
    public void DisplayCustomMessage(string message, float waitDuration)
    {
        messageDisplayer.gameObject.SetActive(true);
        messageDisplayer.text = message;
        if(messageDisplayerFadeCoroutine != null) StopCoroutine(messageDisplayerFadeCoroutine);
        messageDisplayerFadeCoroutine = FadeMessageDisplayer(waitDuration);
        StartCoroutine(messageDisplayerFadeCoroutine);
    }
    public void DisplayPickupItemMessage(string itemName, float waitDuration)
    {
        DisplayCustomMessage("You have found <color=" + pickupItemsMessageColor + "> " + itemName, waitDuration);
    }
    private IEnumerator FadeMessageDisplayer(float waitDuration)
    {
        Color textColor = messageDisplayer.color;
        textColor.a = 0;
        while(textColor.a < 1f)
        {
            textColor.a += Time.unscaledDeltaTime * 4;
            if (textColor.a > 1) textColor.a = 1;
            messageDisplayer.color = textColor;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(waitDuration);
        while(textColor.a > 0)
        {
            textColor.a -= Time.unscaledDeltaTime;
            if (textColor.a < 0) textColor.a = 0;
            messageDisplayer.color = textColor;
            yield return null;
        }
        messageDisplayer.gameObject.SetActive(false);
    }
        #endregion

    private void PlayCloseOrOpenMenuSound(GameObject menu)
    {
        if (menu.activeSelf) closingSavePauseMenu?.Post(listener);
        else openingSavePauseMenu?.Post(listener);
    }
    #endregion

    #region Options/Parameters
    public void UseCurrentOptions(OptionsSaver options)
    {
        SetVolumes(options.musicVolume, options.sfxVolume);
        PlayerHelper.instance.SetMouseSensivity(options.mouseSensitivity);
        PlayerHelper.instance.SetKeyBindings(options.keyBindings);
    }
    public void SetVolumes(float musicVolume, float sfxVolume) //
    {
        // the values are between 0 and 10 miguel ^^
    }
    #endregion

    #region InteratcibleHallucinations
    private InteractibleHallucination[] GetAllInteractibleHallucinations()
    {
        return FindObjectsOfType<InteractibleHallucination>();
    }

    public void SpawnInteractibleHallucinations()
    {
        foreach(InteractibleHallucination interactibleHallucination in GetAllInteractibleHallucinations())
        {
            interactibleHallucination.gameObject.SetActive(true);
        }
    }
    public void DespawnInteractibleHallucinations()
    {
        foreach(InteractibleHallucination interactibleHallucination in GetAllInteractibleHallucinations())
        {
            interactibleHallucination.gameObject.SetActive(false);
        }
    }

    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameHandler))] public class GameHandlerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        GUIStyle groupStyle = new GUIStyle();
        groupStyle.normal.textColor = Color.white;
        groupStyle.fontSize = 20;

        GUIStyle titleStyle = new GUIStyle();
        titleStyle.normal.textColor = Color.black;
        titleStyle.fontSize = 17;

        GameHandler handler = target as GameHandler;

        serializedObject.Update();


        GUILayout.Label("Sanity Gestion", groupStyle);
        GUILayout.Label("Decrease vars", titleStyle);
        GUILayout.Space(5);
        CreatePropertyField(nameof(handler.sanityDecreaseRate));
        CreatePropertyField(nameof(handler.timeToDecreaseSanity));
        GUILayout.Space(20);

        GUILayout.Label("UI gestion", groupStyle);
        GUILayout.Label("UI menus vars", titleStyle);
        GUILayout.Space(5);
        CreatePropertyField(nameof(handler.pauseMenu));
        CreatePropertyField(nameof(handler.optionsMenu));
        CreatePropertyField(nameof(handler.saveMenu));
        CreatePropertyField(nameof(handler.inventoryMenu));
        GUILayout.Space(10);
        GUILayout.Label("Others", titleStyle);
        CreatePropertyField(nameof(handler.messageDisplayer));
        CreatePropertyField(nameof(handler.facelessGirlDamageIndicator));
        CreatePropertyField(nameof(handler.pickupItemsMessageColor));
        GUILayout.Space(5);
        GUILayout.Label("UI sounds", titleStyle);
        CreatePropertyField(nameof(handler.openingSavePauseMenu));
        CreatePropertyField(nameof(handler.closingSavePauseMenu));
        GUILayout.Space(5);

        serializedObject.ApplyModifiedProperties();
    }

    private void CreatePropertyField(string propertyName)
    {
        SerializedProperty sp = serializedObject.FindProperty(propertyName);
        EditorGUILayout.PropertyField(sp);
    }
}
#endif
