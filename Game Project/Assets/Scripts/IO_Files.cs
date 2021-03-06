using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


//All the loading and saving functions
public static class IO_Files
{
    //Load string from file.
    public static string ReadString(string path)  
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

    //Load Data_Player from file.
    public static Data_Player ReadData(string path)  
    {
        Data_Player data = null;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = GetBinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            data = formatter.Deserialize(stream) as Data_Player;
            stream.Close();     //opened files must be closed when done with
        }
        return data;
    }
    //Reading setting data from file.
    public static Save_Settings ReadDataSetting(string path_settings, string path_perSave)
    {
        Save_Settings data = null;
        Data_PerSaveSlot data_perSaveSlot = null;

        if (File.Exists(path_settings))
        {
            BinaryFormatter formatter = GetBinaryFormatter();

            if (File.Exists(path_perSave))
            {
                FileStream streamCofig = new FileStream(path_perSave, FileMode.Open);
                data_perSaveSlot = formatter.Deserialize(streamCofig) as Data_PerSaveSlot;
                streamCofig.Close();
            }
            else data_perSaveSlot = new Data_PerSaveSlot();

            FileStream streamSettings = new FileStream(path_settings, FileMode.Open);
            data = formatter.Deserialize(streamSettings) as Save_Settings;
            streamSettings.Close();
            data.data_perSaveSlot = data_perSaveSlot;
        }
        return data;
    }

    //Save string to file.
    public static void WriteFile(string path, string data)  
    {
        Debug.Log("Attempting to create");
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        Debug.Log("File created");
        stream.Close();         //opened files must be closed when done with
    }

    //Save the player's data to file.
    public static void WriteData(string path, Data_Player data)  
    {
        if (File.Exists(path))
            DeleteData(path);
        BinaryFormatter formatter = GetBinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        //Debug.Log("File created at: " + path);
        stream.Close();         //opened files must be closed when done with
    }
    //Save the player's configuration data to file.
    public static void WriteDataPerSave(string path, Data_PerSaveSlot data)  
    {
        if (File.Exists(path))
            DeleteData(path);
        BinaryFormatter formatter = GetBinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        //Debug.Log("File created at: " + path);
        stream.Close();         //opened files must be closed when done with
    }
    //Save the global data to file.
    public static void WriteDataSetting(string path, Save_Settings data)
    {
        if (File.Exists(path))
            DeleteData(path);
        BinaryFormatter formatter = GetBinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        //Debug.Log("File created at: " + path);
        stream.Close();         //opened files must be closed when done with
    }

    //Dalete the file.
    public static bool DeleteData(string path) 
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            //Debug.Log("File deleted: " + path);
            return true;
        }
        else
            Debug.LogWarning("File not found.");
        return false;
    }

    //Create binary formatter with serialization surrogates.
    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        SurrogateSelector selector = new SurrogateSelector();

        Save_SurrogateVec3 surrogateVec3 = new Save_SurrogateVec3();
        Save_SurrogateQuat surrogateQuat = new Save_SurrogateQuat();
        Save_SurrogateColor surrogateColor = new Save_SurrogateColor();

        //Replace type with serialization surrogate.
        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), surrogateVec3); //Vector3
        selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), surrogateQuat); //Quaterion
        selector.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All), surrogateColor); //Color

        formatter.SurrogateSelector = selector;
        return formatter;
    }
}
