using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class SaveDisplayer : MonoBehaviour
{
    [SerializeField] private int indexSaveToDisplay;

    [SerializeField] private TMP_Text indexDisplayer;
    [SerializeField] private TMP_Text locationDisplayer;
    [SerializeField] private TMP_Text gameTimeDisplayer;

    [SerializeField] private SaveConfirmationCheck saveConfirmationCheck;

    private void OnEnable()
    {
        UpdateDisplay();
        InvokeRepeating("UpdateDisplay", 1f,1);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }

    public void UpdateDisplay()
    {
        indexDisplayer.text = "0" + indexSaveToDisplay.ToString() + ".";

        if (File.Exists(SaveSystem.paths[indexSaveToDisplay - 1]))
        {
            SaveData data = SaveSystem.LoadWorldData(indexSaveToDisplay - 1);
            locationDisplayer.text = data.locationName;
            string gameTime = data.gameTime.ToString();
            if (data.gameTime > 60) { gameTime = ((int)data.gameTime / 60).ToString() + ":" + ((int)data.gameTime % 60).ToString(); }
            gameTimeDisplayer.text = gameTime;
        }
        else
        {
            locationDisplayer.text = " - - - - - - - - - - - - - - - - ";
            gameTimeDisplayer.text = "";
        }
    }

    public void AskConfirmation()
    {
        saveConfirmationCheck.gameObject.SetActive(true);
        saveConfirmationCheck.indexToSaveOn = indexSaveToDisplay-1;
    }
}
