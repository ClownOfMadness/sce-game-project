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
    private string pathGameData;
    private string pathSettings;
    private string pathPerSave;
    private string pathTemporary;
    private GameObject delete; //Delete button
    private List<TMP_Text> slots; //List of all the available slots's text values

    private void Awake()
    {
        
    }
    void Start()
    {
        pathGameData = Application.persistentDataPath + "/saves/" + currSlot + ".save"; //Game data
        pathSettings = Application.persistentDataPath + "/saves/Settings.save"; //Global data
        pathPerSave = Application.persistentDataPath + "/saves/" + currSlot + ".conf"; //Slot configuration
        pathTemporary = Application.persistentDataPath + "/temp/config.temp"; //Temporary data
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
        //Saving the game data from Map_Display.
        IO_Files.WriteData(pathGameData, map_display.Save_Data());
        //Saving the game data from Game_master.
        //IO_Files.WriteDataPerSave(pathPerSave, GameMaster.DAtamashehu);

        //Saving the global settings.
        //IO_Files.WriteDataSetting(pathSettings, GameMaster.DAtamashehu);
        SavedSlots(); 
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

        }
    }

    public void SavedSlots()
    {
        PlayerPrefs.SetString(currSlot, slotName);
        PlayerPrefs.Save();

        LoadedSlots();
    }

    //Delete the current save slot data.
    public void Delete()
    {
        IO_Files.DeleteData(pathGameData);
        //IO_Files.DeleteData(pathPerSave);
        DeleteSlot();
    }

    //The multi scene play button.
    public void Play()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            slotName = saveName.text;

            //Set the path of the temporary data.
            
            //Delete the old temporary data.
            //if (File.Exists(path))
                //IO_Files.DeleteData(path);

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

       

        data_player = (Data_Player)IO_Files.ReadData(pathGameData);
        return data_player;
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
