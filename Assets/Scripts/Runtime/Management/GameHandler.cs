using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;

    #region Sanity vars
    // Sanity vars
    private float sanity = 0f;
    public float Sanity {
        get => sanity;
        set { sanity = value; sanityDecreaseTimer = timeToDecreaseSanity; }
    }

    private float sanityDecreaseTimer;
    [SerializeField] private float sanityDecreaseRate;
    [SerializeField] private float timeToDecreaseSanity;
    #endregion

    #region UI vars
    // UI vars
    public bool isPaused;
    private bool isSaveMenuOpen;
    private bool isInventoryMenuOpen;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject saveMenu;
    [SerializeField] private GameObject inventoryMenu;


    [SerializeField] private GameObject facelessGirlDamageIndicator;
    private float damageIndicatorTimer;

    #endregion

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        #region Sanity gestion
        if(sanity > 0)
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
        #endregion

        #region UI gestion
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
            ToggleMouseLock();
            if (isSaveMenuOpen){ToggleSaveMenu();}
            else if (isInventoryMenuOpen) ToggleInventoryMenu();
            else { TogglePauseMenu(); }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            TogglePause();
            ToggleMouseLock();
            ToggleInventoryMenu();
        }

        if(damageIndicatorTimer > 0)
        {
            damageIndicatorTimer -= Time.deltaTime;
            if(damageIndicatorTimer < 0)
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

        GUILayout.Label("Sanity : " + sanity.ToString(), style);
        
    }

    public bool IsPaused() { return isPaused; }

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

    public void TogglePauseMenu() { pauseMenu.SetActive(!pauseMenu.activeSelf);}
    public void ToggleSaveMenu() { saveMenu.SetActive(!saveMenu.activeSelf); isSaveMenuOpen = !isSaveMenuOpen; }
    public void ToggleInventoryMenu() { inventoryMenu.SetActive(!inventoryMenu.activeSelf); isInventoryMenuOpen = !isInventoryMenuOpen; }
    private void ToggleMouseLock() { if (MouseManagement.instance != null) MouseManagement.instance.ToggleMouseLock(); }

    public void DisplayFacelessGirlDamageIndicator()
    {
        facelessGirlDamageIndicator.SetActive(true);
        damageIndicatorTimer = 0.5f;
    }
}
