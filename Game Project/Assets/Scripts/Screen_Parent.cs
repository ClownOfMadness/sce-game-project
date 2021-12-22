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
    public void ReadSave(int index)    //load Save's stats
    {
        string SavePath = Application.persistentDataPath + "/config" + index + ".player";
        Data_Player SaveData = IO_Files.ReadData(SavePath);
        if (SaveData!=null)
        {

        }
        else
        {
            
        }
    }
    public void DisableParent()       //delete file, use carefully
    {
        File.Delete(path);
        ParentOptions.SetActive(false);
        ParentOptions.transform.parent.gameObject.SetActive(false); //turn Parent off
        OptionsMenu.SetActive(true);
        firstLogin = true;
    }
    public void CombosScreen()
    {
        Card_Pool Pool = ScriptableObject.CreateInstance<Card_Pool>();        //open Card_Pool connection to use its functions
        List<string> cards = Pool.GetAllCombos();
        for (int i = 0; i < cards.Count; i++)
            Debug.Log(cards[i]);
        Debug.Log("Printed by Screen_Parent.CombosScreen, display on combos panel once one is made");
    }
}
