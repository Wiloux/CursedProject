using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogInteraction: MonoBehaviour
{
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
    }

    public void WwiseSoundManaging()
    {

    }
    public void WwiseBeforeNextSentence()
    {

    }
}
