using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

//all the loading and saving functions
public static class IO_Files
{
    public static string ReadString(string path)  //load string from file
    {
        string data = null;
        if (File.Exists(path))
        {
            //BinaryFormatter formatter = new BinaryFormatter();
            BinaryFormatter formatter = GetBinaryFormatter();
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
            //BinaryFormatter formatter = new BinaryFormatter();
            BinaryFormatter formatter = GetBinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            data = formatter.Deserialize(stream) as Data_Player;
            stream.Close();     //opened files must be closed when done with
        }
        return data;
    }
    public static void WriteFile(string path, string data)  //save string to file
    {
        Debug.Log("Attempting to create");
        //BinaryFormatter formatter = new BinaryFormatter();
        BinaryFormatter formatter = GetBinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        Debug.Log("File created");
        stream.Close();         //opened files must be closed when done with
    }


    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        SurrogateSelector selector = new SurrogateSelector();
        Save_SurrogateVec3 surrogateVec3 = new Save_SurrogateVec3();
        Save_SurrogateQuat surrogateQuat = new Save_SurrogateQuat();

        //Replace type with serializable surrogate.
        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), surrogateVec3);
        selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), surrogateQuat);

        formatter.SurrogateSelector = selector;
        return formatter;
    }
}
