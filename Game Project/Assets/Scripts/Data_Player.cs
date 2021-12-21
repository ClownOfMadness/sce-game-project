using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stores data on a save, currently only used by parent
[System.Serializable]
public class Data_Player
{
    Map_Gen Map;
    //--------------------------------[Player config]------------------------------------
    public float TotalGameTime;    //in minutes, will be converted to hours for display??
    public float PlayTimeLimit;    //in minutes, will be converted to hours for display??
    public float Bedtime;
    public enum difficultiesList    //enum for difficulty
    {
        Normal,
        Easy,
        Hardcore,
    }
    public difficultiesList Difficulty;
    public int CardsCombined;
    public bool AllowedHints;
    public bool IsPremium;
    //-----------------------------------------------------------------------------------

    //-----------------------------------[Map data]--------------------------------------
    public static int MapSize;
    public int[,] MapArray = new int[MapSize, MapSize];

    //Tile Data
    public enum TileType
    {
        Sea,
        ShallowSea,
        Sand,
        Plains,
        Marsh,
        Forest,
        IronMine,
        Hill,
        Mountain,
        Ruins,
        Abyss,
    }


    //-----------------------------------------------------------------------------------


    //cards


    //units

    public Data_Player(Data_Player data)
    {
        TotalGameTime = data.TotalGameTime;
        PlayTimeLimit = data.PlayTimeLimit;
        Bedtime = data.Bedtime;
        Difficulty = data.Difficulty;
        CardsCombined = data.CardsCombined;
        AllowedHints = data.AllowedHints;
        IsPremium = data.IsPremium;
    }
    
}
