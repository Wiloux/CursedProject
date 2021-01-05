using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public string clueName;
    public List<DialogueAndVoiceline> dialoguesAndVoicelines;
}


[Serializable] public class DialogueAndVoiceline
{
    [TextArea(1,7)] public string script;

    [Space]
    public AK.Wwise.Event dialogueStart;
    public AK.Wwise.Event dialogueEnd;

    public DialogueAndVoiceline(string script, AK.Wwise.Event dialogueStart, AK.Wwise.Event dialogueEnd)
    {
        this.script = script;
        this.dialogueStart = dialogueStart;
        this.dialogueEnd = dialogueEnd;
    }
}
