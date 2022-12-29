using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;

public static class SaveSystem
{
    public static void SaveLevelData(LevelHandler levelHandler)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/level" + levelHandler.buildIndex.ToString() + ".data";
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

        LevelData levelData = new LevelData(levelHandler);

        formatter.Serialize(stream, levelData);
        stream.Close();
    }

    public static LevelData LoadLevelData(LevelHandler levelHandler)
    {
        string path = Application.persistentDataPath + "/level" + levelHandler.buildIndex.ToString() + ".data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelData levelData = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return levelData;
        }
        Debug.LogError("Save file at the path: '" + path + "' was not found.");
        return null;
    }

    public static LevelData LoadLevelDataThroughBuildIndex(int index)
    {
        string path = Application.persistentDataPath + "/level" + index + ".data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelData levelData = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return levelData;
        }
        Debug.LogError("Save file at the path: '" + path + "' was not found.");
        return null;
    }

    public static TimeSpan LoadLevelScore(LevelHandler levelHandler)
    {
        double seconds = LoadLevelData(levelHandler).seconds;
        return TimeSpan.FromSeconds(seconds);
    }

    // For Debugging
    public static void DeleteSaveFile(LevelHandler levelHandler)
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
}
