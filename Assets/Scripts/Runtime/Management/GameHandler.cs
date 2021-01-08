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

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject saveMenu;

    private void Awake(){instance = this;}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
            MouseManagement.instance.ToggleMouseLock();
            if (isSaveMenuOpen)
            {
                ToggleSaveMenu();
            }
            else { TogglePauseMenu(); }
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        GameObject[] objects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach(GameObject go in objects)
        {
            switch (go.tag)
            {
                case "Enemy":
                    EnemyBase enemy = go.GetComponent<EnemyBase>();
                    if(enemy != null){EnemyHelper.Pause(enemy);}
                    else{
                        for(int i = 0; i < go.transform.childCount;i ++)
                        {
                            enemy = go.transform.GetChild(i).GetComponent<EnemyBase>();
                            if(enemy != null) { EnemyHelper.Pause(enemy); }
                        }
                    }
                    break;
                case "Player":
                    PlayerHelper.instance.ToggleControls();
                    break;
                case "Mirror":
                    MirrorCam mirroCam = go.GetComponent<MirrorCam>();
                    if (mirroCam != null) mirroCam.enabled = !mirroCam.enabled;
                    break;

            }
        }
    }

    public void TogglePauseMenu() { pauseMenu.SetActive(!pauseMenu.activeSelf);}
    public void ToggleSaveMenu() { saveMenu.SetActive(!saveMenu.activeSelf); isSaveMenuOpen = !isSaveMenuOpen; }
}
