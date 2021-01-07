using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldProgress : MonoBehaviour
{
    public static WorldProgress instance;
    public bool isInventoryLoaded = false;

    // ------------------------------- VARS ----------------------------------------- //
    public float playerLife = 1;
    public float gameTime = 0;
    public string locationName = "Spawn";
    public string characterName = "Nerd";

    #region Monobehaviours Methods
    // --------------------------------------- Monobehaviours Methods --------------------------------------- //
    private void Awake(){instance = this; DontDestroyOnLoad(this.gameObject); }

    private void Update() 
    { 
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (!GameHandler.instance.isPaused) gameTime += Time.deltaTime; 
        }
    }
    #endregion

    #region SaveMethods
    // --------------------------------------- SAVE METHODS ---------------------------------------- //

    public void SaveWorldProgress(int saveIndex){
        SaveSystem.SaveWorldData(this, saveIndex);
        Debug.Log(locationName);
    }

    public void LoadProgressData(int saveIndex){
        isInventoryLoaded = true;
        SaveData data = SaveSystem.LoadWorldData(saveIndex);

        playerLife = data.playerLife;
        gameTime = data.gameTime;
        locationName = data.locationName;
        characterName = data.characterName;
    }

    public void ReplaceProgressData(int saveIndex)
    {
        SaveSystem.DeleteWorldData(saveIndex);
        SaveWorldProgress(saveIndex);
    }
    #endregion
}