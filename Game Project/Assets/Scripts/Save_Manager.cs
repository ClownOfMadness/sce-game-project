using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Save_Manager : MonoBehaviour
{
    public TMP_InputField saveName;
    public static GameObject GameSlots;
    private int slotsNum = 4;
   

    Button[] slots;

    //private string[] slotsArr;

    public void StartGame()
    {
        for(int i = 0; i < slotsNum; i++)
            slots[i] = GameSlots.transform.GetChild(i).GetComponent<Button>();

        //Create saves directory if not created yet.
        //if (!Directory.Exists(Application.persistentDataPath + "/saves"))
        //    Directory.CreateDirectory(Application.persistentDataPath + "/saves");

        //Set the path as the game slot.
        string path = Application.persistentDataPath + "/saves/" + saveName.text + ".save";
    }

    public void IsNewSlot()
    {
        Button B = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        Debug.Log(B.GetComponentInChildren<TMPro.TextMeshPro>());
    }

    //public void SaveSlots()
    //{
    //    PlayerPrefs.SetString("Slot1", saveName.text);
    //    PlayerPrefs.SetString("Slot2", saveName.text);
    //    PlayerPrefs.SetString("Slot3", saveName.text);
    //    PlayerPrefs.SetString("Slot4", saveName.text);
    //    PlayerPrefs.Save();
    //}

    public void LoadSlots()
    {
        
    }
}
