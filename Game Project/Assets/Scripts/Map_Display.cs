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
    private Map_Gen Map;
    private Player_SpawnBuilding SpawnBuilding;
    private Map_SpawnControl SpawnControl;
    private Save_Manager save_manager;
    private Screen_Cards screen_cards;
    private Unit_List unit_list;
    private Enemy_List enemy_list;

    int size; //Map size

    public Data_Player saved_data;


    private void Awake()
    {
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
        size = Map.mapSize;

        //if (saved_data == null)
        //{
            TownHall = Map.generateMap();
            MainCamera.transform.position = TownHall.transform.position + new Vector3(0, 150, 0);
            unitlist.townhall = TownHall;
        //}
        if(saved_data != null) Load_Data(saved_data);
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
    }

    //-----------------Load-----------------//

    //Load player data.
    public void Load_Data(Data_Player data)
    {
        Screen_Cards.Cards_Info cards_info = new Screen_Cards.Cards_Info();

        LoadMap(data.TileMap, data.mapSize);
        //LoadUnitList();
        //LoadEnemyList();

        cards_info.Hand = data.Hand;
        cards_info.Storage = data.Storage;
        cards_info.Discovered = data.Discovered;
        cards_info.Combos = data.Combos;
        cards_info.DisStatus = data.DisStatus;
        screen_cards.ImportCards(cards_info);
    }
    //Laod the tile map.
    public void LoadMap(Save_Tile[,] tileMap, int size)
    {
        Map.TileArray = new GameObject[size, size];
        Map.DataTileArray = new Data_Tile[size, size];

        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                LoadTile(tileMap[x, y], x, y);
    }
    public void LoadTile(Save_Tile tile, int x, int y)
    { 
        Vector3 pos = new Vector3(x * 10, 0, y * 10);
        Map.TileArray[x, y] = Map.InstantiateTile(convertTypeToTile(tile.tileType), pos, Map, tile.save_tileData.height);
        if (Map.FogMap)//Create fog if it enabled.
        {
            GameObject thisFog = Instantiate(Map.fog, new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 0), Map.TileArray[x, y].transform);
            thisFog.transform.localPosition = new Vector3(0, 1, 0);
        }
        Map.TileArray[x, y].AddComponent<Data_Tile>().LoadData(LoadTileData(tile.save_tileData));
        if (tile.building.buildingType != -1)
        {
            //LoadBuilding(tile.building, Map.TileArray[x, y]);
        }
    }
    public Data_Tile.Tile_Info LoadTileData(Save_TileData data)
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
        info.revealed = data.revealed;
        //info,unit = data.unit; //GO
        info.building = convertTypeToBuilding(data.building);
        //info.dataBuilding = data.dataBuilding; //Class
        //info.builders = data.builders; //list<Class>
        info.hasTownHall = data.hasTownHall;
        info.hasBuilding = data.hasBuilding;
        info.buildingComplete = data.buildingComplete;
        info.hasResources = data.hasResources;
        info.progress = data.progress;
        info.buildDone = data.buildDone;
        info.nextProgress = data.nextProgress;
        //info.cards = data.cards; //List<Class>
        //info.enemies = data.enemies; //List<Class>
        info.abyssSetup = data.abyssSetup;
        info.canSpawn = data.canSpawn;
        info.hasAbyss = data.hasAbyss;

        return info;
    }
    //public void LoadBuilding(Save_Building building, GameObject tile)
    //{
    //    Data_Building.Building_Info info = new Data_Building.Building_Info();

    //    GameObject newBuilding = convertTypeToBuilding(building.buildingType);
    //    if (newBuilding != null)
    //    {
    //        SpawnBuilding.Spawn(newBuilding, tile);

    //        info.transparency = building.transparency;
    //        info.originalColor = building.originalColor;
    //        info.night = building.night;
    //        info.inProgress = building.inProgress;
    //        info.health = building.health;

    //        newBuilding.AddComponent<Data_Building>().LoadData(info);
    //    }
    //}

    public void LoadUnitList(List<Save_Unit> units)
    {
        Unit_List.Units unitTypes = new Unit_List.Units();
        foreach (Save_Unit unit in units)
        {
            //LoadUnit(unit, unitTypes);
        }
    }

    public void LoadEnemyList(List<Save_Enemy> enemies)
    {
        //Unit_List.Units unitTypes = new Unit_List.Units();
        foreach (Save_Enemy enemy in enemies)
        {
            //LoadUnit(unit, unitTypes);
        }
    }

    //public void LoadUnit(Save_Unit unit)
    //{
    //    Data_Unit.Unit_Info info = new Data_Unit.Unit_Info();

    //    if (unit.unitType != -1)
    //    {
    //        unit_list.SummonUnit(unit.unitType, TileArray[x, y], null, true);

    //        info.nextWander = unit.nextWander;
    //        info.impassable = unit.impassable; ;
    //        //info.currentTileOn = SaveTile(info.currentTileOn.GetComponent<Data_Tile>(), info.currentTileOn);//null?
    //        info.reachedTownHall = unit.reachedTownHall; ;
    //        //info.detectedImpassable = SaveTile(info.detectedImpassable.GetComponent<Data_Tile>(), info.detectedImpassable);//null?;
    //        //info.townHall = SaveBuilding(info.townHall);
    //        info.card = screen_cards.Pool.GetCard(unit.card);
    //        info.busy = unit.busy; ;
    //        info.toTownHall = unit.toTownHall;
    //        info.cardToDeliver = unit.cardToDeliver;
    //        info.buildingInteraction = unit.buildingInteraction;
    //        info.hurt = unit.hurt;
    //        info.durability = unit.durability;
    //        info.health = unit.health;
    //        info.regenCD = unit.regenCD;
    //        info.nextRegen = unit.nextRegen;
    //        //info.workCard;
    //        //info.workPlace = SaveTile(info.workPlace.GetComponent<Data_Tile>(), info.workPlace);//null?
    //        info.working = unit.working;
    //        info.workBegun = unit.workBegun;
    //        info.workIndex = unit.workIndex;
    //        info.workTime = unit.workTime;
    //        info.workDone = unit.workDone;
    //        info.workExtra = unit.workExtra;
    //        //info.buildPlace = SaveTile(info.buildPlace.GetComponent<Data_Tile>(), info.buildPlace);//null?
    //        info.building = unit.building;
    //        info.buildBegun = unit.buildBegun;
    //        info.patroling = unit.patroling;
    //        //info.patrolPlace = SaveTile(info.patrolPlace.GetComponent<Data_Tile>(), info.patrolPlace);//null?;
    //        //info.target = SaveTile(info.target.GetComponent<Data_Tile>(), info.target);//null?;;
    //        //public Save_Tile spottedAbyss; // Not finalized
    //        //info.spottedEnemy = Save_Enemy();
    //        info.nextAttack = unit.nextAttack;
    //        info.workInMemory = unit.workInMemory;
    //        //info.rmbTileData = SaveTileData(info.rmbTileData.SaveData());
    //        //public Data_Card rmbWorkCard; // Need to serialize
    //        //info.rmbWorkPlace = SaveTile(info.rmbWorkPlace.GetComponent<Data_Tile>(), info.rmbWorkPlace);//null?;;;
    //        info.rmbWorkIndex = unit.rmbWorkIndex;
    //        info.rmbWorkTime = unit.rmbWorkTime;
    //        info.rmbWorkExtra = unit.rmbWorkExtra;
    //        //info.tileData = SaveTileData(info.tileData.SaveData());
    //    }
    //}

    //public void LoadEnemy(Save_Enemy enemy)
    //{
    //    Data_Enemy.Enemy_Info info = new Data_Enemy.Enemy_Info();

    //    if (enemy.enemyType != -1)
    //    {
    //        info.nextWander = enemy.nextWander;
    //        info.impassable = enemy.impassable;
    //        //info.currentTileOn = SaveTile(info.currentTileOn.GetComponent<Data_Tile>(), info.currentTileOn);//null?
    //        //info.previousTile = SaveTile(info.previousTile.GetComponent<Data_Tile>(), info.previousTile);//null?;
    //        //info.dataTile = SaveTileData(info.dataTile.SaveData());
    //        info.reachedTown = enemy.reachedTown;
    //        info.reachedAbyss = enemy.reachedAbyss;
    //        //info.target = SaveUnit(info.target.GetComponent<Data_Unit>(), info.target);//null?
    //        //info.spottedUnit = SaveUnit(info.spottedUnit.GetComponent<Data_Unit>(), info.spottedUnit);//null?
    //        //info.spottedBuilding = SaveBuilding(info.spottedBuilding);
    //        //info.spottedTownHall = SaveBuilding(info.spottedTownHall);
    //        //public Save_Player spottedPlayer; // Need to serialize
    //        info.nextAttack = enemy.nextAttack;
    //        //info.abyss = SaveTile(info.abyss.GetComponent<Data_Tile>(), info.abyss);//null?
    //        //info.abyssData = SaveTileData(info.abyssData.SaveData());
    //        //public Data_Card card; // Need to serialize
    //        info.busy = enemy.busy;
    //        info.health = enemy.health;
    //        return info;
    //    }
    //}
    //--------------------------------------//

    //-----------------Save-----------------//
    //Save player data.
    public Data_Player Save_Data()
    {
        Data_Player data = new Data_Player();
        //Screen_Cards.Cards_Info cards_info;

        //data.TileMap = SaveMap();
        //data.units = SaveUnitList();
        //data.enemies = SaveEnemies();

        //cards_info = screen_cards.ExportCards();
        //data.Hand = cards_info.Hand;
        //data.Storage = cards_info.Storage;
        //data.Discovered = cards_info.Discovered;
        //data.Combos = cards_info.Combos;
        //data.DisStatus = cards_info.DisStatus;


        //data.building = SaveBuilding(TownHall.transform.GetChild(TownHall.transform.childCount - 1).gameObject);
        return data;
    }

    //Create serializable tile map.
    //public Save_Tile[,] SaveMap()
    //{
    //    Save_Tile[,] save_map = new Save_Tile[size, size];

    //    for (int y = 0; y < size; y++)
    //        for (int x = 0; x < size; x++)
    //            //SaveTile(Map.DataTileArray[x, y], Map.TileArray[x, y]);

    //    return save_map;
    //}

    //Create serializable building.
    //public Save_Building SaveBuilding(GameObject building)
    //{
    //    Save_Building save_building = new Save_Building();
    //    Data_Building.Building_Info info = new Data_Building.Building_Info();

    //    save_building.buildingType = convertBuildingToType(building);

    //    if (save_building.buildingType != -1)
    //    {
    //        save_building.transparency = info.transparency;
    //        save_building.originalColor = info.originalColor;
    //        save_building.night = info.night;
    //        save_building.inProgress = info.inProgress;
    //        save_building.health = info.health;
    //    }
    //    return save_building;
    //}

    //Create serializable tile.
    public Save_Tile SaveTile(GameObject tile, bool isInvoke, int x, int y)
    {
        Save_Tile save_tile = new Save_Tile();
        Data_Tile data_tile;

        if (tile == null)
            save_tile.tileType = -1;
        else
        {
            if (isInvoke == true)
                data_tile = Map.DataTileArray[x, y];
            else data_tile = tile.GetComponent<Data_Tile>();

            save_tile.tileType = convertTileToType(data_tile);

            //save_tile.save_tileData = SaveTileData(data_tile);
            if (save_tile.save_tileData.hasBuilding || save_tile.save_tileData.hasTownHall)
            {
                //save_tile.building = SaveBuilding(tile.transform.GetChild(tile.transform.childCount - 1).gameObject);
            }
        }
        return save_tile;
    }

    //public Save_TileData SaveTileData(Data_Tile data_tile)
    //{
    //    Save_TileData save_tileData = new Save_TileData();

    //    if (data_tile == null)
    //        save_tileData.isNull = true;
    //    else
    //    {
    //        Data_Tile.Tile_Info info = data_tile.SaveData();
    //        save_tileData.isNull = false;

    //        save_tileData.height = info.height;
    //        save_tileData.gizmoUse = info.gizmoUse;
    //        save_tileData.canRecharge = info.canRecharge;
    //        save_tileData.canRandom = info.canRandom;
    //        //theExtra = info.theExtra; //need to serialize
    //        //parentExtra = info.parentExtra; //need to serialize
    //        save_tileData.extra = info.extra;
    //        save_tileData.check = info.check;
    //        save_tileData.revealed = info.revealed;
    //        save_tileData.unit = SaveUnit(info.unit); 
    //        save_tileData.building = convertBuildingToType(info.building);
    //        save_tileData.dataBuilding = SaveBuildingData(info.dataBuilding);

    //        save_tileData.builders = new List<Save_Unit>();
    //        foreach (GameObject builder in info.builders)
    //            save_tileData.builders.Add(SaveUnit(builder));

    //        save_tileData.hasTownHall = info.hasTownHall;
    //        save_tileData.hasBuilding = info.hasBuilding;
    //        save_tileData.buildingComplete = info.buildingComplete;
    //        save_tileData.hasResources = info.hasResources;
    //        save_tileData.progress = info.progress;
    //        save_tileData.buildDone = info.buildDone;
    //        save_tileData.nextProgress = info.nextProgress;

    //        save_tileData.cards = new List<int>();
    //        foreach (Data_Card card in info.cards)
    //            save_tileData.cards.Add(card.code);

    //        save_tileData.enemies = new List<Save_Enemy>();
    //        foreach (GameObject enemy in info.enemies)
    //            save_tileData.enemies.Add(SaveEnemy(enemy));

    //        save_tileData.abyssSetup = info.abyssSetup;
    //        save_tileData.canSpawn = info.canSpawn;
    //        save_tileData.hasAbyss = info.hasAbyss;
    //    }
    //    return save_tileData;
    //}

    public List<Save_Unit> SaveUnitList()
    {
        List<Save_Unit> list = new List<Save_Unit>();

        foreach (Transform child in Units.transform)
        {
            //list.Add(SaveUnit(child.gameObject));
        }

        return list;
    }

    //public Save_Unit SaveUnit(GameObject unitPrefab)
    //{
    //    Save_Unit unit = new Save_Unit();

    //    if (unitPrefab == null)
    //        unit.unitType = -1;

    //    else
    //    {
    //        Data_Unit.Unit_Info info = unitPrefab.GetComponent<Data_Unit>().SaveData();
    //        unit.unitType = convertUnitToType(unitPrefab.gameObject);

    //        unit.nextWander = info.nextWander;
    //        unit.impassable = info.impassable; ;
    //        unit.currentTileOn = SaveTile(info.currentTileOn, false, -1, -1);
    //        unit.reachedTownHall = info.reachedTownHall;
    //        unit.detectedImpassable = SaveTile(info.detectedImpassable, false, -1, -1);
    //        unit.townHall = SaveBuilding(info.townHall);

    //        if (info.workCard != null) unit.card = info.card.code;
    //        else unit.card = -1;

    //        unit.busy = info.busy; ;
    //        unit.toTownHall = info.toTownHall;
    //        unit.cardToDeliver = info.cardToDeliver;
    //        unit.buildingInteraction = info.buildingInteraction;
    //        unit.hurt = info.hurt;
    //        unit.durability = info.durability;
    //        unit.health = info.health;
    //        unit.regenCD = info.regenCD;
    //        unit.nextRegen = info.nextRegen;
    //        if (info.workCard != null) unit.workCard = info.workCard.code;
    //        else unit.workCard = -1;
    //        unit.workPlace = SaveTile(info.workPlace, false, -1, -1);
    //        unit.working = info.working;
    //        unit.workBegun = info.workBegun;
    //        unit.workIndex = info.workIndex;
    //        unit.workTime = info.workTime;
    //        unit.workDone = info.workDone;
    //        unit.workExtra = info.workExtra;
    //        unit.buildPlace = SaveTile(info.buildPlace, false, -1, -1);
    //        unit.building = info.building;
    //        unit.buildBegun = info.buildBegun;
    //        unit.patroling = info.patroling;
    //        unit.patrolPlace = SaveTile(info.patrolPlace, false, -1, -1);
    //        unit.target = SaveTile(info.target, false, -1, -1);
    //        //public Save_Tile spottedAbyss; // Not finalized
    //        unit.spottedEnemy = Save_Enemy(info.spottedEnemy);
    //        unit.nextAttack = info.nextAttack;
    //        unit.workInMemory = info.workInMemory;
    //        unit.rmbTileData = SaveTileData(info.rmbTileData);

    //        if (info.rmbWorkCard != null) unit.rmbWorkCard = info.rmbWorkCard.code;
    //        else unit.rmbWorkCard = -1;

    //        unit.rmbWorkPlace = SaveTile(info.rmbWorkPlace, false, -1, -1);
    //        unit.rmbWorkIndex = info.rmbWorkIndex;
    //        unit.rmbWorkTime = info.rmbWorkTime;
    //        unit.rmbWorkExtra = info.rmbWorkExtra;
    //        unit.tileData = SaveTileData(info.tileData);
    //    }
    //    return unit;
    //}

    public List<Save_Enemy> SaveEnemyList()
    {
        List<Save_Enemy> list = new List<Save_Enemy>();

        foreach (Transform child in Enemies.transform)
        {
            //list.Add(SaveEnemy(child.gameObject.GetComponent<Data_Enemy>(), child.gameObject));
        }

        return list;
    }

    //public Save_Enemy SaveEnemy(GameObject enemyPrefab)
    //{
    //    Save_Enemy enemy = new Save_Enemy();
    //    if (enemyPrefab == null)
    //        enemy.enemyType = -1;
    //    else
    //    {
    //        Data_Enemy.Enemy_Info info = enemyPrefab.GetComponent<Data_Enemy>().SaveData();
    //        enemy.enemyType = convertEnemyToType(enemyPrefab); //Type identifier

    //        enemy.nextWander = info.nextWander;
    //        enemy.impassable = info.impassable;
    //        enemy.currentTileOn = SaveTile(info.currentTileOn, false, -1, -1);
    //        enemy.previousTile = SaveTile(info.previousTile, false, -1, -1);
    //        enemy.dataTile = SaveTileData(info.dataTile.SaveData());
    //        enemy.reachedTown = info.reachedTown;
    //        enemy.reachedAbyss = info.reachedAbyss;
    //        enemy.target = SaveUnit(info.target.GetComponent<Data_Unit>(), info.target);//null?
    //        enemy.spottedUnit = SaveUnit(info.spottedUnit.GetComponent<Data_Unit>(), info.spottedUnit);//null?
    //        enemy.spottedBuilding = SaveBuilding(info.spottedBuilding);
    //        enemy.spottedTownHall = SaveBuilding(info.spottedTownHall);
    //        //public Save_Player spottedPlayer; // Need to serialize
    //        enemy.nextAttack = info.nextAttack;
    //        enemy.abyss = SaveTile(info.abyss.GetComponent<Data_Tile>(), info.abyss);//null?
    //        enemy.abyssData = SaveTileData(info.abyssData.SaveData());
    //        if (info.card != null) enemy.card = info.card.code;
    //        else enemy.card = -1;
    //        enemy.busy = info.busy;
    //        enemy.health = info.health;
    //    }
    //    return enemy;
    //}
    //-------------------------------------------//

    //-------------------------------------------//

    //Convertion Enemy >>> EnemyType
    public int convertEnemyToType(GameObject enemyPrefab)
    {
        Enemy_List.Enemies[] enemyTypes = enemy_list.enemies;

        for (int i = 0; i < enemyTypes.Length; i++)
        {
            if (enemyTypes[i].enemyPrefab.name == enemyPrefab.name)
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
            if (unitTypes[i].unitPrefab.name == unitPrefab.name)
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
        Debug.LogError("Tile not found");
        return null;
    }
    //Convertion Tile >>> TileType
    public int convertTileToType(Data_Tile tile)
    {
        for (int i = 0; i < Map.tileList.Count; i++)
            if (Map.tileList[i].name == tile.tileName)
                return i;
        Debug.LogError("Type not found");
        return -1;
    }
    //Convertion TileType >>> Tile
    public GameObject convertTypeToTile(int type)
    {
        for (int i = 0; i < Map.tileList.Count; i++)
            if (i == type)
                return Map.tileList[i];
        Debug.LogError("Tile not found");
        return null;
    }

    [System.Serializable]
    public class Save_Tile
    {
        public int tileType;

        public Save_TileData save_tileData;

        //Children//
        public Save_Building building;
    }

    [System.Serializable]
    public class Save_TileData
    {
        public bool isNull; //null identifier

        // [Tile Information] //
        public float height;
        public bool gizmoUse;

        // [Tile Resources] //
        public bool canRecharge;
        public bool canRandom;
        public Save_Extra theExtra;
        //public GameObject parentExtra; //Not used, unknown type
        public bool extra;
        public bool check;

        // [Tile Fog]//
        public bool revealed;

        // [Tile Work] //
        public Save_Unit unit;
        public int building;
        public Data_Building.Building_Info dataBuilding;
        public bool hasTownHall;
        public bool hasBuilding;
        public bool buildingComplete;
        public bool hasResources;

        // [Tile Build] //
        public List<Save_Unit> builders;
        public int progress;
        public bool buildDone;
        public bool canProgress;
        public float nextProgress;

        // [Abyss]
        public List<int> cards; // data_card
        public List<Save_Enemy> enemies;
        public bool abyssSetup;
        public bool canSpawn;
        public bool hasAbyss;
    }

    [System.Serializable]
    public class Save_Extra
    {
        public int typeExtra;
    }

    [System.Serializable]
    public class Save_Building
    {
        public int buildingType;

        public Save_BuildingData data;
    }

    [System.Serializable]
    public class Save_BuildingData
    {
        public bool isNull; //null identifier

        public Color transparency;
        public Color originalColor;
        public bool night;
        public bool inProgress;
        public int health;
    }

    [System.Serializable]
    public class Save_Unit
    {
        public int unitType; //Type identifier
        public bool isInvoke;

        public float nextWander;
        public bool impassable;
        public Save_Tile currentTileOn;
        public bool reachedTownHall;
        public Save_Tile detectedImpassable;

        // [Unit Control]
        public Save_Building townHall;
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
        public Save_Tile workPlace;
        public bool working;
        public bool workBegun;
        public int workIndex;
        public float workTime;
        public float workDone;
        public bool workExtra;

        // [Build Routine]
        public Save_Tile buildPlace;
        public bool building;
        public bool buildBegun;

        // [Patrol Routine]
        public bool patroling;
        public Save_Tile patrolPlace;
        public Save_Tile target;
        public Save_Tile spottedAbyss; // Not finalized
        public Save_Enemy spottedEnemy;
        public float nextAttack;

        // [Work in memory]
        public bool workInMemory;
        public Save_TileData rmbTileData;
        public int rmbWorkCard; // data_card
        public Save_Tile rmbWorkPlace;
        public int rmbWorkIndex;
        public float rmbWorkTime;
        public bool rmbWorkExtra;

        // [Recieved from PlayerControl]
        public Save_TileData tileData;
    }

    [System.Serializable]
    public class Save_Enemy
    {
        public int enemyType; //Type identifier
        public bool isInvoke;

        public float nextWander;
        public bool impassable;
        public Save_Tile currentTileOn;
        public Save_Tile previousTile;
        public Save_TileData dataTile;

        // [Enemy Chase]
        public bool reachedTown;
        public bool reachedAbyss;
        public Save_Unit target;
        public Save_Unit spottedUnit;
        public Save_Building spottedBuilding;
        public Save_Building spottedTownHall;
        public Save_Player spottedPlayer; // Need to serialize
        public float nextAttack;

        // [Enemy Control]
        public Save_Tile abyss;
        public Save_TileData abyssData;
        public int card; // data_card
        public bool busy;

        // [Enemy Health]
        public int health;
    }

    [System.Serializable]
    public class Save_Player
    {

    }
}
