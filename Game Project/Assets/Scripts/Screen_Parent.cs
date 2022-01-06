using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System;

//responsible for Parent user GUI
public class Screen_Parent : MonoBehaviour
{
    [HideInInspector] public static bool IsParent = false;
    private static bool firstLogin;
    private static string defaultPass = "0000";
    private static string savedPass;
    private static string path;
    public GameObject OptionsMenu;
    public GameObject EditPlayer;
    public GameObject ParentLogin;
    public InputField InputPass;
    public GameObject PasswordPrompt;
    public InputField InputNewPass;
    public GameObject ParentOptions;
    public GameObject PlayerStats;
    public GameObject ComboGuide;
    public GameObject PickSave;
    public Text combosText;
    public Dropdown DropF; //for the fontsize dropdown event
    public Button submitButton; //for the login button
    public Button Confirm; //for the password change
    public GameObject ErrorMessage;

    public InputField limit;
    public InputField bedTime;
    public TMP_Text isHintText;
    public Button isHint;

    public TMP_Text Total;
    public TMP_Text PlayTime;
    public TMP_Text BedTimeLimit;
    public TMP_Text Difficulty;
    public TMP_Text Combos;
    public TMP_Text AllowHints;
    string pathSettings;
    string SavePath;
    Data_PerSaveSlot SaveData;
    private bool hintBool;

    public void Awake()
    {
        ErrorMessage.SetActive(false);
        pathSettings = Application.persistentDataPath + "/saves/Settings.save"; //Global data
        DropF.value = PlayerPrefs.GetInt("ChangeFont",0);
        path = Application.persistentDataPath + "/config.parent";
        savedPass = IO_Files.ReadString(path);
        if (savedPass!=null)
        {
            firstLogin = false;
        }
        else
        {
            firstLogin = true;
            Debug.Log("First Login");
        }
        //DropF = GetComponent<Dropdown>(); //for font size
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || (Input.GetKey("enter")))
        {
            submitButton.onClick.Invoke(); //the login button
            Confirm.onClick.Invoke(); //the change password button
        }
    }
    public void TryLogin()  //try to login
    {
        Debug.Log(path);
        if (firstLogin)
        {
            if (InputPass.text == defaultPass)
            {
                Debug.Log("Log in succefull");
                ErrorMessage.SetActive(false);
                IsParent = true;
                ParentLogin.SetActive(false);
                PasswordPrompt.SetActive(true);
            }
            else
            {
                ErrorMessage.SetActive(true);
                Debug.Log("Incorrect Code");
            }
        }
        else    //not a first login
        {
            Debug.Log("Saved "+savedPass);
            Debug.Log("Input "+InputPass.text);
            if (InputPass.text == savedPass)
            {
                Debug.Log("Log in succefull");
                IsParent = true;
                ParentLogin.SetActive(false);
                ParentOptions.SetActive(true);
            }
            else
            {
                Debug.Log("Incorrect Code");
            }
        }
        ClearInputField();
    }
    public void ChangePass()  //save new password
    {
        savedPass = InputNewPass.text;
        Debug.Log(savedPass);
        IO_Files.WriteFile(path,savedPass);
        InputNewPass.text="";
        Debug.Log("Log in succefull");
        firstLogin = false;
        PasswordPrompt.SetActive(false);
        ParentOptions.SetActive(true);
    }
    public void PassBack()  //back button on new password page
    {
        if (firstLogin)
        {
            PasswordPrompt.SetActive(false);
            PasswordPrompt.transform.parent.gameObject.SetActive(false); //turn Parent off
            OptionsMenu.SetActive(true);
        }
        else
        {
            PasswordPrompt.SetActive(false);
            ParentOptions.SetActive(true);
        }
        ErrorMessage.SetActive(false); //reseting the error message
        ClearPassInputField(); //clear the pass input field

    }
    public void CombosScreen()
    {
        if (combosText.text == "")  //only fetch text if the object is empty
        {
            Card_Pool Pool = ScriptableObject.CreateInstance<Card_Pool>();        //open Card_Pool connection to use its functions
            string cards = Pool.GetAllCombos();
            combosText.text = cards.ToString();
        }
        ParentOptions.SetActive(false);
        ComboGuide.SetActive(true);
    }

    public void DisableParent()       //delete file, use carefully
    {
        File.Delete(path);
        ParentOptions.SetActive(false);
        ParentOptions.transform.parent.gameObject.SetActive(false); //turn Parent off
        OptionsMenu.SetActive(true);
        firstLogin = true;
    }
    
    public void SetFontSize()    //set font size (0 - default, 1 - big)
    {
        if (DropF.value == 1)
        {
            PlayerPrefs.SetInt("ChangeFont", 1);
        }
        
        if(DropF.value == 0)
        {
            PlayerPrefs.SetInt("ChangeFont", 0);
        }
    }
    public void ReadSave(string slot)    //load Save's stats to display
    {
        SavePath = Application.persistentDataPath + "/saves/" + slot + ".conf"; //adjust when saves exist

        SaveData = IO_Files.ReadDataSetting(pathSettings, SavePath).data_perSaveSlot;
        if (File.Exists(SavePath))
        {
            PlayerStats.SetActive(true);
            Total.text = "Total Game Time: " + FloatToTime(SaveData.TotalGameTime);
            if(SaveData.timeLimitSet)
                PlayTime.text = "Play Time Limit Set: " + FloatToTime(SaveData.timeLimit);
            else
                PlayTime.text = "Play Time Limit Set: NONE";

            if (SaveData.bedtimeSet)
                BedTimeLimit.text = "Bedtime Set: " + FloatToTime(SaveData.bedtime);
            else
                BedTimeLimit.text = "Bedtime Set: NONE";

            Difficulty.text = "Difficulty: " + SaveData.difficulty.ToString();
            Combos.text = "Cards Combined: " + SaveData.CardsCombined.ToString();

            if (SaveData.hintsOn)
                AllowHints.text = "Allowed Hints: ON";
            else
                AllowHints.text = "Allowed Hints: OFF";
            PickSave.SetActive(false);
        }
        else
        {
            Debug.Log("No save in the slot.");
        }
    }

    public void SetStats()    //enable&set/disable time limit per save
    {
        SaveData = IO_Files.ReadDataSetting(pathSettings, SavePath).data_perSaveSlot;

        if (limit.text == "")
        {
            SaveData.timeLimitSet = false; //update file
            SaveData.timeLimit = 0f;
        }
        else
        {
            SaveData.timeLimitSet = true; //update file
            SaveData.timeLimit = TimeToFloat(limit.text); //update file
        }

        if (bedTime.text == "")
        {
            SaveData.bedtimeSet = false;
            SaveData.bedtime = 0f;
        }
        else
        {
            SaveData.bedtimeSet = true;
            SaveData.bedtime = TimeToFloat(bedTime.text);
        }
        SaveData.hintsOn = hintBool;

        IO_Files.WriteDataPerSave(SavePath, SaveData);
    }

    public void loadButtonText()
    {
        if(SaveData.hintsOn)
            isHintText.text = "Allow hints: ON";
        else
            isHintText.text = "Allow hints: OFF";
    }

    public void SetHints()       //enable/disable hints per save
    {
        if (SaveData.hintsOn)
        {
            isHintText.text = "Allow hints: OFF";
            hintBool = false;
        }
        else
        {
            isHintText.text = "Allow hints: ON";
            hintBool = true;
        }
    }

    public float TimeToFloat(string time)   //converts time in hh:mm:ss or hh:mm format into float
    {
        string[] times = time.Split(':');
        float hours = float.Parse(times[0]);
        float minutes = float.Parse(times[1]);
        float seconds = 0;
        if (times.Length > 2)
        {
            seconds = float.Parse(times[2]);
        }
        return (hours * 60 + minutes) * 60 + seconds;
    }
    public float TimeToFloat(int hours, int minutes, int seconds)   //converts time in hh:mm:ss format into float
    {
        return (hours * 60 + minutes) * 60 + seconds;
    }
    public string FloatToTime(float time)   //converts time floats into a string
    {
        double hours = Math.Floor(time / (60 * 60));
        double minutes = Math.Floor((time - hours * 60 * 60) / 60);
        double seconds = time - hours * 60 * 60 - minutes * 60;
        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    public void ClearInputField()
    {
        //to clear the input field
        InputPass.Select();
        InputPass.text = "";
    }

    public void ClearPassInputField()
    {
        //to clear the change password input field
        InputNewPass.Select();
        InputNewPass.text = "";
    }
}
