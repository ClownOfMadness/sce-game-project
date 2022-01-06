using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data_Player
{
    private static Data_Player _current;
    public static Data_Player current
    {
        get
        {
            if(_current == null)
            {
                _current = new Data_Player();
            }
            return _current;
        }
    }

    //------------------------------------[Game Data]------------------------------------//

    //[Map Data]//
    public int mapSize; //The size of the map
    public Map_Display.Save_Tile[,] TileMap; //The map of tiles
    public string townHallTile; //Town Hall tile

    //[Characters data]//
    public List<List<Map_Display.Save_Unit>> units; //List of the units
    public List<List<Map_Display.Save_Enemy>> enemies; //List of the enemies

    //[Player Control]//
    public string currentTileOn;
    //public Save_Player player; //The player character

    //[Day/Night System]//
    public int currentTime;
    public float cycleSpeed;

    //[Cards Data]//
    public List<int> Hand; //Cards in hand
    public List<int> Storage; //Cards in storage
    public List<bool> DisStatus;

    //-----------------------------------------------------------------------------------//

    public Data_Player()
    {
        mapSize = 0;
        TileMap = null;
        townHallTile = null;
        units = null;
        enemies = null;
        currentTileOn = null;
        currentTime = 0;
        cycleSpeed = 0;
        Hand = null;
        Storage = null;
        DisStatus = null;
    }
}

