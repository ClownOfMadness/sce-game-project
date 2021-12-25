using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TileType
{
    Abyss,
    Forest,
    Hill,
    IronMine,
    Marsh,
    Mountain,
    Plains,
    Ruins,
    Sand,
    Sea,
    ShallowSea
}

[System.Serializable]
public class Save_Tile
{
    public int posX;
    public int posY;

    public int tileType;

    //-------------------------------------------[Data_Tile]-------------------------------------------

    // [Tile Information]
    public float height; 
    public bool gizmoUse;

    // [Tile Resources]
    public bool canRecharge;
    public bool canRandom;
    public GameObject theExtra; //GO
    public GameObject parentExtra; //GO
    public bool extra; 
    public bool check; 

    // [Tile Fog]
    public bool revealed;

    // [Tile Work]
    public GameObject unit; //GO
    public GameObject building; //GO
    public Data_Building dataBuilding; //Data_Building
    public bool hasTownHall;
    public bool hasBuilding; 
    public bool buildingComplete;
    public bool hasResources;

    // [Tile Build]
    public List<GameObject> builders; //List<GameObject>???
    public int progress; 
    public bool buildDone; 
    public bool canProgress; 
    public float nextProgress; 
    public AstarPath path; //AstarPath???

    // [Abyss]
    public List<Data_Card> cards; //List<Data_Card> cards???
    public List<GameObject> enemies; //List<GameObject>???
    public bool abyssSetup;
    public bool canSpawn;
    public bool hasAbyss;


    public void SaveData(Data_Tile data_tile, GameObject tile, int type_tile)
    {
        Data_Tile.Tile_Info info = data_tile.CreateTile_Info();

        height = data_tile.height;
        gizmoUse = info.gizmoUse;
        canRecharge = info.canRecharge;
        canRandom = info.canRandom;
        theExtra = info.theExtra; //GO
        parentExtra = data_tile.parentExtra; //GO
        extra = info.extra;
        check = info.check;
        revealed = data_tile.revealed;
        unit = info.unit; //GO
        building = data_tile.building; //GO
        dataBuilding = info.dataBuilding; //Class
        builders = info.builders; //list<Class>
        hasTownHall = data_tile.hasTownHall;
        hasBuilding = data_tile.hasBuilding;
        buildingComplete = data_tile.buildingComplete;
        hasResources = data_tile.hasResources;
        progress = info.progress;
        buildDone = info.buildDone;
        canProgress = info.canProgress;
        nextProgress = info.nextProgress;
        path = info.path; //Plugin
        cards = data_tile.cards; //List<Class>
        enemies = info.enemies; //List<Class>
        abyssSetup = info.abyssSetup;
        canSpawn = info.canSpawn;
        hasAbyss = info.hasAbyss;
}
}