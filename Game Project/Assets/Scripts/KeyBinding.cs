using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeyBinding : MonoBehaviour
{
    public Dictionary<string, KeyCode> Keys = new Dictionary<string, KeyCode>(); //setting up a keys dictionary
    public Text Creative,Storage,Hints,Jobs,MoveUp,MoveDown,MoveRight,MoveLeft,Sprint;
    [HideInInspector] public GameObject CurrentKey; //
    Event e; //holds the key (e.keycode)
    private Color32 normal = new Color32(255, 255, 255, 255); //white
    private Color32 selected = new Color32(39, 171, 249, 255); //blue to highlight a selected button
    public GameObject ErrorMessage; //to display that the key is taken

    public Player_Control playerControl;

    private void Start()
    {
        //just a heads up: (KeyCode)System.Enum.Parse(typeof(KeyCode) is the code to convert a string to keycode
        //The Dictionary Default settings
        //The dictionary values
        Keys.Add("Creative", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Creative", "C")));
        Keys.Add("Storage", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Storage", "I")));
        Keys.Add("Hints", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Hints","H")));
        Keys.Add("Jobs", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jobs", "J")));
        Keys.Add("MoveUp", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveUp", "W")));
        Keys.Add("MoveDown", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveDown", "S")));
        Keys.Add("MoveRight", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveRight", "D")));
        Keys.Add("MoveLeft", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MoveLeft", "A")));
        Keys.Add("Sprint", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Sprint", "LeftShift")));

        //Attaching the correct text to display on the buttons
        Creative.text = Keys["Creative"].ToString();
        Storage.text = Keys["Storage"].ToString();
        Hints.text = Keys["Hints"].ToString();
        Jobs.text = Keys["Jobs"].ToString();
        MoveUp.text = Keys["MoveUp"].ToString();
        MoveDown.text = Keys["MoveDown"].ToString();
        MoveRight.text = Keys["MoveRight"].ToString();
        MoveLeft.text = Keys["MoveLeft"].ToString();
        Sprint.text = Keys["Sprint"].ToString();

        ErrorMessage.SetActive(false);
    }

    private void OnGUI() //changes the keys, (only OnGui works fast enough for this)
    {
        if (CurrentKey != null)
        {
            e = Event.current;
            if (e.isKey)
            {
                if (IsKeyFree(e.keyCode))
                {
                    ErrorMessage.SetActive(false);
                    Keys[CurrentKey.name] = e.keyCode;
                    CurrentKey.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
                    SetKey(e.keyCode); //the part that actually changes the key's functionality to match the new one
                }
                CurrentKey.GetComponent<Image>().color = normal;
                CurrentKey = null;
            }
        } 
    }

    public void ChangeKey(GameObject clicked) // the event for each button to become the referenced button
    {
        if (CurrentKey != null)
        {
            CurrentKey.GetComponent<Image>().color = normal;
        }
        CurrentKey=clicked;
        CurrentKey.GetComponent<Image>().color = selected;
    }

    public void SetKey(KeyCode ne) //temp atm -> should be changed to switch case when I get it to work
    {
        if (ne == Keys["Creative"])
        {
            Game_Master.creativeK = ne;
        }
        else if (ne == Keys["Storage"])
        {
            Game_Master.storageK = ne;
        }
        else if(ne == Keys["Hints"]) {
            Game_Master.Hintsk = ne;
        }
        else if (ne == Keys["Jobs"])
        {
            Game_Master.Jobsk = ne;
        }
        else if(ne == Keys["MoveUp"])
        {
            playerControl.up = ne;
        }
        else if (ne == Keys["MoveDown"])
        {
            playerControl.down = ne;
        }
        else if (ne == Keys["MoveRight"])
        {
            playerControl.right = ne;
        }
        else if (ne == Keys["MoveLeft"])
        {
            playerControl.left = ne;
        }
        else if (ne == Keys["Sprint"])
        {
            playerControl.sprint = ne;
        }
    }
    public string GetKey(string k)
    {
        if (k == "Creative")
        {
            return Keys["Creative"].ToString();
        }
        else if (k == "Storage")
        {
            return Keys["Storage"].ToString();
        }
        else if (k == "Hints")
        {
            return Keys["Hints"].ToString();
        }
        else if (k == "Jobs")
        {
            return Keys["Jobs"].ToString();
        }
        else if (k == "MoveUp")
        {
            return Keys["MoveUp"].ToString();
        }
        else if (k == "MoveDown")
        {
            return Keys["MoveDown"].ToString();
        }
        else if (k == "MoveRight")
        {
            return Keys["MoveRight"].ToString();
        }
        else if (k == "MoveLeft")
        {
            return Keys["MoveLeft"].ToString();
        }
        else if (k == "Sprint")
        {
            return Keys["Sprint"].ToString();
        }
        else return "B"; //just for failsafe
        //return "H";
    }

   /* public string GetKey(string k) //temp atm -> should be changed to switch case when I get it to work
    {
        if (ne == Keys["Creative"])
        {
            Game_Master.creativeK = ne;
        }
        else if (ne == Keys["Storage"])
        {
            Game_Master.storageK = ne;
        }
        else if (ne == Keys["Hints"])
        {
            Game_Master.Hintsk = ne;
        }
        else if (ne == Keys["Jobs"])
        {
            Game_Master.Jobsk = ne;
        }
        else if (ne == Keys["MoveUp"])
        {
            playerControl.up = ne;
        }
        else if (ne == Keys["MoveDown"])
        {
            playerControl.down = ne;
        }
        else if (ne == Keys["MoveRight"])
        {
            playerControl.right = ne;
        }
        else if (ne == Keys["MoveLeft"])
        {
            playerControl.left = ne;
        }
        else if (ne == Keys["Sprint"])
        {
            playerControl.sprint = ne;
        }
    }*/

    private bool IsKeyFree(KeyCode ne)
    {
        foreach (KeyCode tk in Keys.Values)
        {
            if (tk == ne)
            {
                ErrorMessage.SetActive(true);
                //Debug.Log("That key is already taken!");
                return false;
            }
        }
        return true;
    }
}
