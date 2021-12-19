using System.Collections.Generic;
using UnityEngine;

//allows you to create Data_Cards
[CreateAssetMenu(fileName = "0000", menuName = "Data_Card")]
public class Data_Card : ScriptableObject
{
    public enum typesList       //enum for type menu
    {                           //notice! changing order changes the selection in the exisiting Data_Cards
        Job,
        Basic,
        Special,
        Complex,
        Structure,
        Tool,
    }
    public enum sourcesList     //enum for source menu, replace with tile/buildings objects list when one is made
    {                           //notice! changing order changes the selection in the exisiting Data_Cards
        Menu,
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
        None,
    }
    [System.Serializable]   //needed for combinations to appear in inspector
    public struct Combinations  //struct for combinations (stores 2 at a time)
    {
        public Data_Card card1;
        public Data_Card card2;
    }

    public new string name;
    public int code;        //same as fileName
    public typesList type;
    public List<sourcesList> source;

    //for Data_Cards you can craft (type=Combination)
    public List<Combinations> combinationOf;

    //for Data_Cards you can gather (type=Basic,Complex,Special)
    public List<Data_Card> gatheredBy;

    //for Data_Cards that have tool durability (type=Job)
    public string tier;

    public int complexity;
    public string description;
    public Sprite artwork;

    public GameObject buildingPrefab;   //can interact with the map
    public string unitIndex;            //can interact with the map

    [HideInInspector] public bool neverDiscovered; //for statistics
}

    
