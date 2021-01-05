using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float playerLife;

    public SaveData(WorldProgress progress){
        playerLife = progress.playerLife;
    }
}
