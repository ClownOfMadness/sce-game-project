using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class Save_Manager : MonoBehaviour
{
    public GameObject GameMaster;
    public TMP_InputField saveName; //Name Input field
    public GameObject SaveSlots; //Panel SaveSlots
    public GameObject startGame; //Panel StartGame 
    public GameObject Back; //Button Back
    public static string currSlot; //The slot in which the data will be saved
    public static string slotName;

    //The classes that operate the UI.
    private Menu_Main menu_main = null; //Main menu
    private Menu_Pause menu_pause = null; //Pause menu
    //The class that gathers data for save(right now).
    private Map_Display map_display = null; //Display

    //Configuration values.
    private int slotsNum = 4; //Number of slots
    private string emptySlot = "Empty slot"; //Empty slot constant
    private string path; //The saving path
    private GameObject delete; //Delete button
    private List<TMP_Text> slots; //List of all the available slots's text values


    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            if (menu_main == null) menu_main = FindObjectOfType<Menu_Main>();
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            if (menu_pause == null) menu_pause = FindObjectOfType<Menu_Pause>();
            if (map_display == null) map_display = FindObjectOfType<Map_Display>();
        }

        delete = startGame.transform.GetChild(2).gameObject;

        Back.GetComponent<Button>().onClick.AddListener(BackSelect);

        slots = new List<TMP_Text>();
        for (int i = 0; i < slotsNum; i++)
            slots.Add(SaveSlots.transform.GetChild(i).GetComponentInChildren<TMP_Text>());

        //Create saves directory if not created yet.
        if (!Directory.Exists(Application.persistentDataPath + "/saves"))
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        //Create temp directory if not created yet.
        if (!Directory.Exists(Application.persistentDataPath + "/temp"))
            Directory.CreateDirectory(Application.persistentDataPath + "/temp");
        LoadedSlots();
    }

    //Open the save slots.
    public void ActivateSlot()
    {
        if (SceneManager.GetActiveScene().name == "Menu") 
        {
            currSlot = EventSystem.current.currentSelectedGameObject.transform.parent.name;

            if (EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text == emptySlot)
            {
                SaveSlots.SetActive(false);
                startGame.SetActive(true);
            }
            else
            {
                SaveSlots.SetActive(false);
                startGame.SetActive(true);
                saveName.text = PlayerPrefs.GetString(currSlot, "");
                delete.SetActive(true);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        { 
            menu_pause.gamePlayUI.SetActive(true);
            menu_pause.pauseMenuUI.SetActive(false);
        }
    }

    //The multi scene back button function.
    void BackSelect()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
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
                menu_main.GameMenu.SetActive(false);
                menu_main.MenuMain.SetActive(true);
            }
        }
        else if(SceneManager.GetActiveScene().name == "Game")
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
                menu_pause.gamePlayUI.SetActive(false);
                menu_pause.pauseMenuUI.SetActive(true);
            }
        }
    }

    //Save the current slot with the save button on the pause menu.
    public void Save()
    {
        //Set the path of the save slot.
        path = Application.persistentDataPath + "/saves/" + currSlot + ".save";

        IO_Files.WriteData(path, map_display.Save_Data());

        string path2 = Application.persistentDataPath + "/saves/Settings.save";
        Save_Settings current = new Save_Settings();
        IO_Files.WriteDataSetting(path2, current);
        SavedSlots(); 
    }

    //Delete the current save slot data.
    public void Delete()
    {
        //Set the path of the save slot.
        path = Application.persistentDataPath + "/saves/" + currSlot + ".save";

        IO_Files.DeleteData(path);
        DeleteSlot();
    }

    //The multi scene play button.
    public void Play()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            slotName = saveName.text;

            //Set the path of the temporary data.
            path = Application.persistentDataPath + "/temp/config.temp";
            //Delete the old temporary data.
            if (File.Exists(path))
                IO_Files.DeleteData(path);

            menu_main.NewGame();
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            SceneManager.LoadScene("Game");
        }
    }

    public Data_Player SceneLoaded()
    {
        Data_Player data_player = new Data_Player();

        //Set the path of the save slot.
        path = Application.persistentDataPath + "/saves/" + currSlot + ".save";

        data_player = (Data_Player)IO_Files.ReadData(path);
        return data_player;
    }

    //Save the player's configuration on the current save (or as temporary file).
    public void SavePlayerConfig(string path)
    {
        Data_PlayerConfig data_player = new Data_PlayerConfig();

        if(SceneManager.GetActiveScene().name == "Menu") //Create temporary file
        {

        }
    }

    public void SavedSlots()
    {
        PlayerPrefs.SetString(currSlot, slotName);
        PlayerPrefs.Save();

        LoadedSlots();
    }

    public void LoadedSlots()
    {
        for (int i = 0; i < slotsNum; i++)
            slots[i].text = PlayerPrefs.GetString(string.Format("Slot {0}", i + 1), emptySlot);
    }

    public void DeleteSlot()
    {
        PlayerPrefs.SetString(currSlot, emptySlot);
        PlayerPrefs.Save();
        LoadedSlots();
        saveName.text = "";
        delete.SetActive(false);
    }

    public void UpdateSettings(Game_Master data)
    {
        string path = Application.persistentDataPath + "/saves/Settings.save";
        //Save_Settings current = IO_Files.ReadDataSetting(path);

        //premiumUser = data.premiumUser;
        //parentPassword = data.parentPassword;

        //isVeteran = data.isVeteran;
        //if (current.maxGameDays < data.gameDays)
        //    current.maxGameDays = data.gameDays;
        //else maxGameDays = current.maxGameDays;

        //isBuilder = data.isBuilder;
        //if (current.maxBuildings < data.buildings)
        //    current.maxBuildings = data.buildings;
        //else maxBuildings = current.maxBuildings;

        //isCrafty = data.isExplorer;
        //if (current.maxBuildings. < data.buildings)
        //    current.maxCardsFound = data.cardsFound;
        //else maxCardsFound = current.maxCardsFound;

        //IO_Files.WriteDataSetting(path, current);
    }
}
