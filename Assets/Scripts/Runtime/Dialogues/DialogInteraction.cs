using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class DialogInteraction: MonoBehaviour
{
    [SerializeField] private bool hasToBeFocused;
    private string camState;
    [SerializeField] private DialogueSO dialogue;
    private int index = 0;
    [Tooltip("Script yield return this float between the writing of two letters")]
    private static float timeBetweenLetters = 0.04f;

    private TMP_Text dialogDisplayer;
    private AudioSource dialogAudioSource;
    private AudioSource sfxAudioSource;

    private string clue;

    private bool talking;

    private void Start()
    {
        dialogDisplayer = DialogueManager.instance.dialogDisplayer;
        dialogAudioSource = DialogueManager.instance.dialogAudioSource;
        sfxAudioSource = DialogueManager.instance.sfxAudioSource;
        clue = dialogue.clueName;
    }

    public void Talk()
    {
        if (talking) return;

        if(hasToBeFocused) SetCameraWatchingPlayer();

        PlayerHelper.instance.ToggleControls();
        talking = true;
        dialogDisplayer.gameObject.SetActive(true);
        dialogDisplayer.text = "";
        StartCoroutine(TypeSentence());
    }

    private IEnumerator TypeSentence()
    {
        if (dialogue.dialoguesAndVoicelines[index].dialogueStart != null){dialogue.dialoguesAndVoicelines[index].dialogueStart.Post(gameObject);}

        foreach (char character in dialogue.dialoguesAndVoicelines[index].script.ToCharArray())
        {
            dialogDisplayer.text += character;
            yield return new WaitForSeconds(timeBetweenLetters);
        }
    }

    public void NextSentence()
    {
        if (hasToBeFocused)
        {
            if(camState == "this") { SetCameraWatchingPlayer(); }
            else { SetCameraWatchingThis(); }
        }
        dialogDisplayer.text = "";
        if(index < dialogue.dialoguesAndVoicelines.Count - 1)
        {
            index++;
            StopAllCoroutines();
            if(dialogAudioSource.isPlaying) dialogAudioSource.Stop();
            if(sfxAudioSource.isPlaying) sfxAudioSource.Stop();
            StartCoroutine(TypeSentence());
        }
        else
        {
            if(dialogue.dialoguesAndVoicelines[index].dialogueEnd != null){ dialogue.dialoguesAndVoicelines[index].dialogueEnd.Post(gameObject);}
            StopTalking();
        }
    }

    public void StopTalking()
    {
        PlayerHelper.instance.ToggleControls();
        talking = false;
        dialogDisplayer.gameObject.SetActive(false);
        index = 0;

        if(clue != "")
        {
            PlayerHelper.instance.AddClueToInventory(clue);
            Destroy(gameObject);
        }

        Camera mainCam = Camera.main;
        mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled = true;
    }

    private void SetCameraWatchingPlayer()
    {
        camState = "player";
        Transform player = PlayerHelper.instance.transform;
        Camera mainCam = Camera.main;
        mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
        mainCam.transform.position = player.position + player.forward * 5f + player.up * 1f + player.right * 0.5f;
        mainCam.transform.rotation = Quaternion.LookRotation((player.position - mainCam.transform.position).normalized);
    }
    private void SetCameraWatchingThis()
    {
        Camera mainCam = Camera.main;
        mainCam.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
        mainCam.transform.position = transform.position + transform.forward * 5f + transform.up * 1f + transform.right * 0.5f;
        mainCam.transform.rotation = Quaternion.LookRotation((transform.position - mainCam.transform.position).normalized);
        camState = "this";
    }
}
