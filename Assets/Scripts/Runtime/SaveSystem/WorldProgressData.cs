using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldProgressData
{
    public float playerLife;
    public float gameTime;
    public string locationName;
    public string characterName;

    public bool[] isCutscenesPlayed;

    public WorldProgressData(WorldProgressSaver progress){
        playerLife = progress.playerLife;
        gameTime = progress.gameTime;
        locationName = progress.locationName;
        characterName = progress.characterName;

        isCutscenesPlayed = progress.isCutscenesPlayed;
    }
}
