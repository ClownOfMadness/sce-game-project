using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public class Save_Unit
//{
//    public int unitType; //Type identifier
//    public int unitGroup; //Group identifier
//    public int listIndex; //Index in the unit list

//    public float nextWander;
//    public bool impassable;
//    public Save_Tile currentTileOn;
//    public bool reachedTownHall;
//    public Save_Tile detectedImpassable;

//    // [Unit Control]
//    private Save_Building townHall;
//    //public Data_Card card; // Need to serialize
//    public bool busy;
//    public bool toTownHall;
//    public bool cardToDeliver;
//    public string buildingInteraction;

//    // [Unit Health]
//    public bool hurt;
//    public int durability;
//    public int health;
//    public float regenCD;
//    public float nextRegen;

//    // [Work Routine]
//    //public Data_Card workCard; // Need to serialize
//    public Save_Tile workPlace;
//    public bool working;
//    public bool workBegun;
//    public int workIndex;
//    public float workTime;
//    public float workDone;
//    public bool workExtra;

//    // [Build Routine]
//    public Save_Tile buildPlace;
//    public bool building;
//    public bool buildBegun;

//    // [Patrol Routine]
//    public bool patroling;
//    public Save_Tile patrolPlace;
//    public Save_Tile target;
//    //public Save_Tile spottedAbyss; // Not finalized
//    public Save_Enemy spottedEnemy;
//    public float nextAttack;

//    // [Work in memory]
//    public bool workInMemory;
//    public Save_TileData rmbTileData;
//    //public Data_Card rmbWorkCard; // Need to serialize
//    public Save_Tile rmbWorkPlace;
//    public int rmbWorkIndex;
//    public float rmbWorkTime;
//    public bool rmbWorkExtra;

//    // [Recieved from PlayerControl]
//    public Save_TileData tileData; // Tile Data that is recieved from the player
//}