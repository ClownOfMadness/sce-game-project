using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

//responsible for Parent user GUI
public class Screen_Parent : MonoBehaviour
{
    [HideInInspector] public static bool IsParent = false;
    private static bool firstLogin;
    private static string defaultPass = "0000";
    private static string savedPass;
    private static string path;
    public GameObject OptionsMenu;
    public GameObject ParentLogin;
    public InputField InputPass;
    public GameObject PasswordPrompt;
    public InputField InputNewPass;
    public GameObject ParentOptions;
    public GameObject PlayerStats;
    public GameObject ComboGuide;
    public Text combosText;
    public Dropdown Drop;

    public void Awake()
    {
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
        Drop = GetComponent<Dropdown>(); //for font size
    }
    public void TryLogin()  //try to login
    {
        
        Debug.Log(path);
        if (firstLogin)
        {
            if (InputPass.text == defaultPass)
            {
                Debug.Log("Log in succefull");
                //use an ingame message later on
                IsParent = true;
                ParentLogin.SetActive(false);
                PasswordPrompt.SetActive(true);
            }
            else
            {
                Debug.Log("Incorrect Code");
                //use an ingame message later on
            }
        }
        else    //not a first login
        {
            Debug.Log("Saved "+savedPass);
            Debug.Log("Input "+InputPass.text);
            if (InputPass.text == savedPass)
            {
                Debug.Log("Log in succefull");
                //use an ingame message later on
                IsParent = true;
                ParentLogin.SetActive(false);
                ParentOptions.SetActive(true);
            }
            else
            {
                Debug.Log("Incorrect Code");
                //use an ingame message later on
            }
        }
        InputPass.text = "";
    }
    public void ChangePass()  //save new password
    {
        savedPass = InputNewPass.text;
        Debug.Log(savedPass);
        IO_Files.WriteFile(path,savedPass);
        InputNewPass.text="";
        Debug.Log("Log in succefull");
        //use an ingame message later on
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
    public void SetFontSize()    //enable&set/disable time limit per save
    {
        if (PlayerPrefs.GetInt("ChangeFont") == 0)
        {
            PlayerPrefs.SetInt("ChangeFont", 1);
        }
        else
        {
            PlayerPrefs.SetInt("ChangeFont", 0);
        }
        //Debug.Log("Screen_Parent.SetFontSize: this function currently does nothing.");
    }
    public void ReadSave(int index)    //load Save's stats to display
    {
        string SavePath = Application.persistentDataPath + "/config" + index + ".player"; //adjust when saves exist
        Data_Player SaveData = IO_Files.ReadData(SavePath);
        if (SaveData != null)
        {
            //fill objects in Player stats with fields from save file
        }
        else
        {
            //loading save failed
        }
    }
    public void SetTimeLimit()    //enable&set/disable time limit per save
    {
        Debug.Log("Screen_Parent.SetTimeLimit: this function currently does nothing.");
        //if InputField="";
            //timeLimitSet=false; & update file
        //else
            //timeLimitSet=true; & update file
            //timeLimit=convert InputField string to float & update file
    }

    public void SetBedtime()    //enable&set/disable bedtime per save
    {
        Debug.Log("Screen_Parent.SetBedtime: this function currently does nothing.");
        //if InputField="";
            //bedtimeSet=false; & update file
        //else
            //bedtimeSet=true; & update file
            //bedtime=convert InputField string to float & update file
    }

    public void SetDificulty()    //set difficulty per save
    {
        Debug.Log("Screen_Parent.SetDifficulty: this function currently does nothing.");
        //difficulty=dropdown; & update file
    }
    public void SetGameSpeed()    //set game speed per save
    {
        Debug.Log("Screen_Parent.SetGameSpeed: this function currently does nothing.");
        //gameSpeed=dropdown; & update file
    }

    public void SetHints()       //enable/disable hints per save
    {
        Debug.Log("Screen_Parent.SetHints: this function currently does nothing.");
        //if HintsOn==true in save file
            //HintsOn=false; & update file
        //else
            //HintsOn=true; & update file
    }

    public void SetEnemies()       //enable/disable enemies per save
    {
        Debug.Log("Screen_Parent.SetEnemies: this function currently does nothing.");
        //if EnemiesOff==true in save file
            //EnemiesOff=false; & update file
        //else
            //EnemiesOff=true; & update file
    }
}
