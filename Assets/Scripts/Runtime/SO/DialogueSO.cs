using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public List<DialogueAndVoiceline> dialoguesAndVoicelines;
}


[Serializable] public class DialogueAndVoiceline
{
    [TextArea(1,7)] public string script;
    [Space]
    public AudioClip voiceline;
    public AudioClip SFX;
    public AK.Wwise.Event dialogueStart;
    public AK.Wwise.Event dialogueEnd;

    public DialogueAndVoiceline(string script, AudioClip voiceline, AudioClip SFX)
    {
        this.script = script;
        this.voiceline = voiceline;
        this.SFX = SFX;
    }
}
