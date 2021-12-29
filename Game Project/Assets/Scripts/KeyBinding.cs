using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeyBinding : MonoBehaviour
{
    private Dictionary<string, KeyCode> Keys = new Dictionary<string, KeyCode>(); //setting up a keys dictionary
    public Text Creative,Storage,Hints,Jobs,MoveUp,MoveDown,MoveRight,MoveLeft,Sprint;
    [HideInInspector] public GameObject CurrentKey;
    Event e; //holds the key (e.keycode)
    private Color32 normal = new Color32(255, 255, 255, 255); //white
    private Color32 selected = new Color32(39, 171, 249, 255); //blue to highlight a selected button

    public Player_Control playerControl;

    private void Start()
    {
        //The Dictionary Default settings
        //The dictionary values
        Keys.Add("Creative", KeyCode.C);
        Keys.Add("Storage", KeyCode.I);
        Keys.Add("Hints", KeyCode.H);
        Keys.Add("Jobs", KeyCode.J);
        Keys.Add("MoveUp", KeyCode.W);
        Keys.Add("MoveDown", KeyCode.S);
        Keys.Add("MoveRight", KeyCode.D);
        Keys.Add("MoveLeft", KeyCode.A);
        Keys.Add("Sprint", KeyCode.LeftShift);

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

    private void SetKey(KeyCode ne) //temp atm -> should be changed to switch case when I get it to work
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

    private bool IsKeyFree(KeyCode ne)
    {
        foreach (KeyCode tk in Keys.Values)
        {
            if (tk == ne)
            {
                Debug.Log("key is already taken!");
                return false;
            }
        }
        return true;
    }
}
