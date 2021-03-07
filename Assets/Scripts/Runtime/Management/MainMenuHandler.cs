using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public static MainMenuHandler instance;
    [SerializeField] private GameObject pressButtonMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject loadSavesMenu;
    [SerializeField] private GameObject newGameMenu;

    [Space(10)]
    [SerializeField] private AK.Wwise.Event onKeyPressedWEvent;

    private bool hasPressed;

    private void Awake(){ instance = this; }

    private void Start()
    {
        pressButtonMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    private void Update()
    {
        if (!hasPressed)
        {
            if (Input.anyKeyDown)
            {
                OpenMainMenu();
                hasPressed = true;
                onKeyPressedWEvent?.Post(gameObject);
            }
        }
        else
        {

        }
    }

    public void OpenMainMenu()
    {
        pressButtonMenu.SetActive(false);
        optionsMenu.SetActive(false);
        loadSavesMenu.SetActive(false);
        newGameMenu.SetActive(false);

        mainMenu.SetActive(true);
    }

    public void OpenOptionsMenu()
    {
        mainMenu.SetActive(false);
        pressButtonMenu.SetActive(false);
        loadSavesMenu.SetActive(false);
        newGameMenu.SetActive(false);
        
        optionsMenu.SetActive(true);
    }

    public void OpenLoadGameMenu()
    {
        mainMenu.SetActive(false);
        pressButtonMenu.SetActive(false);
        optionsMenu.SetActive(false);
        newGameMenu.SetActive(false);
        
        loadSavesMenu.SetActive(true);
    }

    public void OpenNewGameMenu()
    {
        mainMenu.SetActive(false);
        pressButtonMenu.SetActive(false);
        loadSavesMenu.SetActive(false);
        optionsMenu.SetActive(false);

        newGameMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
