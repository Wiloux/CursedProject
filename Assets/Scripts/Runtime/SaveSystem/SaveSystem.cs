using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static string path = Application.persistentDataPath + "/playerinventory.data";
    public static void SaveWorldProgress(WorldProgress progress){
        BinaryFormatter formatter = new BinaryFormatter();
        
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(progress);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadWorldProgress(){
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else{
            Debug.LogError("File not found in " + path);
            return null;
        }
    }
}
