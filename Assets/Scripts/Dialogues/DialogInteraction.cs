using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogInteraction: MonoBehaviour
{
    [SerializeField] private DialogueSO dialogue;
    private int index = 0;
    [Tooltip("Script yield return this float between the writing of two letters")]
    private static float timeBetweenLetters = 0.02f;

    private TMP_Text dialogDisplayer;
    private AudioSource dialogAudioSource;
    private AudioSource sfxAudioSource;

    private bool talking;

    private void Start()
    {
        dialogDisplayer = DialogueManager.instance.dialogDisplayer;
        dialogAudioSource = DialogueManager.instance.dialogAudioSource;
        sfxAudioSource = DialogueManager.instance.sfxAudioSource;
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
        if(dialogue.dialoguesAndVoicelines[index].SFX != null)
        {
            sfxAudioSource.clip = dialogue.dialoguesAndVoicelines[index].SFX;
            sfxAudioSource.Play();
        }
        if(dialogue.dialoguesAndVoicelines[index].voiceline != null)
        {
            dialogAudioSource.clip = dialogue.dialoguesAndVoicelines[index].voiceline;
            dialogAudioSource.Play();
        }
        foreach(char character in dialogue.dialoguesAndVoicelines[index].script.ToCharArray())
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
            StopTalking();
        }
    }

    public void StopTalking()
    {
        PlayerHelper.instance.ToggleControls();
        talking = false;
        dialogDisplayer.gameObject.SetActive(false);
        index = 0;
    }

    public void WwiseSoundManaging()
    {

    }
    public void WwiseBeforeNextSentence()
    {

    }
}
