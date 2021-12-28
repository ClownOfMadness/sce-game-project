using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stores data on a save, currently only used by parent
[System.Serializable]
public class Data_Player
{
    //--------------------------------[Configuration]------------------------------------

    public bool SetTimeLimit;
    public float TimeLimit;
    public bool SetBedTime;
    public float BedTime;

    //public enum difficultiesList    //enum for difficulty
    //{
    //    Normal,
    //    Easy,
    //    Hardcore,
    //}
    //public difficultiesList Difficulty;
    //public int CardsCombined;
    //public bool AllowedHints;
    //public bool IsPremium;

    //-----------------------------------------------------------------------------------

    //------------------------------------[Game Data]------------------------------------

    //public Save_Tile[,] TileMap; //The map of tiles
    //public Vector3 townHallPos; //Town Hall position on the map

    public List<Map_Display.Save_Unit> units; //List of the units
    public List<Map_Display.Save_Enemy> enemies; //List of the enemies
                                     //public Save_Player player; //The data on the player character

    //[Cards Data]//
    public List<int> Hand; //Cards in hand
    public List<int> Storage; //Cards in storage
    public int Discovered;
    public int Combos;
    public List<bool> DisStatus;

    //-----------------------------------------------------------------------------------




    //public Data_Player(Data_Player data)
    //{
    //    TotalGameTime = data.TotalGameTime;
    //    PlayTimeLimit = data.PlayTimeLimit;
    //    Bedtime = data.Bedtime;
    //    Difficulty = data.Difficulty;
    //    CardsCombined = data.CardsCombined;
    //    AllowedHints = data.AllowedHints;
    //    IsPremium = data.IsPremium;
    //}
}

