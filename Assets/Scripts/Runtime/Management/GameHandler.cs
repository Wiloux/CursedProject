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

    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    public GameObject facelessGirlDamageIndicator;
    private float damageIndicatorTimer;
    private IEnumerator gradualIncreaseCoroutine;
    private bool graduallyIncreasing;

    private Material DmgIndMat;
    private float DmgIndMax;

    #endregion

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

    // Update is called once per frame
    void Update()
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

        AkSoundEngine.SetRTPCValue("HallucinationsRTPC", Mathf.Clamp(sanity, 0f, 100f), gameObject); // Here Wwise stuffs
        #endregion

        #region UI gestion
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
            ToggleMouseLock();
            if (isSaveMenuOpen) { ToggleSaveMenu(); }
            else if (isInventoryMenuOpen) ToggleInventoryMenu();
            else { TogglePauseMenu(); }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            TogglePause();
            ToggleMouseLock();
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

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 50;
        style.normal.textColor = Color.white;

        //GUILayout.Label("Sanity : " + sanity.ToString(), style);
        GUILayout.Label("Faceless girl DamageIndicatorTimer : " + damageIndicatorTimer.ToString(), style);

        // on s'en blc c pour le debug
    }
    #endregion

    public bool IsPaused() { return isPaused; }

    #region Toggle functions
    public void TogglePause()
    {
        if (isPaused) Time.timeScale = 1f;
        else Time.timeScale = 0f;
        isPaused = !isPaused;
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
    private void ToggleMouseLock() { if (MouseManagement.instance != null) MouseManagement.instance.ToggleMouseLock(); }

    #endregion

    #region UI functions
    #region Toggle menus
    public void TogglePauseMenu() { PlayCloseOrOpenMenuSound(pauseMenu); pauseMenu.SetActive(!pauseMenu.activeSelf); }
    public void ToggleSaveMenu() { PlayCloseOrOpenMenuSound(pauseMenu); saveMenu.SetActive(!saveMenu.activeSelf); isSaveMenuOpen = !isSaveMenuOpen; }
    public void ToggleInventoryMenu() { inventoryMenu.SetActive(!inventoryMenu.activeSelf); isInventoryMenuOpen = !isInventoryMenuOpen; }
    #endregion

    #region Func for UI buttons
    public void LeftPauseMenu() { ToggleMouseLock(); TogglePause(); TogglePauseMenu(); }
    public void QuitApplication() { Application.Quit(); }

    public void OpenOptionsFromPauseMenu() { pauseMenu.SetActive(false); optionsMenu.SetActive(true); }
    public void OpenPauseMenuFromOptions() { pauseMenu.SetActive(true); optionsMenu.SetActive(false); }
    #endregion
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
        graduallyIncreasing = true;
    }
    private IEnumerator IncreaseGraduallyTimer()
    {
        while (damageIndicatorTimer < 0.7f)
        {
            damageIndicatorTimer += 2 * Time.deltaTime;
            yield return null;
        }
    }

    private void PlayCloseOrOpenMenuSound(GameObject menu)
    {
        if (menu.activeSelf) closingSavePauseMenu?.Post(listener);
        else openingSavePauseMenu?.Post(listener);
    }
    #endregion

    public void SetVolume() //
    {
        float musicVolume = musicVolumeSlider.value; // the values are between 0 and 10 miguel ^^
        float sfxVolume = sfxVolumeSlider.value;
    }
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
        GUILayout.Label("Options UI vars", titleStyle);
        GUILayout.Space(5);
        CreatePropertyField(nameof(handler.musicVolumeSlider));
        CreatePropertyField(nameof(handler.sfxVolumeSlider));
        GUILayout.Space(10);
        GUILayout.Label("Others", titleStyle);
        CreatePropertyField(nameof(handler.facelessGirlDamageIndicator));
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
