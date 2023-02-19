using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static object LoadData(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".def";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            object data = formatter.Deserialize(stream);

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveData(object dataScript, string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".def";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        object data = dataScript;

        formatter.Serialize(stream, data);
        stream.Close();
    }
}
