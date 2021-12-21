using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//all the loading and saving functions
public static class IO_Files
{
    public static string ReadString(string path)  //load string from file
    {
        string data = null;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            data = formatter.Deserialize(stream) as string;
            Debug.Log(data);
            stream.Close();     //opened files must be closed when done with
        }
        return data;
    }
    public static Data_Player ReadData(string path)  //load Data_Player from file
    {
        Data_Player data = null;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            data = formatter.Deserialize(stream) as Data_Player;
            stream.Close();     //opened files must be closed when done with
        }
        return data;
    }
    public static void WriteFile(string path, string data)  //save string to file
    {
        Debug.Log("Attempting to create");
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        Debug.Log("File created");
        stream.Close();         //opened files must be closed when done with
    }

    //public static bool Save(string saveSlot, object data)
    //{
    //    BinaryFormatter formatter = GetBinaryFormatter(); //Create formatter

    //    //Create saves directory if not created yet.
    //    if (!Directory.Exists(Application.persistentDataPath + "/saves"))
    //        Directory.CreateDirectory(Application.persistentDataPath + "/saves");

    //    //Set the path as the game slot.
    //    string path = Application.persistentDataPath + "/saves/" + saveSlot + ".save";
    //    FileStream file = File.Create(path);
    //    formatter.Serialize(file, data);
    //    file.Close();

    //    return true;
    //}

    //public static object Load(string path)
    //{
    //    if (!File.Exists(path))
    //        return null;

    //    BinaryFormatter formatter = GetBinaryFormatter();

    //    FileStream file = File.Open(path, FileMode.Open);

    //    try
    //    {
    //        object save = formatter.Deserialize(file);
    //        file.Close();
    //        return save;
    //    }
    //    catch
    //    {
    //        Debug.LogErrorFormat("Failed to load file at {0}", path);
    //        file.Close();
    //        return null;
    //    }
    //}

    //public static BinaryFormatter GetBinaryFormatter()
    //{
    //    BinaryFormatter formatter = new BinaryFormatter();

    //    return formatter;
    //}


}
