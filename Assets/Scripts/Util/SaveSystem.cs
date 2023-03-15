using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;

public static class SaveSystem
{
    // For Level Data
    public static void SaveLevelData(LevelSaveData levelSaveData, bool isSinglePlayer)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = "";
            
        if (isSinglePlayer)
            path = Application.persistentDataPath + "/SinglePlayer/level" + levelSaveData.levelIndex.ToString() + ".data";
        else
            path = Application.persistentDataPath + "/DoublePlayer/level" + levelSaveData.levelIndex.ToString() + ".data";

        new FileInfo(path).Directory.Create();

        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

        formatter.Serialize(stream, levelSaveData);
        stream.Close();
    }

    public static LevelSaveData LoadLevelData(LevelSaveData levelSaveData, bool isSinglePlayer)
    {
        string path = "";

        if (isSinglePlayer)
            path = Application.persistentDataPath + "/SinglePlayer/level" + levelSaveData.levelIndex.ToString() + ".data";
        else
            path = Application.persistentDataPath + "/DoublePlayer/level" + levelSaveData.levelIndex.ToString() + ".data";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelSaveData savedData = formatter.Deserialize(stream) as LevelSaveData;
            stream.Close();

            return savedData;
        }
        Debug.LogWarning("Save file at the path: '" + path + "' was not found. null was returned");
        return null;
    }

    public static LevelSaveData LoadLevelDataThroughBuildIndex(int index, bool isSinglePlayer)
    {
        string path = "";

        if (isSinglePlayer)
            path = Application.persistentDataPath + "/SinglePlayer/level" + index.ToString() + ".data";
        else
            path = Application.persistentDataPath + "/DoublePlayer/level" + index.ToString() + ".data";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelSaveData savedData = formatter.Deserialize(stream) as LevelSaveData;
            stream.Close();

            return savedData;
        }
        Debug.LogError("Save file at the path: '" + path + "' was not found.");
        return null;
    }

    public static TimeSpan LoadLevelScore(LevelSaveData levelSaveData, bool isSinglePlayer)
    {
        double seconds = LoadLevelData(levelSaveData, isSinglePlayer).seconds;
        return TimeSpan.FromSeconds(seconds);
    }

    // For Debugging Level Save Data
    public static void DeleteLevelSaveFile(LevelHandler levelHandler)
    {
        string path = Application.persistentDataPath + "/level" + levelHandler.buildIndex.ToString() + ".data";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            Debug.LogError("Save file at the path: '" + path + "' was not found and therefore could not be deleted.");
        }
    }

    // For Options Data
    public static void SaveOptionsData(OptionsHandler optionsHandler)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/options.data";
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

        OptionsData optionsData = new OptionsData(optionsHandler);

        formatter.Serialize(stream, optionsData);
        stream.Close();
    }

    public static OptionsData LoadOptionsData()
    {
        string path = Application.persistentDataPath + "/options.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            OptionsData optionsData = formatter.Deserialize(stream) as OptionsData;
            stream.Close();

            return optionsData;
        }
        Debug.LogWarning("Options file at the path: '" + path + "' was not found. | Default value of 0.6f has been set.");
        return null;
    }

    // For Both Options and Level Save Data
    public static void DeleteAllSaveData()
    {
        string path = Application.persistentDataPath;
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
            Directory.CreateDirectory(path);
        }
        else
        {
            Debug.LogError("Save folder at the path: '" + path + "' was not found and therefore all of the save files could not be deleted.");
        }
    }
}
