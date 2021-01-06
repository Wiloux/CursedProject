using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldProgress : MonoBehaviour
{
    public static WorldProgress instance;
    public bool isInventoryLoaded = false;

    // ------------------------------- VARS ----------------------------------------- //
    public float playerLife;
    public float gameTime;
    public string locationName;
    public string characterName;

    #region Monobehaviours Methods
    // --------------------------------------- Monobehaviours Methods --------------------------------------- //
    private void Awake(){instance = this;}

    private void Update() { if (!GameHandler.instance.isPaused) gameTime += Time.deltaTime; }
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

    #endregion
}