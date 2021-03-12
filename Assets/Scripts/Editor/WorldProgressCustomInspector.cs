using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldProgressSaver))]
public class WorldProgressCustomInspector : Editor
{
    private int indexSaveToLoad = 1;
    private int indexToSaveOn = 1;
    private int indexSaveToRemove = 1;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WorldProgressSaver worldProgress = target as WorldProgressSaver;

        #region Load Index Save
        EditorGUILayout.BeginHorizontal();
        indexSaveToLoad = EditorGUILayout.IntField(indexSaveToLoad);
        if(GUILayout.Button("Load index save"))
        {
            worldProgress.LoadProgressData(indexSaveToLoad-1);
        }
        EditorGUILayout.EndHorizontal();
        #endregion

        GUILayout.Space(10);

        #region SaveOnIndex
        EditorGUILayout.BeginHorizontal();
        indexToSaveOn = EditorGUILayout.IntField(indexToSaveOn);
        if(GUILayout.Button("Save on index"))
        {
            worldProgress.SaveWorldProgress(indexToSaveOn-1);
        }
        EditorGUILayout.EndHorizontal();
        #endregion

        GUILayout.Space(10);

        #region Remove/Erase save
        EditorGUILayout.BeginHorizontal();
        indexSaveToRemove = EditorGUILayout.IntField(indexSaveToRemove);
        if(GUILayout.Button("Remove index save"))
        {
            WorldProgressData data = SaveSystem.LoadWorldData(indexSaveToRemove-1);

            data.playerLife = 1;
            data.gameTime = 0;
            data.locationName = "Spawn";

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(SaveSystem.worldProgressPaths[indexSaveToRemove - 1], FileMode.Create);
            formatter.Serialize(stream, data);
            stream.Close();

            if (File.Exists(SaveSystem.worldProgressPaths[indexSaveToRemove - 1])) { File.Delete(SaveSystem.worldProgressPaths[indexSaveToRemove - 1]); }
        }
        EditorGUILayout.EndHorizontal();
        #endregion
    }
}
