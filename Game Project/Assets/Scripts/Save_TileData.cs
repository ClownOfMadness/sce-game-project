using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save_TileData
{
    // [Tile Information] //
    public float height;
    public bool gizmoUse;

    // [Tile Resources] //
    public bool canRecharge;
    public bool canRandom;
    //public Save_Extra theExtra;
    //public GameObject parentExtra; //Not used, unknown type
    public bool extra;
    public bool check;

    // [Tile Fog]//
    public bool revealed;

    // [Tile Work] //
    //public Save_Unit unit;
    public int building;
    //public Save_BuildingData dataBuilding;
    public bool hasTownHall;
    public bool hasBuilding;
    public bool buildingComplete;
    public bool hasResources;

    // [Tile Build] //
    //public List<Save_Unit> builders;
    public int progress;
    public bool buildDone;
    public bool canProgress;
    public float nextProgress;

    // [Abyss]
    //public List<Data_Card> cards; //List<Data_Card> cards???
    //public List<Save_Enemy> enemies;
    public bool abyssSetup;
    public bool canSpawn;
    public bool hasAbyss;
}
