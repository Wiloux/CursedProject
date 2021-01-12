using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float playerLife;
    public float gameTime;
    public string locationName;
    public string characterName;

    public bool[] isCutscenesPlayed;

    public SaveData(WorldProgress progress){
        playerLife = progress.playerLife;
        gameTime = progress.gameTime;
        locationName = progress.locationName;
        characterName = progress.characterName;

        isCutscenesPlayed = progress.isCutscenesPlayed;
    }
}
