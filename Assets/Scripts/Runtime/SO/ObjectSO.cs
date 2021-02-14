using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Scriptable Objects/ObjectInfos")]
public class ObjectSO : ScriptableObject
{
    public string objectName;
    public Sprite objectSprite;
    public GameObject objectModel;

    public Vector3 previewPosition;
    public Vector3 previewRotation;


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

        CreatePropertyField(nameof(so.objectName));
        CreatePropertyField(nameof(so.objectSprite));

        if(so.objectSprite != null)
        {
            GUILayout.Space(5);
            Rect rect = GUILayoutUtility.GetLastRect();
            Debug.Log(rect);
            EditorGUI.DrawPreviewTexture(new Rect(rect.x + EditorGUIUtility.currentViewWidth/2 - so.objectSprite.texture.width/2, rect.y + 0.1f * so.objectSprite.texture.height, so.objectSprite.texture.width, so.objectSprite.texture.height), so.objectSprite.texture);
            GUILayout.Space(so.objectSprite.texture.height * 1.2f);
        }

        CreatePropertyField(nameof(so.objectModel));
        if(so.objectModel != null)
        {
            GUILayout.Space(10);

            GUIStyle bgColor = new GUIStyle();
            bgColor.normal.background = EditorGUIUtility.whiteTexture;

            previewEditor = Editor.CreateEditor(so.objectModel);
            previewEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(256, 256), bgColor);
        }

        CreatePropertyField(nameof(so.shortDescription));
        CreatePropertyField(nameof(so.longDescription));

        GUILayout.Space(20);

        CreatePropertyField(nameof(so.type));
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