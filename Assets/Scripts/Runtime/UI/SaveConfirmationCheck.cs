using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveConfirmationCheck : MonoBehaviour
{
    public static SaveConfirmationCheck instance;
    public int indexToSaveOn;

    private void Awake()
    {
        instance = this;
    }

    public void Save()
    {
        WorldProgress.instance.SaveWorldProgress(indexToSaveOn);
        Debug.Log("saved");
        gameObject.SetActive(false);

        string roomName = WorldProgress.instance.locationName;
        RoomsManager.instance.DestroyEnemiesOfRoom(roomName);
        RoomsManager.instance.SpawnEnemiesOfRoom(roomName);

        PlayerHelper.instance.FillUpHealInBag();
    }
}
