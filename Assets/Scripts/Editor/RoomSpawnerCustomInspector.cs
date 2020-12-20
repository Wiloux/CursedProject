using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomSpawner))]
public class RoomSpawnerCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("For each element of the list: put an Enemy on the scene and place it as you want and fill the element's 'enemy' field with it. Then press the 'Help' button, and you can now replace the 'enemy' fields by prefabs and delete the enemies you created on scene", MessageType.Info);
        base.OnInspectorGUI();

        serializedObject.Update();

        RoomSpawner roomSpawner = target as RoomSpawner;

        if (GUILayout.Button("Help"))
        {
            roomSpawner.HelpReferencement();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
