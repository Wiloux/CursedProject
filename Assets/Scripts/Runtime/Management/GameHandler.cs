using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;

    public bool isPaused;
    private bool isSaveMenuOpen;
    private bool isInventoryMenuOpen;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject saveMenu;
    [SerializeField] private GameObject inventoryMenu;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
            if (isSaveMenuOpen){ToggleSaveMenu();}
            if (isInventoryMenuOpen) ToggleInventoryMenu();
            else { TogglePauseMenu(); }
            if(MouseManagement.instance != null) MouseManagement.instance.ToggleMouseLock();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventoryMenu();
        }
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
    public void ToggleInventoryMenu() { inventoryMenu.SetActive(!inventoryMenu.activeSelf); isInventoryMenuOpen = !isInventoryMenuOpen; TogglePause(); }
    private void ToggleMouseLock() { if (MouseManagement.instance != null) MouseManagement.instance.ToggleMouseLock(); }
}
