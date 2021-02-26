using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Object")]
public class ObjectSO : ScriptableObject
{
    public new string name;
    public GameObject objectModel;

    public Vector3 previewStartRotation;
    public Vector3 previewRotation;

    public float scaleValue = 250f;

    public string shortDescription;
    [TextArea(1,7)] public string longDescription;

    public enum TypeOfObject
    {
        key,
        clue,
        item,
        useless,
    }
    public TypeOfObject type;

    public KeySO key;
}

#if UNITY_EDITOR
[CustomEditor(typeof(ObjectSO))]
public class ObjectSOInspector : Editor
{
    Editor previewEditor;
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        ObjectSO so = target as ObjectSO;

        serializedObject.Update();

        CreatePropertyField(nameof(so.name));

        CreatePropertyField(nameof(so.objectModel));
        if(so.objectModel != null)
        {
            GUILayout.Space(10);

            GUIStyle bgColor = new GUIStyle();
            bgColor.normal.background = EditorGUIUtility.whiteTexture;
            if(previewEditor == null) { previewEditor = Editor.CreateEditor(so.objectModel);}
            
            previewEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(256, 256), bgColor);
        }

        CreatePropertyField(nameof(so.previewStartRotation));
        CreatePropertyField(nameof(so.previewRotation));
        CreatePropertyField(nameof(so.scaleValue));

        CreatePropertyField(nameof(so.shortDescription));
        CreatePropertyField(nameof(so.longDescription));

        GUILayout.Space(20);

        CreatePropertyField(nameof(so.type));
        if(so.type == ObjectSO.TypeOfObject.key)
        {
            CreatePropertyField(nameof(so.key));
        }
        GUILayout.Space(250);
        GUILayout.Label(" ");

        serializedObject.ApplyModifiedProperties();
    }

    private void CreatePropertyField(string propertyName)
    {
        SerializedProperty sp = serializedObject.FindProperty(propertyName);
        EditorGUILayout.PropertyField(sp);
    }
}
#endif