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
    }
}
