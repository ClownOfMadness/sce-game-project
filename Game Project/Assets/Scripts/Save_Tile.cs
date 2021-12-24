using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public enum TileType
//{
//    Sea,
//    ShallowSea,
//    Sand,
//    Plains,
//    Marsh,
//    Forest,
//    IronMine,
//    Hill,
//    Mountain,
//    Ruins,
//    Abyss
//}

[System.Serializable]
public class Save_Tile
{
    public int posX;
    public int posY;

    public int tileType;

    //-------------------------------------------[Data_Tile]-------------------------------------------

    // [Tile Information]
    public float height; 
    public bool gizmoUse; //private

    // [Tile Resources]
    public bool canRecharge; //private
    public bool canRandom; //private
    public GameObject theExtra; //private,GO
    public GameObject parentExtra; //GO
    public bool extra; //private
    public bool check; //private

    // [Tile Fog]
    public bool revealed;

    // [Tile Work]
    public GameObject unit; //private,GO
    public GameObject building; //GO
    public Data_Building dataBuilding; //Data_Building
    public bool hasTownHall;
    public bool hasBuilding; 
    public bool buildingComplete;
    public bool hasResources;

    // [Tile Build]
    public List<GameObject> builders = new List<GameObject>(); //List<GameObject>???, private
    public int progress; //private
    public bool buildDone; //private
    public bool canProgress; //private
    public float nextProgress; //private
    public AstarPath path; //AstarPath

    // [Abyss]
    public List<Data_Card> cards = new List<Data_Card>(); //List<Data_Card> cards???
    public List<GameObject> enemies = new List<GameObject>(); //List<GameObject>???, private
    public bool abyssSetup; //private
    public bool canSpawn; //private
    public bool hasAbyss; //private

    // [Find Once Function]???
    public Player_Control playerControl; //private, Player_Control
    public System_DayNight systemDayNight; //private, System_DayNight
    public Data_CommonDataHolder commonData; //private, Data_CommonDataHolder
    public Screen_Cards screenCards; //private, Screen_Cards
    public Enemy_List enemyList; //private, Enemy_List
    public int loadCount; //private

}