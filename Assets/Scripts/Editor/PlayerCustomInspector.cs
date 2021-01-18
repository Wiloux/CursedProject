using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerCustomInspector : Editor
{
    private string text;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        GUILayout.Space(15);

        Player player = target as Player;

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
