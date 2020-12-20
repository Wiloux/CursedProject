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
    private AudioSource audioSource;

    private bool talking;

    private void Start()
    {
        dialogDisplayer = DialogueManager.instance.dialogDisplayer;
        audioSource = DialogueManager.instance.audioSource;
    }

    public void Talk()
    {
        if (talking) return;

        talking = true;
        dialogDisplayer.gameObject.SetActive(true);
        dialogDisplayer.text = "";
        StartCoroutine(TypeSentence());
    }

    private IEnumerator TypeSentence()
    {
        audioSource.clip = dialogue.dialoguesAndVoicelines[index].voiceline;
        audioSource.Play();
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
            audioSource.Stop();
            StartCoroutine(TypeSentence());
        }
        else
        {
            StopTalking();
        }
    }

    public void StopTalking()
    {
        talking = false;
        dialogDisplayer.gameObject.SetActive(false);
        index = 0;
    }
}
