using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldProgress : MonoBehaviour
{
    public static WorldProgress instance;
    public bool isInventoryLoaded = false;


    // ------------------------------- VARS ----------------------------------------- //
    public float playerLife;


    // --------------------------------------- Monobehaviours Methods --------------------------------------- //
    private void Awake(){instance = this;}
    void Start(){
        LoadProgressData();
    }

    #region SaveMethods
    // --------------------------------------- SAVE METHODS ---------------------------------------- //

    public void SaveWorldProgress(){
        SaveSystem.SaveWorldProgress(this);
    }

    public void LoadProgressData(){
        isInventoryLoaded = true;
        SaveData data = SaveSystem.LoadWorldProgress();

        playerLife = data.playerLife;
    }

    #endregion
}