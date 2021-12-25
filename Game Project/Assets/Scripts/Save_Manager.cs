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
    public GameObject SaveSlots;
    public GameObject startGame;
    public GameObject Back;
    public static string currSlot;

    private Menu_Main menu;
    private int slotsNum = 4;
    private string emptySlot = "Empty slot";
    private string selected;
    private GameObject delete;
    private List<TMP_Text> slots;


    void Start()
    {
        menu = FindObjectOfType<Menu_Main>();
        delete = startGame.transform.GetChild(2).gameObject;

        Back.GetComponent<Button>().onClick.AddListener(BackSelect);

        slots = new List<TMP_Text>();
        for (int i = 0; i < slotsNum; i++)
            slots.Add(SaveSlots.transform.GetChild(i).GetComponentInChildren<TMP_Text>());

        //Create saves directory if not created yet.
        //if (!Directory.Exists(Application.persistentDataPath + "/saves"))
        //    Directory.CreateDirectory(Application.persistentDataPath + "/saves");

        //Set the path as the game slot.
        string path = Application.persistentDataPath + "/saves/" + saveName.text + ".save";
        LoadedSlots();
    }

    public void IsNewSlot()
    {
        selected = EventSystem.current.currentSelectedGameObject.transform.parent.name;

        if (EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text == emptySlot)
        {
            SaveSlots.SetActive(false);
            startGame.SetActive(true);
        }
       else
        {
            SaveSlots.SetActive(false);
            startGame.SetActive(true);
            saveName.text = PlayerPrefs.GetString(selected, "");
            delete.SetActive(true);
        }
    }

    void BackSelect()
    {
        if (startGame.activeSelf)
        {
            SaveSlots.SetActive(true);
            startGame.SetActive(false);
            saveName.text = "";
            delete.SetActive(false);
        }
        else
        {
            menu.GameMenu.SetActive(false);
            menu.MenuMain.SetActive(true);
        }
    }

    public void DeleteSlot()
    {
        Debug.Log(selected);
        PlayerPrefs.SetString(selected, emptySlot);
        PlayerPrefs.Save();
        LoadedSlots();
        saveName.text = "";
        delete.SetActive(false);
    }

    public void SavedSlots()
    {
        PlayerPrefs.SetString(selected, saveName.text);

        currSlot = selected;
        PlayerPrefs.Save();
        LoadedSlots();
        menu.NewGame();
    }

    public void LoadedSlots()
    {
        slots[0].text = PlayerPrefs.GetString("Slot 1", emptySlot);
        slots[1].text = PlayerPrefs.GetString("Slot 2", emptySlot);
        slots[2].text = PlayerPrefs.GetString("Slot 3", emptySlot);
        slots[3].text = PlayerPrefs.GetString("Slot 4", emptySlot);

    }
}
