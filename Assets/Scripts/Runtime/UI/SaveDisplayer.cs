using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveDisplayer : MonoBehaviour
{
    [SerializeField] private int indexSaveToDisplay;
    [Space]
    [SerializeField] private TMP_Text indexDisplayer;
    [Space]
    [SerializeField] private Image characterIconDisplayer;
    [SerializeField] private Sprite[] charactersIcons;
    [Space]
    [SerializeField] private TMP_Text locationDisplayer;
    [Space]
    [SerializeField] private TMP_Text gameTimeDisplayer;
    [Space]
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

        if (File.Exists(SaveSystem.worldProgressPaths[indexSaveToDisplay - 1]))
        {
            WorldProgressData data = SaveSystem.LoadWorldData(indexSaveToDisplay - 1);
            
            int characterIconIndex = 0;
            switch (data.characterName)
            {
                case "Judoka":
                    characterIconIndex = 0;
                    break;
                case "Nerd":
                    characterIconIndex = 1;
                    break;
                case "Weirdos":
                    characterIconIndex = 2;
                    break;
            }
            characterIconDisplayer.sprite = charactersIcons[characterIconIndex];

            locationDisplayer.text = data.locationName;
            string gameTime = ((int)data.gameTime).ToString();
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
