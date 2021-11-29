using System.Collections.Generic;
using UnityEngine;

//allows you to create cards
[CreateAssetMenu(fileName = "0000", menuName = "Card")]
public class Card : ScriptableObject
{
    public enum typesList       //enum for type menu
    {                           //notice! changing order changes the selection in the exisiting cards
        Job,
        Basic,
        Special,
        Complex,
        Structure,
        Tool,
    }
    public enum sourcesList     //enum for source menu, replace with tile/buildings objects list when one is made
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
    [System.Serializable]   //needed for combinations to appear in inspector
    public struct Combinations  //struct for combinations (stores 2 at a time)
    {
        public Card card1;
        public Card card2;
    }

    public new string name;
    public int code;        //same as fileName
    public typesList type;
    public List<sourcesList> source;

    //for cards you can craft (type=Combination)
    public List<Combinations> combinationOf;

    //for cards you can gather (type=Basic,Complex,Special)
    public List<Card> gatheredBy;

    public int complexity;
    public string description;
    public Sprite artwork;

    //public ScriptableObject Building;
    public GameObject buildingPrefab;   //can interact with the map
}

    
