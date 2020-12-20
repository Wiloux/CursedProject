using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public AudioSource dialogAudioSource;
    public AudioSource sfxAudioSource;
    public TMP_Text dialogDisplayer;

    public static DialogueManager instance;
    private void Awake()
    {
        instance = this;
    }
}
