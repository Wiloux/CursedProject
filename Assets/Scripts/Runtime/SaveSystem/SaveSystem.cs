using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static string[] paths =
    {
        Application.persistentDataPath + "/Save01.data",
        Application.persistentDataPath + "/Save02.data",
        Application.persistentDataPath + "/Save03.data",
        Application.persistentDataPath + "/Save04.data",
        Application.persistentDataPath + "/Save05.data",
        Application.persistentDataPath + "/Save06.data"
    };

    public static void SaveWorldData(WorldProgress progress, int saveIndex){
        BinaryFormatter formatter = new BinaryFormatter();
        
        FileStream stream = new FileStream(paths[saveIndex], FileMode.Create);

        SaveData data = new SaveData(progress);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadWorldData(int saveIndex){
        if(File.Exists(paths[saveIndex])){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(paths[saveIndex], FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else{
            Debug.LogError("File not found in " + paths[saveIndex]);
            return null;
        }
    }

    public static void DeleteWorldData(int saveIndex)
    {
        if (File.Exists(paths[saveIndex]))
        {
            File.Delete(paths[saveIndex]);
        }
    }
}
