using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private TMP_Text dialog;
    [TextArea]
    [SerializeField] private string[] sentences;
    private int index = 0;
    [Tooltip("Script yield return this float between the writing of two letters")]
    private static float timeBetweenLetters = 0.02f;

    private bool talking;

    private void Start()
    {
        if(dialog == null) { dialog = GameObject.Find("Dialog").GetComponent<TMP_Text>(); }
    }

    public void Talk()
    {
        if (talking) return;

        talking = true;
        dialog.gameObject.SetActive(true);
        dialog.text = "";
        StartCoroutine(TypeSentence());
    }

    private IEnumerator TypeSentence()
    {
        foreach(char character in sentences[index].ToCharArray())
        {
            dialog.text += character;
            yield return new WaitForSeconds(timeBetweenLetters);
        }
    }

    public void NextSentence()
    {
        dialog.text = "";
        if(index < sentences.Length - 1)
        {
            index++;
            StopAllCoroutines();
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
        dialog.gameObject.SetActive(false);
        index = 0;
    }
}
