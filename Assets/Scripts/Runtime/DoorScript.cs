using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DoorScript : MonoBehaviour
{
    public bool needKey;
    public KeySO neededKey;

    public DoorScript otherDoorScript;
    public Transform SpawnPoint;
    public RoomManager roomManager;
    public int nextRoomInt;
    public int currentRoomInt;


    // Start is called before the first frame update
    void Start()
    {
        SpawnPoint = transform.Find("Spawn").transform;
        if(otherDoorScript != null)
        {
            otherDoorScript.otherDoorScript = this;
        }
        roomManager = RoomManager.instance;
    }

    public void TryUseDoor(Transform Using)
    {
        if (needKey)
        {
            List<KeySO> keys = PlayerHelper.instance.GetPlayerKeys();
            foreach(KeySO key in keys)
            {
                if(key.name == neededKey.name)
                {
                    needKey = false;

                    UseDoor(Using);

                    return;
                }
            }
        }
        else { UseDoor(Using); }
    }
    private void UseDoor(Transform Using)
    {
        otherDoorScript.currentRoomInt = nextRoomInt;

        WorldProgress.instance.locationName = roomManager.AllRooms[nextRoomInt].RoomName;

        // Destroy enemies of the current room
        RoomManager.instance.DestroyEnemies(currentRoomInt);

        TransitionManager.instance.StartFade(Using, otherDoorScript.SpawnPoint, 1, otherDoorScript, this);

        // Spawn enemies of the new room
        RoomManager.instance.SpawnEnemies(nextRoomInt);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DoorScript))]
public class DoorScriptInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DoorScript door = target as DoorScript;

        serializedObject.Update();

        CreatePropertyField(nameof(door.needKey));
        if (door.needKey) { CreatePropertyField(nameof(door.neededKey)); }

        GUILayout.Space(20);

        //        public DoorScript otherDoorScript;
        //public Transform SpawnPoint;
        //public RoomManager roomManager;
        //public int nextRoomInt;
        //public int currentRoomInt;
        CreatePropertyField(nameof(door.otherDoorScript));

        GUILayout.Space(10);

        CreatePropertyField(nameof(door.SpawnPoint));

        GUILayout.Space(10);

        CreatePropertyField(nameof(door.roomManager));

        GUILayout.Space(10);

        CreatePropertyField(nameof(door.nextRoomInt));
        CreatePropertyField(nameof(door.currentRoomInt));


        serializedObject.ApplyModifiedProperties();
    }
    private void CreatePropertyField(string propertyName)
    {
        SerializedProperty sp = serializedObject.FindProperty(propertyName);
        EditorGUILayout.PropertyField(sp);
    }
}
#endif
 