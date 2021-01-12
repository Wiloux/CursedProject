using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player_Movement))]
public class PlayerMovementCustomInspector : Editor
{
    private string text;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        Player_Movement player = target as Player_Movement;

        EditorGUILayout.BeginHorizontal();
        text = EditorGUILayout.TextField(text);
        if(GUILayout.Button("Play playerHitEvent"))
        {
            player.OnHit(text);
        }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
