using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveStatistics(StatisticsManager statsManager)
    {
        string path = Application.persistentDataPath + "/statistics.def";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerStatistics data = new PlayerStatistics(statsManager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerStatistics LoadStatistics()
    {
        string path = Application.persistentDataPath + "/statistics.def";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerStatistics data = formatter.Deserialize(stream) as PlayerStatistics;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
