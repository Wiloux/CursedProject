using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldProgressSaver : MonoBehaviour
{
    public static WorldProgressSaver instance;
    public bool isInventoryLoaded = false;

    // ------------------------------- VARS ----------------------------------------- //
    public float playerLife = 1;
    public float gameTime = 0;
    public string locationName = "Spawn";
    public string characterName = "Nerd";

    public bool[] isCutscenesPlayed;

    #region Monobehaviours Methods
    // --------------------------------------- Monobehaviours Methods --------------------------------------- //
    private void Awake(){instance = this; DontDestroyOnLoad(transform.root.gameObject); }

    private void Start()
    {
        Initialize();
    }

    private void Update() 
    { 
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            GameHandler gameHandler = GameHandler.instance;
            if(gameHandler != null) if (!gameHandler.isPaused) gameTime += Time.deltaTime; 
        }
    }
    #endregion

    private void Initialize()
    {
        playerLife = 1;
        gameTime = 0;
        if (RoomsManager.instance != null) locationName = RoomsManager.instance.AllRooms[0].roomName;
        else
        {
            if (RoomsManager.instance != null) locationName = RoomsManager.instance.GetRoomName(0);
            else locationName = "default";
        }
        characterName = "Gyaru";

        isCutscenesPlayed = new bool[5];
    }

    #region Save Functions
    // --------------------------------------- SAVE METHODS ---------------------------------------- //


    public void SaveWorldProgress(int saveIndex){
        SaveSystem.SaveWorldData(this, saveIndex);
        Debug.Log(locationName);
    }

    public void LoadProgressData(int saveIndex){
        isInventoryLoaded = true;
        WorldProgressData data = SaveSystem.LoadWorldData(saveIndex);

        playerLife = data.playerLife;
        gameTime = data.gameTime;
        locationName = data.locationName;
        characterName = data.characterName;

        isCutscenesPlayed = data.isCutscenesPlayed;
    }

    public void ReplaceProgressData(int saveIndex)
    {
        SaveSystem.DeleteWorldData(saveIndex);
        SaveWorldProgress(saveIndex);
    }
    #endregion
}