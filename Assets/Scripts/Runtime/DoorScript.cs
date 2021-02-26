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

    public Transform spawnPoint;

    public DoorScript otherDoorScript;
    public string otherDoorRoomName;

    public string currentRoomName;

    private RoomsManager roomsManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.GetChild(0);
        if(otherDoorScript != null)
        {
            otherDoorScript.otherDoorScript = this;
        }
        roomsManager = RoomsManager.instance;

        roomsManager.CheckIfRoomNameExists(currentRoomName);
        roomsManager.CheckIfRoomNameExists(otherDoorRoomName);
    }



    public void TryUseDoor(Transform Using)
    {
        if (needKey)
        {
            List<KeySO> keys = PlayerHelper.instance.GetPlayerKeys();
            foreach(KeySO key in keys)
            {
                Debug.Log(key.name);
                if(key.name == neededKey.name)
                {
                    needKey = false;
                    PlayerHelper.instance.RemoveObjectFromPlayerInvenotry(neededKey);

                    UseDoor(Using);

                    return;
                }
            }
        }
        else { UseDoor(Using); }
    }
    private void UseDoor(Transform Using)
    {
        otherDoorScript.currentRoomName = otherDoorRoomName;

        WorldProgress.instance.locationName = otherDoorRoomName;

        // Destroy enemies of the current room
        RoomsManager.instance.DestroyEnemiesOfRoom(currentRoomName);

        TransitionManager.instance.StartFade(Using, otherDoorScript.spawnPoint, 1, otherDoorScript, this);

        // Spawn enemies of the new room
        RoomsManager.instance.SpawnEnemiesOfRoom(otherDoorRoomName);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DoorScript))]
public class DoorScriptInspector : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

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

        CreatePropertyField(nameof(door.otherDoorRoomName));
        CreatePropertyField(nameof(door.currentRoomName));


        serializedObject.ApplyModifiedProperties();
    }
    private void CreatePropertyField(string propertyName)
    {
        SerializedProperty sp = serializedObject.FindProperty(propertyName);
        EditorGUILayout.PropertyField(sp);
    }
}
#endif
 