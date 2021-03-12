using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static string[] worldProgressPaths =
    {
        Application.persistentDataPath + "/Save01.data",
        Application.persistentDataPath + "/Save02.data",
        Application.persistentDataPath + "/Save03.data",
        Application.persistentDataPath + "/Save04.data",
        Application.persistentDataPath + "/Save05.data",
        Application.persistentDataPath + "/Save06.data"
    };
    public static string optionsSavePath = Application.persistentDataPath + "/options.data";

    #region World Progress
    public static void SaveWorldData(WorldProgressSaver progress, int saveIndex){
        BinaryFormatter formatter = new BinaryFormatter();
        
        FileStream stream = new FileStream(worldProgressPaths[saveIndex], FileMode.Create);

        WorldProgressData data = new WorldProgressData(progress);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static WorldProgressData LoadWorldData(int saveIndex){
        if(File.Exists(worldProgressPaths[saveIndex])){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(worldProgressPaths[saveIndex], FileMode.Open);

            WorldProgressData data = formatter.Deserialize(stream) as WorldProgressData;
            stream.Close();
            return data;
        }
        else{
            Debug.LogError("World progress data file not found in " + worldProgressPaths[saveIndex]);
            return null;
        }
    }

    public static void DeleteWorldData(int saveIndex)
    {
        if (File.Exists(worldProgressPaths[saveIndex]))
        {
            File.Delete(worldProgressPaths[saveIndex]);
        }
    }
    #endregion

    #region Options
    public static void SaveOptionsData(OptionsSaver options)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(optionsSavePath, FileMode.Create);

        OptionsData data = new OptionsData(options);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static OptionsData LoadOptionsData()
    {
        if (File.Exists(optionsSavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(optionsSavePath, FileMode.Open);

            OptionsData data = formatter.Deserialize(stream) as OptionsData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Options data file not found in " + optionsSavePath);
            return null;
        }
    }
    #endregion
}
