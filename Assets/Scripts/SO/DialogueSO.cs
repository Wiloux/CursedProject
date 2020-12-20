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

    public DialogueAndVoiceline(string script, AudioClip voiceline)
    {
        this.script = script;
        this.voiceline = voiceline;
    }
}
