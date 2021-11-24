using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDropZone1 { void DropZone1(); }
[CreateAssetMenu(fileName = "0000", menuName = "Card")] //allows you to create cards
public class Card : ScriptableObject
{
    //The basic card stuff
    public enum typesList       //enum for type menu
    {                           //notice! changing order changes the selection in the exisiting cards
        Job,
        Basic,
        Special,
        Complex,
        Structure,
        Tool,
    }

    public enum sourcesList     //enum for source menu
    {                           //notice! changing order changes the selection in the exisiting cards
        None,
        Combination,
        River,
        Plains,
        Hills,
        Animals,
        Forest,
        Mountain,
        Marsh,
        CoalMine,
        IronMine,
        GoldMine,
        Beach,
        TownHall,
        House,
        Farm,
    }
    [System.Serializable]
    public struct combinations  //struct for combinations (stores 2 at a time)
    {
        public Card Card_1;
        public Card Card_2;
    }

    public new string name;
    public int code;    //same as fileName
    public typesList type;
    public List<sourcesList> source;

    //for cards you can craft (type=Combination)
    public List<combinations> combination_of;

    //for cards you can gather (type=Basic,Complex,Special)
    public List<Card> gathered_by;

    public int complexity;
    public string description;
    public Sprite artwork;

    public void PrintMessage()
    {
        Debug.Log("Card " + code + " succesefully added to cards dictionary.");
    }
}

    
