using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
    Abyss
}

[System.Serializable]
public class Save_Tile
{
    public int posX;
    public int posY;

    public TileType tileType;


}