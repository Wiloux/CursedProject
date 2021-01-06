using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;

    private bool isPaused;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject saveMenu;

    private void Awake(){instance = this;}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
            TogglePauseMenu();
            if (saveMenu.activeSelf) { ToggleSaveMenu(); }
        }
    }

    private void OnGUI()
    {
        if(isPaused)GUI.Label(new Rect(50, 50, Screen.width / 2, Screen.width / 4), "Pause");
    }

    public void TogglePause()
    {
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

            }
        }

        if (Cursor.lockState == CursorLockMode.Locked) { Cursor.lockState = CursorLockMode.None; }
        else Cursor.lockState = CursorLockMode.None;
    }

    public void TogglePauseMenu() { pauseMenu.SetActive(!pauseMenu.activeSelf);}
    public void ToggleSaveMenu() { saveMenu.SetActive(!saveMenu.activeSelf); }
}
