using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDialogManager : MonoBehaviour
{
    private DialogInteraction currentDialog;
    private TMP_Text dialogDisplayer;

    private bool talking;

    private void Start()
    {
        dialogDisplayer = DialogueManager.instance.dialogDisplayer;
    }
    // Update is called once per frame
    void Update()
    {
        if (talking)
        {
            if (!dialogDisplayer.gameObject.activeSelf) talking = false;
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
                DialogInteraction dialogInteraction= hit.transform.GetComponent<DialogInteraction>();
                if (dialogInteraction != null)
                {
                    currentDialog = dialogInteraction;
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
