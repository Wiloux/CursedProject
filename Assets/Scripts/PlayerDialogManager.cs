using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDialogManager : MonoBehaviour
{
    [SerializeField] private TMP_Text dialog;
    private DialogManager currentDialog;

    private bool talking;
    // Update is called once per frame
    void Update()
    {
        if (talking)
        {
            if (!dialog.gameObject.activeSelf) talking = false;
            else if (Input.anyKeyDown)
            {
                currentDialog.NextSentence();
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit, 5f);
            if(hit.transform != null)
            {
                DialogManager dialogManager = hit.transform.GetComponent<DialogManager>();
                if (dialogManager != null)
                {
                    currentDialog = dialogManager;
                    currentDialog.Talk();
                    talking = true;
                }
                else
                {
                    Debug.LogWarning("No Dialog Manager there");
                }
            }
        }
    }
}
