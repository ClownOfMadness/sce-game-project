using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;

public class Map_Display : MonoBehaviour
{
    public GameObject GameMap;
    public GameObject Units;
    public GameObject Enemies;
    public Camera MainCamera;
    public AstarPath path;
    [HideInInspector] public GameObject TownHall;
    public Unit_List unitlist;

    //Classes used.
    public Game_Master GameMaster;
    private Map_Gen Map;
    private Player_SpawnBuilding SpawnBuilding;
    private Map_SpawnControl SpawnControl;
    private Save_Manager save_manager;
    private Screen_Cards screen_cards;
    private Unit_List unit_list;
    private Enemy_List enemy_list;
    private Player_Control player_control;
    private System_DayNight system_dayNight;

    private int size; //Map  

    private Data_Player saved_data;
    public Dictionary<int, GameObject> instances;

    private void Awake()
    {
        instances = new Dictionary<int, GameObject>();

        save_manager = FindObjectOfType<Save_Manager>();
        saved_data = save_manager.SceneLoaded();

        if (!(unitlist = GameObject.Find("Units").GetComponent<Unit_List>()))
        {
            Debug.LogError("Units gameobject or Unit_List is not found for Map_Display");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Map = FindObjectOfType<Map_Gen>();
        SpawnBuilding = FindObjectOfType<Player_SpawnBuilding>();
        SpawnControl = FindObjectOfType<Map_SpawnControl>();
        screen_cards = FindObjectOfType<Screen_Cards>();
        unit_list = FindObjectOfType<Unit_List>();
        enemy_list = FindObjectOfType<Enemy_List>();
        player_control = FindObjectOfType<Player_Control>();
        system_dayNight = FindObjectOfType<System_DayNight>();
        GameMaster = FindObjectOfType<Game_Master>();
        size = Map.mapSize;


        if (saved_data == null)
        {
            TownHall = Map.generateMap();
        }
        else
        {
            LoadData(saved_data);
        }
        MainCamera.transform.position = TownHall.transform.position + new Vector3(0, 150, 0);
        unitlist.townhall = TownHall;
        path.Scan();
    }

    void Update()
    {
        if (size != 0)
        {
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    Map.DataTileArray[x, y].UpdateMe();
        }
        if (GameMaster.CardsCombined < screen_cards.CardsCombined)
            GameMaster.CardsCombined = screen_cards.CardsCombined;
        if (GameMaster.CardsDiscovered < screen_cards.CardsDiscovered)
            GameMaster.CardsDiscovered = screen_cards.CardsDiscovered;
    }

    //---------------------------------------------------Load---------------------------------------------------//

    //Load player data.
    public bool LoadData(Data_Player data)
    {
        Cards_Info cards_info = new Cards_Info();

        LoadMap(data.TileMap, data.mapSize); //Load map
        LoadUnitList(data.units); //Load units
        LoadEnemyList(data.enemies); //Load 
        LoadDataOfItems(data);

        TownHall = FindReference(data.townHallTile);
        player_control.currentTileOn = FindReference(data.currentTileOn);
        player_control.CreatePlayer(player_control.currentTileOn);
        system_dayNight.currentTime = data.currentTime;
        system_dayNight.cycleSpeed = data.cycleSpeed;
        
        //Cards information//
        cards_info.Hand = data.Hand;
        cards_info.Storage = data.Storage;
        cards_info.DisStatus = data.DisStatus;
        screen_cards.ImportCards(cards_info);

        return true;
    }
    //Load all the data.
    public bool LoadDataOfItems(Data_Player data)
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                LoadTileData(data.TileMap[x, y].save_tileData, x, y);
            }
        }

        for (int i = 0; i < Units.transform.childCount; i++)
        {
            Transform group = Units.transform.GetChild(i);
            for (int j = 0; j < group.transform.childCount; j++)
            {
                Data_Unit.Unit_Info info = new Data_Unit.Unit_Info();
                GameObject unit = group.transform.GetChild(j).gameObject;

                info.nextWander = data.units[i][j].nextWander;
                info.impassable = data.units[i][j].impassable; ;
                info.currentTileOn = FindReference(data.units[i][j].currentTileOn);
                info.reachedTownHall = data.units[i][j].reachedTownHall; ;
                info.detectedImpassable = FindReference(data.units[i][j].detectedImpassable);
                info.townHall = FindReference(data.units[i][j].townHall);

                if (data.units[i][j].card != -1)
                    info.card = screen_cards.Pool.GetCard(data.units[i][j].card);
                else info.card = null;

                info.busy = data.units[i][j].busy;
                info.toTownHall = data.units[i][j].toTownHall;
                info.cardToDeliver = data.units[i][j].cardToDeliver;
                info.buildingInteraction = data.units[i][j].buildingInteraction;
                info.hurt = data.units[i][j].hurt;
                info.durability = data.units[i][j].durability;
                info.health = data.units[i][j].health;
                info.regenCD = data.units[i][j].regenCD;
                info.nextRegen = data.units[i][j].nextRegen;

                if (data.units[i][j].workCard != -1)
                    info.workCard = screen_cards.Pool.GetCard(data.units[i][j].workCard);
                else info.workCard = null;

                info.workPlace = FindReference(data.units[i][j].workPlace);
                info.working = data.units[i][j].working;
                info.workBegun = data.units[i][j].workBegun;
                info.workIndex = data.units[i][j].workIndex;
                info.workTime = data.units[i][j].workTime;
                info.workDone = data.units[i][j].workDone;
                info.workExtra = data.units[i][j].workExtra;
                info.buildPlace = FindReference(data.units[i][j].buildPlace);
                info.building = data.units[i][j].building;
                info.buildBegun = data.units[i][j].buildBegun;
                info.patroling = data.units[i][j].patroling;
                info.patrolPlace = FindReference(data.units[i][j].patrolPlace);
                info.target = FindReference(data.units[i][j].target);
                //public Save_Tile spottedAbyss; // Not finalized
                info.spottedEnemy = FindReference(data.units[i][j].spottedEnemy);
                info.nextAttack = data.units[i][j].nextAttack;
                info.workInMemory = data.units[i][j].workInMemory;

                if (FindReference(data.units[i][j].rmbTileData) != null)
                    info.rmbTileData = (FindReference(data.units[i][j].rmbTileData)).GetComponent<Data_Tile>();
                else info.rmbTileData = null;

                if (data.units[i][j].rmbWorkCard != -1)
                    info.rmbWorkCard = screen_cards.Pool.GetCard(data.units[i][j].rmbWorkCard);
                else info.rmbWorkCard = null;

                info.rmbWorkPlace = FindReference(data.units[i][j].rmbWorkPlace);
                info.rmbWorkIndex = data.units[i][j].rmbWorkIndex;
                info.rmbWorkTime = data.units[i][j].rmbWorkTime;
                info.rmbWorkExtra = data.units[i][j].rmbWorkExtra;

                if (FindReference(data.units[i][j].tileData) != null)
                    info.tileData = (FindReference(data.units[i][j].tileData)).GetComponent<Data_Tile>();
                else info.tileData = null;

                unit.GetComponent<Data_Unit>().LoadData(info);
            }
        }

        for (int i = 0; i < Enemies.transform.childCount; i++)
        {
            Transform group = Enemies.transform.GetChild(i);
            for (int j = 0; j < group.transform.childCount; j++)
            {
                Data_Enemy.Enemy_Info info = new Data_Enemy.Enemy_Info();
                GameObject enemy = group.transform.GetChild(j).gameObject;

                info.nextWander = data.enemies[i][j].nextWander;
                info.impassable = data.enemies[i][j].impassable;
                info.currentTileOn = FindReference(data.enemies[i][j].currentTileOn);
                info.previousTile = FindReference(data.enemies[i][j].previousTile);
                info.dataTile = (FindReference(data.enemies[i][j].dataTile)).GetComponent<Data_Tile>();
                info.reachedTown = data.enemies[i][j].reachedTown;
                info.reachedAbyss = data.enemies[i][j].reachedAbyss;
                info.target = FindReference(data.enemies[i][j].target);
                info.spottedUnit = FindReference(data.enemies[i][j].spottedUnit);
                info.spottedBuilding = FindReference(data.enemies[i][j].spottedBuilding);
                info.spottedTownHall = FindReference(data.enemies[i][j].spottedTownHall);
                //public Save_Player spottedPlayer; // Need to serialize
                info.nextAttack = data.enemies[i][j].nextAttack;
                info.abyss = FindReference(data.enemies[i][j].abyss);
                info.abyssData = (FindReference(data.enemies[i][j].abyssData)).GetComponent<Data_Tile>();
                
                if (data.enemies[i][j].card != -1)
                    info.card = screen_cards.Pool.GetCard(data.enemies[i][j].card);
                else info.card = null;

                info.busy = data.enemies[i][j].busy;
                info.health = data.enemies[i][j].health;
                enemy.GetComponent<Data_Enemy>().LoadData(info);
            }
        }
        return true;
    }
    //Load the tile map.
    public bool LoadMap(Save_Tile[,] tileMap, int size)
    {
        Map.TileArray = new GameObject[size, size];
        Map.DataTileArray = new Data_Tile[size, size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            { 
                LoadTile(tileMap[x, y], x, y);
            }
        }
        return true;
    }
    //Load tile.
    public bool LoadTile(Save_Tile tile, int x, int y)
    {
        if (tile.tileType != -1)
        {
            GameObject tilePrefab = convertTypeToTile(tile.tileType);

            Vector3 pos = new Vector3(x * 10, 0, y * 10);
            Map.TileArray[x, y] = Map.InstantiateTile(tilePrefab, pos, Map, tile.save_tileData.height);
            instances[tile.instanceID] = Map.TileArray[x, y];

            Map.DataTileArray[x, y] = Map.TileArray[x, y].GetComponent<Data_Tile>();

            if (Map.FogMap)//Create fog if it enabled.
            {
                GameObject thisFog = Instantiate(Map.fog, new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 0), Map.TileArray[x, y].transform);
                thisFog.transform.localPosition = new Vector3(0, 1, 0);
            }

            if (tile.save_tileData.hasBuilding || tile.save_tileData.hasTownHall)
                LoadBuilding(tile.building, Map.TileArray[x, y], saved_data.TileMap[x, y].save_tileData);
        }
        return true;
    }
    //Load data tile.
    public bool LoadTileData(Save_TileData data, int x, int y)
    {
        if (data.isNull != true)
        {
            Data_Tile.Tile_Info info = new Data_Tile.Tile_Info();

            info.height = data.height;
            info.gizmoUse = data.gizmoUse;
            info.canRecharge = data.canRecharge;
            info.canRandom = data.canRandom;
            //info.theExtra = data.theExtra; //GO
            //info.parentExtra = data.parentExtra; //GO
            info.extra = data.extra;
            info.check = data.check;
            
            if(GameMaster.fogOff)
                info.revealed = true;
            else
                info.revealed = data.revealed;

            info.unit = FindReference(data.unit);
            info.building = FindReference(data.building);

            if (FindReference(data.dataBuilding) != null)
                info.dataBuilding = (FindReference(data.dataBuilding)).GetComponent<Data_Building>();
            else info.dataBuilding = null;

            info.builders = new List<GameObject>();
            if (data.builders.Count > 0)
            {
                foreach (string builder in data.builders)
                    info.builders.Add(FindReference(builder));
            }

            info.hasTownHall = data.hasTownHall;
            info.hasBuilding = data.hasBuilding;
            info.buildingComplete = data.buildingComplete;
            info.hasResources = data.hasResources;
            info.progress = data.progress;
            info.buildDone = data.buildDone;
            info.nextProgress = data.nextProgress;

            info.cards = new List<Data_Card>();
            if (data.cards.Count > 0)
            { 
                foreach (int card in data.cards)
                    info.cards.Add(screen_cards.Pool.GetCard(card));
            }

            info.enemies = new List<GameObject>();
            if (data.enemies.Count > 0)
            {
                foreach (string enemy in data.enemies)
                    info.builders.Add(FindReference(enemy));
            }

            info.abyssSetup = data.abyssSetup;
            info.canSpawn = data.canSpawn;
            info.hasAbyss = data.hasAbyss;

            Map.DataTileArray[x, y].LoadData(info);
        }
        return true;
    }
    //Load building.
    public bool LoadBuilding(Save_Building building, GameObject tile, Save_TileData data)
    {
        Data_Building.Building_Info info = new Data_Building.Building_Info();

        Data_Tile.Tile_Info tile_data = new Data_Tile.Tile_Info();
        tile.GetComponent<Data_Tile>().revealed = true;
        tile.GetComponent<Data_Tile>().UpdateMe();

        GameObject newBuilding = convertTypeToBuilding(building.buildingType);
        if (newBuilding != null)
        {
            info.transparency = building.data.transparency;
            info.originalColor = building.data.originalColor;
            info.night = building.data.night;
            info.inProgress = building.data.inProgress;
            info.health = building.data.health;
            Data_Building data_newBuild = newBuilding.GetComponent<Data_Building>();
            data_newBuild.LoadData(info);

            if (data.buildingComplete)
                SpawnBuilding.SpawnReady(newBuilding, tile);
            else 
                SpawnBuilding.Spawn(newBuilding, tile);

            instances[building.instanceID] = tile.transform.GetChild(tile.transform.childCount - 1).gameObject;
        }
        return true;
    }
    //Load unit list.
    public bool LoadUnitList(List<List<Save_Unit>> units)
    {
        for (int i = 0; i < units.Count; i++)
        {
            for (int j = 0; j < units[i].Count; j++)
            {
                Data_Card card;
                if (units[i][j].unitType != -1)
                {
                    if (units[i][j].card != -1)
                        card = screen_cards.Pool.GetCard(units[i][j].card);
                    else card = null;

                    unit_list.SummonUnit(units[i][j].unitType, FindReference(units[i][j].currentTileOn), card, true);
                    instances[units[i][j].instanceID] = Units.transform.GetChild(i).transform.GetChild(j).gameObject;
                }
            }
        }
        return true;
    }
    //Load enemy list.
    public bool LoadEnemyList(List<List<Save_Enemy>> enemies)
    {
        foreach (List<Save_Enemy> group in enemies)
        {
            foreach (Save_Enemy enemy in group)
            {
                if (enemy.enemyType != -1)
                {
                    instances[enemy.instanceID] = enemy_list.CreateEnemy(enemy.enemyType, FindReference(enemy.abyss));
                }
            }
        }
        return true;
    }

    //----------------------------------------------------------------------------------------------------------//

    //---------------------------------------------------Save---------------------------------------------------//

    //Save player data.
    public Data_Player Save_Data()
    {
        Data_Player data = new Data_Player();

        data.mapSize = size; //Map size
        data.TileMap = SaveMap(); //Map info
        data.units = SaveUnitList(); //Units info
        data.enemies = SaveEnemyList(); //Enemies info

        data.townHallTile = CreateReferenceGO(TownHall); //Town hall tile
        data.currentTileOn = CreateReferenceGO(player_control.currentTileOn); //The current player's tile

        data.currentTime = system_dayNight.currentTime;
        data.cycleSpeed = system_dayNight.cycleSpeed;

        //Cards information//
        Cards_Info cards_info = screen_cards.ExportCards();
        data.Hand = cards_info.Hand;
        data.Storage = cards_info.Storage;
        data.DisStatus = cards_info.DisStatus;

        return data;
    }
    //Create serializable tile map.
    public Save_Tile[,] SaveMap()
    {
        Save_Tile[,] save_map = new Save_Tile[size, size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                save_map[x, y] = SaveTile(Map.TileArray[x, y], x, y);
            }
        }
        return save_map;
    }
    //Create serializable tile.
    public Save_Tile SaveTile(GameObject tile, int x, int y)
    {
        Save_Tile save_tile = new Save_Tile();

        if (tile == null)
            save_tile.tileType = -1;
        else
        {
            Data_Tile data_tile = Map.DataTileArray[x, y];

            save_tile.tileType = convertTileToType(data_tile);
            save_tile.instanceID = tile.GetInstanceID();

            save_tile.save_tileData = SaveTileData(data_tile);

            if (data_tile.hasBuilding || data_tile.hasTownHall)
            {
                save_tile.building = SaveBuilding(tile.transform.GetChild(tile.transform.childCount - 1).gameObject);
            }
            else save_tile.building = SaveBuilding(null);
        }
        return save_tile;
    }
    //Create serializable tile data.
    public Save_TileData SaveTileData(Data_Tile data_tile)
    {
        Save_TileData save_tileData = new Save_TileData();

        if (data_tile == null)
            save_tileData.isNull = true;
        else
        {
            Data_Tile.Tile_Info info = data_tile.SaveData();
            save_tileData.isNull = false;

            save_tileData.height = info.height;
            save_tileData.gizmoUse = info.gizmoUse;
            save_tileData.canRecharge = info.canRecharge;
            save_tileData.canRandom = info.canRandom;
            //theExtra = info.theExtra; //need to serialize
            //parentExtra = info.parentExtra; //need to serialize
            save_tileData.extra = info.extra;
            save_tileData.check = info.check;
            save_tileData.revealed = info.revealed;
            save_tileData.unit = CreateReferenceGO(info.unit);
            save_tileData.building = CreateReferenceGO(info.building);

            if (info.dataBuilding != null)
                save_tileData.dataBuilding = CreateReferenceGO(info.dataBuilding.gameObject);
            else
                save_tileData.dataBuilding = "null";

            save_tileData.builders = new List<string>();
            if (info.builders.Count > 0)
            {
                foreach (GameObject builder in info.builders)
                    save_tileData.builders.Add(CreateReferenceGO(builder));
            }

            save_tileData.hasTownHall = info.hasTownHall;
            save_tileData.hasBuilding = info.hasBuilding;
            save_tileData.buildingComplete = info.buildingComplete;
            save_tileData.hasResources = info.hasResources;
            save_tileData.progress = info.progress;
            save_tileData.buildDone = info.buildDone;
            save_tileData.nextProgress = info.nextProgress;

            save_tileData.cards = new List<int>();
            if (info.cards.Count > 0)
            {
                foreach (Data_Card card in info.cards)
                    save_tileData.cards.Add(card.code);
            }

            save_tileData.enemies = new List<string>();
            if (info.enemies.Count > 0)
            {
                foreach (GameObject enemy in info.enemies)
                    save_tileData.enemies.Add(CreateReferenceGO(enemy));
            }

            save_tileData.abyssSetup = info.abyssSetup;
            save_tileData.canSpawn = info.canSpawn;
            save_tileData.hasAbyss = info.hasAbyss;
        }
        return save_tileData;
    }
    //Create serializable building.
    public Save_Building SaveBuilding(GameObject building)
    {
        Save_Building save_building = new Save_Building();

        if (building == null)
        {
            save_building.buildingType = -1;
            save_building.instanceID = -1;
            save_building.data = SaveBuildingData(null);
        }
        else
        {
            save_building.buildingType = convertBuildingToType(building);
            save_building.instanceID = building.GetInstanceID();
            save_building.data = SaveBuildingData(building.GetComponent<Data_Building>());
        }
        return save_building;
    }
    //Create serializable building data.
    public Save_BuildingData SaveBuildingData(Data_Building data_building)
    {
        Save_BuildingData data = new Save_BuildingData();

        if (data_building == null)
        {
            data.isNull = true;
            data.transparency = Color.white;
            data.originalColor = Color.white;
            data.night = false;
            data.inProgress = false;
            data.health = 0;
        }
        else
        {
            Data_Building.Building_Info info = data_building.SaveData();
            data.isNull = false;

            data.transparency = info.transparency;
            data.originalColor = info.originalColor;
            data.night = info.night;
            data.inProgress = info.inProgress;
            data.health = info.health;
        }
        return data;
    }
    //Create serializable unit list.
    public List<List<Save_Unit>> SaveUnitList()
    {
        List<List<Save_Unit>> list = new List<List<Save_Unit>>();

        foreach (Transform group in Units.transform)
        {
            List<Save_Unit> groupList = new List<Save_Unit>();
            foreach (Transform child in group.transform)
            {
                groupList.Add(SaveUnit(child.gameObject));
            }
            list.Add(groupList);
        }
        return list;
    }
    //Create serializable unit.
    public Save_Unit SaveUnit(GameObject unitPrefab)
    {
        Save_Unit unit = new Save_Unit();
        Data_Unit.Unit_Info info;

        if (unitPrefab == null)
            unit.unitType = -1;
        else
        {
            info = unitPrefab.GetComponent<Data_Unit>().SaveData();

            unit.unitType = convertUnitToType(unitPrefab);
            unit.instanceID = unitPrefab.GetInstanceID();

            unit.nextWander = info.nextWander;
            unit.impassable = info.impassable;
            unit.currentTileOn = CreateReferenceGO(info.currentTileOn);
            unit.reachedTownHall = info.reachedTownHall;
            unit.detectedImpassable = CreateReferenceGO(info.detectedImpassable);
            unit.townHall = CreateReferenceGO(info.townHall);

            if (info.card != null) unit.card = info.card.code;
            else unit.card = -1;

            unit.busy = info.busy;
            unit.toTownHall = info.toTownHall;
            unit.cardToDeliver = info.cardToDeliver;
            unit.buildingInteraction = info.buildingInteraction;
            unit.hurt = info.hurt;
            unit.durability = info.durability;
            unit.health = info.health;
            unit.regenCD = info.regenCD;
            unit.nextRegen = info.nextRegen;

            if (info.workCard != null) unit.workCard = info.workCard.code;
            else unit.workCard = -1;

            unit.workPlace = CreateReferenceGO(info.workPlace);
            unit.working = info.working;
            unit.workBegun = info.workBegun;
            unit.workIndex = info.workIndex;
            unit.workTime = info.workTime;
            unit.workDone = info.workDone;
            unit.workExtra = info.workExtra;
            unit.buildPlace = CreateReferenceGO(info.buildPlace);
            unit.building = info.building;
            unit.buildBegun = info.buildBegun;
            unit.patroling = info.patroling;
            unit.patrolPlace = CreateReferenceGO(info.patrolPlace);
            unit.target = CreateReferenceGO(info.target);
            //public Save_Tile spottedAbyss; // Not finalized
            unit.spottedEnemy = CreateReferenceGO(info.spottedEnemy);
            unit.nextAttack = info.nextAttack;
            unit.workInMemory = info.workInMemory;

            if (info.rmbTileData != null)
                unit.rmbTileData = CreateReferenceGO(info.rmbTileData.gameObject);
            else
                unit.rmbTileData = "null";

            if (info.rmbWorkCard != null) unit.rmbWorkCard = info.rmbWorkCard.code;
            else unit.rmbWorkCard = -1;

            unit.rmbWorkPlace = CreateReferenceGO(info.rmbWorkPlace);
            unit.rmbWorkIndex = info.rmbWorkIndex;
            unit.rmbWorkTime = info.rmbWorkTime;
            unit.rmbWorkExtra = info.rmbWorkExtra;

            if (info.tileData != null)
                unit.tileData = CreateReferenceGO(info.tileData.gameObject);
            else
                unit.tileData = "null";
        }
        return unit;
    }
    //Create serializable enemy list.
    public List<List<Save_Enemy>> SaveEnemyList()
    {
        List<List<Save_Enemy>> list = new List<List<Save_Enemy>>();

        foreach (Transform group in Enemies.transform)
        {
            List<Save_Enemy> groupList = new List<Save_Enemy>();
            foreach (Transform enemy in group.transform)
                groupList.Add(SaveEnemy(enemy.gameObject));
            list.Add(groupList);
        }
        return list;
    }
    //Create serializable enemy
    public Save_Enemy SaveEnemy(GameObject enemyPrefab)
    {
        Save_Enemy enemy = new Save_Enemy();
        if (enemyPrefab == null)
            enemy.enemyType = -1;
        else
        {
            Data_Enemy.Enemy_Info info = enemyPrefab.GetComponent<Data_Enemy>().SaveData();
            enemy.enemyType = convertEnemyToType(enemyPrefab); //Type identifier
            enemy.instanceID = enemyPrefab.GetInstanceID();

            enemy.nextWander = info.nextWander;
            enemy.impassable = info.impassable;
            enemy.currentTileOn = CreateReferenceGO(info.currentTileOn);
            enemy.previousTile = CreateReferenceGO(info.previousTile);

            if (enemy.dataTile != null)
                enemy.dataTile = CreateReferenceGO(info.dataTile.gameObject);
            else
                enemy.dataTile = "null";

            enemy.reachedTown = info.reachedTown;
            enemy.reachedAbyss = info.reachedAbyss;
            enemy.target = CreateReferenceGO(info.target);
            enemy.spottedUnit = CreateReferenceGO(info.spottedUnit);
            enemy.spottedBuilding = CreateReferenceGO(info.spottedBuilding);
            enemy.spottedTownHall = CreateReferenceGO(info.spottedTownHall);
            //public Save_Player spottedPlayer; // Need to serialize
            enemy.nextAttack = info.nextAttack;
            enemy.abyss = CreateReferenceGO(info.abyss);

            if (enemy.abyssData != null)
                enemy.abyssData = CreateReferenceGO(info.abyssData.gameObject);
            else
                enemy.abyssData = "null";

            if (info.card != null) enemy.card = info.card.code;
            else enemy.card = -1;

            enemy.busy = info.busy;
            enemy.health = info.health;
        }
        return enemy;
    }

    //----------------------------------------------------------------------------------------------------------//

    //------------------------------------------------Convertion------------------------------------------------//

    //Create reference.
    public string CreateReferenceGO(GameObject obj)
    {
        if (obj != null)
            return obj.GetInstanceID().ToString();
        return "null";
    }
    //Find reference.
    public GameObject FindReference(string reference)
    {
        if (reference != "null")
            return instances[int.Parse(reference)];
        return null;
    }
    //Convertion Enemy >>> EnemyType
    public int convertEnemyToType(GameObject enemyPrefab)
    {
        Enemy_List.Enemies[] enemyTypes = enemy_list.enemies;

        for (int i = 0; i < enemyTypes.Length; i++)
        {
            if (enemyTypes[i].enemyPrefab.name == enemyPrefab.GetComponent<Data_Enemy>().enemyType)
                return i;
        }
        return -1;
    }
    //Convertion Unit >>> UnitType
    public int convertUnitToType(GameObject unitPrefab)
    {
        Unit_List.Units[] unitTypes = unit_list.units;

        for (int i = 0; i < unitTypes.Length; i++)
        {
            if (unitTypes[i].unitPrefab.name == unitPrefab.GetComponent<Data_Unit>().unitJob)
                return i;
        }
        return -1;
    }
    //Convertion Building >>> BuildingType
    public int convertBuildingToType(GameObject building)
    {
        for (int i = 0; i < SpawnBuilding.BuildingList.Count; i++)
            if (SpawnBuilding.BuildingList[i].name == building.name)
                return i;
        return -1;
    }
    //Convertion BuildingType >>> Building
    public GameObject convertTypeToBuilding(int type)
    {
        for (int i = 0; i < SpawnBuilding.BuildingList.Count; i++)
            if (i == type)
                return SpawnBuilding.BuildingList[i];
        return null;
    }
    //Convertion Tile >>> TileType
    public int convertTileToType(Data_Tile tile)
    {
        for (int i = 0; i < Map.tileList.Count; i++)
            if (Map.tileList[i].name == tile.tileName)
                return i;
        return -1;
    }
    //Convertion TileType >>> Tile
    public GameObject convertTypeToTile(int type)
    {
        for (int i = 0; i < Map.tileList.Count; i++)
            if (i == type)
                return Map.tileList[i];
        return null;
    }

    //----------------------------------------------------------------------------------------------------------//

    //--------------------------------------------Serializable_Types--------------------------------------------//
    [System.Serializable]
    public class Save_Tile //Tile
    {
        public int tileType;
        public int instanceID;

        public Save_TileData save_tileData;

        //Children//
        public Save_Building building;
    }

    [System.Serializable]
    public class Save_TileData //Tile data
    {
        public bool isNull; //null identifier

        // [Tile Information] //
        public float height;
        public bool gizmoUse;

        // [Tile Resources] //
        public bool canRecharge;
        public bool canRandom;
        //public string theExtra; //Extra
        //public GameObject parentExtra; //Not used, unknown type
        public bool extra;
        public bool check;

        // [Tile Fog]//
        public bool revealed;

        // [Tile Work] //
        public string unit; //Unit
        public string building; //Building
        public string dataBuilding; //BuildingData
        public bool hasTownHall;
        public bool hasBuilding;
        public bool buildingComplete;
        public bool hasResources;

        // [Tile Build] //
        public List<string> builders; //Unit
        public int progress;
        public bool buildDone;
        public bool canProgress;
        public float nextProgress;

        // [Abyss]
        public List<int> cards; // data_card
        public List<string> enemies; //Enemy
        public bool abyssSetup;
        public bool canSpawn;
        public bool hasAbyss;
    }

    [System.Serializable]
    public class Save_Extra //Extra to tile
    {
        public int instanceID;
        public int typeExtra;
    }

    [System.Serializable]
    public class Save_Building //Building
    {
        public int buildingType;
        public int instanceID;

        public Save_BuildingData data;
    }

    [System.Serializable]
    public class Save_BuildingData //Building data
    {
        public bool isNull; //null identifier

        public Color transparency;
        public Color originalColor;
        public bool night;
        public bool inProgress;
        public int health;
    }

    [System.Serializable]
    public class Save_Unit //Unit
    {
        public int unitType; //Type identifier
        public int instanceID;

        public float nextWander;
        public bool impassable;
        public string currentTileOn; //Tile
        public bool reachedTownHall;
        public string detectedImpassable; //Tile

        // [Unit Control]
        public string townHall; //Building
        public int card; // data_card
        public bool busy;
        public bool toTownHall;
        public bool cardToDeliver;
        public string buildingInteraction;

        // [Unit Health]
        public bool hurt;
        public int durability;
        public int health;
        public float regenCD;
        public float nextRegen;

        // [Work Routine]
        public int workCard; // card_data
        public string workPlace; //Tile
        public bool working;
        public bool workBegun;
        public int workIndex;
        public float workTime;
        public float workDone;
        public bool workExtra;

        // [Build Routine]
        public string buildPlace; //Tile
        public bool building;
        public bool buildBegun;

        // [Patrol Routine]
        public bool patroling;
        public string patrolPlace; //Tile
        public string target; //Tile
        //public Save_Tile spottedAbyss; // Not finalized
        public string spottedEnemy; //Enemy
        public float nextAttack;

        // [Work in memory]
        public bool workInMemory;
        public string rmbTileData;
        public int rmbWorkCard; // data_card
        public string rmbWorkPlace; //Tile
        public int rmbWorkIndex;
        public float rmbWorkTime;
        public bool rmbWorkExtra;

        // [Recieved from PlayerControl]
        public string tileData;
    }

    [System.Serializable]
    public class Save_Enemy //Enemy
    {
        public int enemyType; //Type identifier
        public int instanceID;

        public float nextWander;
        public bool impassable;
        public string currentTileOn; //Tile
        public string previousTile; //Tile
        public string dataTile;

        // [Enemy Chase]
        public bool reachedTown;
        public bool reachedAbyss;
        public string target; //Unit
        public string spottedUnit; //Unit
        public string spottedBuilding; //Building
        public string spottedTownHall; //Building
        //public Save_Player spottedPlayer; // Need to serialize
        public float nextAttack;

        // [Enemy Control]
        public string abyss; //Tile
        public string abyssData;
        public int card; // data_card
        public bool busy;

        // [Enemy Health]
        public int health;
    }

    [System.Serializable]
    public class Save_Player //Player character information(?)
    {

    }

    //----------------------------------------------------------------------------------------------------------//
}
