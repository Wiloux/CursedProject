using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Collectible : MonoBehaviour
{
    public bool healingItem;
    public int healValues;

    public ObjectSO so;
}

#if UNITY_EDITOR
[CustomEditor(typeof(Collectible))] public class CollectibleInspector : Editor
{
    public override void OnInspectorGUI()
    {
        Collectible collectible = target as Collectible;

        serializedObject.Update();

        CreatePropertyField(nameof(collectible.healingItem));

        if (!collectible.healingItem)
        {
            GUILayout.Space(10);
            CreatePropertyField(nameof(collectible.so));
        }
        else { CreatePropertyField(nameof(collectible.healValues)); collectible.so = null; }

        serializedObject.ApplyModifiedProperties();
    }

    private void CreatePropertyField(string propertyName)
    {
        SerializedProperty sp = serializedObject.FindProperty(propertyName);
        EditorGUILayout.PropertyField(sp);
    }
}
#endif
