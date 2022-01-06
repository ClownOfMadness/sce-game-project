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
    private Game_Master GameMaster;
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
    private string pathGameData;
    private string pathSettings;
    private string pathPerSave;
    private string pathTemporary;
    private GameObject delete; //Delete button
    public List<TMP_Text> slots; //List of all the available slots's text values

    private void Awake()
    {
        //Create saves directory if not created yet.
        if (!Directory.Exists(Application.persistentDataPath + "/saves"))
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        //Create temp directory if not created yet.
        if (!Directory.Exists(Application.persistentDataPath + "/temp"))
            Directory.CreateDirectory(Application.persistentDataPath + "/temp");

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            slots = new List<TMP_Text>();
            for (int i = 0; i < 24; i++)
                slots.Add(SaveSlots.transform.GetChild(i).GetComponentInChildren<TMP_Text>());
        }

        pathGameData = Application.persistentDataPath + "/saves/" + currSlot + ".save"; //Game data
        pathSettings = Application.persistentDataPath + "/saves/Settings.save"; //Global data
        pathPerSave = Application.persistentDataPath + "/saves/" + currSlot + ".conf"; //Slot configuration
        pathTemporary = Application.persistentDataPath + "/temp/config.temp"; //Temporary data

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            if (menu_main == null) menu_main = FindObjectOfType<Menu_Main>();
            UpdateMenu();
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            if (GameMaster == null) GameMaster = FindObjectOfType<Game_Master>();
            if (menu_pause == null) menu_pause = FindObjectOfType<Menu_Pause>();
            if (map_display == null) map_display = FindObjectOfType<Map_Display>();

            if (File.Exists(pathPerSave))
                SettingsLoaded(pathPerSave);
            else if (File.Exists(pathTemporary))
                SettingsLoaded(pathTemporary);
            else Debug.LogError("Configuration file not found.");
        }
    }

    void Start()
    {
        delete = startGame.transform.GetChild(2).gameObject;
        Back.GetComponent<Button>().onClick.AddListener(BackSelect);
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

    public void updateSlotNum()
    {
        if (PlayerPrefs.GetInt("premium", 0) == 1)
            slotsNum = 24;
        else slotsNum = 4;

        LoadedSlots();
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
        //Saving the game data from Map_Display.
        IO_Files.WriteData(pathGameData, map_display.Save_Data());
        //Saving the game data from Game_master.
        SavePerSlot(pathPerSave);
        //Saving the global settings.
        SaveSettings();
        SavedSlots(); 
    }

    public void SaveSettings()
    {
        Save_Settings save_settings = new Save_Settings();

        if (SceneManager.GetActiveScene().name == "Game")
        {
            save_settings.isVeteran = GameMaster.isVeteran;
            save_settings.isBuilder = GameMaster.isBuilder;
            save_settings.isCrafty = GameMaster.isCrafty;
            save_settings.windowLook = GameMaster.windowLook;
        }
        else if (SceneManager.GetActiveScene().name == "Menu")
        {
            save_settings.isVeteran = menu_main.isVeteran;
            save_settings.isBuilder = menu_main.isBuilder;
            save_settings.isCrafty = menu_main.isCrafty;
            save_settings.windowLook = menu_main.windowLook;
        }

        IO_Files.WriteDataSetting(pathSettings, save_settings);
    }

    //Save the player's configuration on the current save (or as temporary file).
    public void SavePerSlot(string path)
    {
        Data_PerSaveSlot data_perSlot = new Data_PerSaveSlot();

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            if (!File.Exists(pathPerSave)) //Saved automaticly when loading the game
            {
                data_perSlot.isFirstGame = true;
            }
            else //Will be set on the panels in the menu
            {
                data_perSlot.isFirstGame = false;
                data_perSlot.TotalGameTime = menu_main.TotalGameTime;
                data_perSlot.CardsCombined = menu_main.CardsCombined;
                data_perSlot.CardsDiscovered = menu_main.CardsDiscovered;
                data_perSlot.gameDays = menu_main.gameDays;
                data_perSlot.buildingsCount = menu_main.buildingsCount;
            }
            data_perSlot.charLook = menu_main.charLook;
            data_perSlot.bedtimeSet = menu_main.bedtimeSet;
            data_perSlot.bedtime = menu_main.bedtime;
            data_perSlot.timeLimitSet = menu_main.timeLimitSet;
            data_perSlot.timeLimit = menu_main.timeLimit;
            data_perSlot.timeLeft = menu_main.timeLeft;
            data_perSlot.fontSize = menu_main.fontSize;
            data_perSlot.hintsOn = menu_main.hintsOn;
            data_perSlot.gameSpeed = menu_main.gameSpeed;
            data_perSlot.enemiesOff = menu_main.enemiesOff;
            data_perSlot.difficulty = menu_main.difficulty;
            data_perSlot.fogOff = menu_main.fogOff;
        }
        else if(SceneManager.GetActiveScene().name == "Game")
        {
            data_perSlot.isFirstGame = false;
            data_perSlot.TotalGameTime = GameMaster.totalGameTime;
            data_perSlot.CardsCombined = GameMaster.Cards.CardsCombined;
            data_perSlot.CardsDiscovered = GameMaster.Cards.CardsDiscovered;
            data_perSlot.gameDays = GameMaster.gameDays;
            data_perSlot.buildingsCount = GameMaster.buildingsCount;
            data_perSlot.charLook = GameMaster.charLook;
            data_perSlot.bedtimeSet = GameMaster.bedtimeSet;
            data_perSlot.bedtime = GameMaster.bedtime;
            data_perSlot.timeLimitSet = GameMaster.timeLimitSet;
            data_perSlot.timeLimit = GameMaster.timeLimit;
            data_perSlot.timeLeft = GameMaster.timeLeft;
            data_perSlot.fontSize = (int)GameMaster.fontSize;
            data_perSlot.hintsOn = GameMaster.hintsOn;
            data_perSlot.gameSpeed = (int)GameMaster.gameSpeed;
            data_perSlot.enemiesOff = GameMaster.enemiesOff;
            data_perSlot.difficulty = GameMaster.difficulty;
            data_perSlot.fogOff = GameMaster.fogOff;
        }
        IO_Files.WriteDataPerSave(path, data_perSlot);
    }

    //Save the current name of the current slot.
    public void SavedSlots()
    {
        PlayerPrefs.SetString(currSlot, slotName);
        PlayerPrefs.Save();

        //LoadedSlots();
    }

    //Delete the current save slot data.
    public void Delete()
    {
        pathGameData = Application.persistentDataPath + "/saves/" + currSlot + ".save"; //Game data
        pathPerSave = Application.persistentDataPath + "/saves/" + currSlot + ".conf"; //Slot configuration

        if (IO_Files.DeleteData(pathGameData))
            if(IO_Files.DeleteData(pathPerSave))
                DeleteSlot();
    }

    public void LoadCustum()
    {
        string path = Application.persistentDataPath + "/config.parent";

        if (File.Exists(path) && !File.Exists(pathPerSave))
        {
            menu_main.customParent.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("premium", 0) == 1 && !File.Exists(pathPerSave))
        {
            menu_main.customPremium.SetActive(true);
        }
        else
            Play();
    }

    //The multi scene play button.
    public void Play()
    {
        if (menu_main.customPremium.activeSelf)
            menu_main.customPremium.SetActive(false);
        if (menu_main.customParent.activeSelf)
            menu_main.customParent.SetActive(false);

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            slotName = saveName.text;

            //Delete the old temporary data.
            if (File.Exists(pathTemporary))
                IO_Files.DeleteData(pathTemporary);

            SavePerSlot(pathTemporary);
            SaveSettings();

            menu_main.NewGame();
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            SceneManager.LoadScene("Game");
        }
    }

    public Data_Player SceneLoaded()
    {
        Data_Player data_player = (Data_Player)IO_Files.ReadData(pathGameData);

        pathPerSave = Application.persistentDataPath + "/saves/" + currSlot + ".conf"; //Slot configuration
        pathTemporary = Application.persistentDataPath + "/temp/config.temp"; //Temporary data

        return data_player;
    }

    public void SettingsLoaded(string path)
    {
        Save_Settings save_settings = (Save_Settings)IO_Files.ReadDataSetting(pathSettings, path);

        GameMaster.charLook = save_settings.data_perSaveSlot.charLook;
        GameMaster.bedtimeSet = save_settings.data_perSaveSlot.bedtimeSet;
        GameMaster.bedtime = save_settings.data_perSaveSlot.bedtime;
        GameMaster.timeLimitSet = save_settings.data_perSaveSlot.timeLimitSet;
        GameMaster.timeLimit = save_settings.data_perSaveSlot.timeLimit;
        GameMaster.timeLeft = save_settings.data_perSaveSlot.timeLeft;
        GameMaster.fontSize = (Game_Master.fontList)save_settings.data_perSaveSlot.fontSize;
        GameMaster.hintsOn = save_settings.data_perSaveSlot.hintsOn;
        GameMaster.gameSpeed = (Game_Master.speedList)save_settings.data_perSaveSlot.gameSpeed;
        GameMaster.enemiesOff = save_settings.data_perSaveSlot.enemiesOff;
        GameMaster.difficulty = save_settings.data_perSaveSlot.difficulty;
        GameMaster.fogOff = save_settings.data_perSaveSlot.fogOff;
        GameMaster.isFirst = save_settings.data_perSaveSlot.isFirstGame;
        GameMaster.totalGameTime = save_settings.data_perSaveSlot.TotalGameTime;
        GameMaster.CardsCombined = save_settings.data_perSaveSlot.CardsCombined;
        GameMaster.CardsDiscovered = save_settings.data_perSaveSlot.CardsDiscovered;
        GameMaster.gameDays = save_settings.data_perSaveSlot.gameDays;
        GameMaster.buildingsCount = save_settings.data_perSaveSlot.buildingsCount;

        GameMaster.isVeteran = save_settings.isVeteran;
        GameMaster.isBuilder = save_settings.isBuilder;
        GameMaster.isCrafty = save_settings.isCrafty;
        GameMaster.windowLook = save_settings.windowLook;
    }

    public void LoadedSlots()
    {
        for (int i = 0; i < 24; i++)
        {
            if (i < slotsNum)
            {
                slots[i].transform.parent.gameObject.SetActive(true);
                slots[i].text = PlayerPrefs.GetString(string.Format("Slot {0}", i + 1), emptySlot);
            }
            else
                slots[i].transform.parent.gameObject.SetActive(false);
        }
    }

    //Delete the current slot.
    public void DeleteSlot()
    {
        PlayerPrefs.SetString(currSlot, emptySlot);
        PlayerPrefs.Save();
        LoadedSlots();
        saveName.text = "";
        delete.SetActive(false);
    }

    //Update the data in the menu.
    public void UpdateMenu()
    {
        Save_Settings current;

        if (File.Exists(pathSettings))
            current = IO_Files.ReadDataSetting(pathSettings, null);
        else current = new Save_Settings();

        menu_main.isVeteran = current.isVeteran;
        menu_main.isBuilder = current.isBuilder;
        menu_main.isCrafty = current.isCrafty;
        menu_main.windowLook = current.windowLook;
    }
}
