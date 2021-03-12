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
        WorldProgressSaver.instance.SaveWorldProgress(indexToSaveOn);
        Debug.Log("saved");
        gameObject.SetActive(false);

        string roomName = WorldProgressSaver.instance.locationName;
        RoomsManager.instance.DestroyEnemiesOfRoom(roomName);
        RoomsManager.instance.SpawnEnemiesOfRoom(roomName);

        PlayerHelper.instance.FillUpHealInBag();
    }
}
