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
        //save_manager = FindObjectOfType<Save_Manager>();
        //save_manager.SceneLoaded();
        
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
        //else Load_Data(saved_data);
        path.Scan();

        //Debug.Log(Save_Manager.slotName);
        //Debug.Log(Save_Manager.currSlot);
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

    public void Load_Data(Data_Player data)
    {
        Screen_Cards.Cards_Info cards_info = new Screen_Cards.Cards_Info();


        cards_info.Hand = data.Hand;
        cards_info.Storage = data.Storage;
        cards_info.Discovered = data.Discovered;
        cards_info.Combos = data.Combos;
        cards_info.DisStatus = data.DisStatus;
        screen_cards.ImportCards(cards_info);
    }

    public Data_Player Save_Data()
    {
        Data_Player data = new Data_Player();
        Screen_Cards.Cards_Info cards_info;

        //data.TileMap = SaveMap();
        //data.units = SaveUnits();
        //data.enemies = SaveEnemies();

        cards_info = screen_cards.ExportCards();
        data.Hand = cards_info.Hand;
        data.Storage = cards_info.Storage;
        data.Discovered = cards_info.Discovered;
        data.Combos = cards_info.Combos;
        data.DisStatus = cards_info.DisStatus;

        return data;
    }

    //Create serializable tile map.
    //public Save_Tile[,] SaveMap()
    //{
    //    Save_Tile[,] save_map = new Save_Tile[size, size];

    //    for (int y = 0; y < size; y++)
    //        for (int x = 0; x < size; x++)
    //            SaveTile(Map.DataTileArray[x, y], Map.TileArray[x, y]);

    //    return save_map;
    //}

    //Create serializable building.
    public Save_Building SaveBuilding(GameObject building)
    {
        Save_Building save_building = new Save_Building();
        Data_Building.Building_Info info = new Data_Building.Building_Info();

        save_building.buildingType = convertBuildingToType(building);
        //save_building.parentPosX = ;
        //save_building.parentPosY = ;

        save_building.transparency = info.transparency;
        save_building.originalColor = info.originalColor;
        save_building.night = info.night;
        save_building.inProgress = info.inProgress;
        save_building.health = info.health;

        return save_building;
    }

    //Create serializable tile.
    public Save_Tile SaveTile(Data_Tile data_tile, GameObject tile)
    {
        Save_Tile save_tile = new Save_Tile();
        Data_Tile.Tile_Info info = data_tile.SaveData();

        save_tile.posX = (int)tile.transform.position.x / 10;
        save_tile.posY = (int)tile.transform.position.z / 10;
        save_tile.tileType = convertTileToType(data_tile);

        save_tile.save_tileData = SaveTileData(info);

        return save_tile;
    }

    public Save_TileData SaveTileData(Data_Tile.Tile_Info info)
    {
        Save_TileData save_tileData = new Save_TileData();

        save_tileData.height = info.height;
        save_tileData.gizmoUse = info.gizmoUse;
        save_tileData.canRecharge = info.canRecharge;
        save_tileData.canRandom = info.canRandom;
        //theExtra = info.theExtra; //GO
        //parentExtra = info.parentExtra; //GO
        save_tileData.extra = info.extra;
        save_tileData.check = info.check;
        save_tileData.revealed = info.revealed;
        //unit = info.unit; //GO
        save_tileData.building = convertBuildingToType(info.building);
        //dataBuilding = info.dataBuilding; //Class
        //builders = info.builders; //list<Class>
        save_tileData.hasTownHall = info.hasTownHall;
        save_tileData.hasBuilding = info.hasBuilding;
        save_tileData.buildingComplete = info.buildingComplete;
        save_tileData.hasResources = info.hasResources;
        save_tileData.progress = info.progress;
        save_tileData.buildDone = info.buildDone;
        save_tileData.nextProgress = info.nextProgress;
        //save_tileData.cards = info.cards; //List<Class>
        //save_tileData.enemies = info.enemies; //List<Class>
        save_tileData.abyssSetup = info.abyssSetup;
        save_tileData.canSpawn = info.canSpawn;
        save_tileData.hasAbyss = info.hasAbyss;

        return save_tileData;
    }

    public List<Save_Unit> CreateUnitList()
    {
        List<Save_Unit> list = new List<Save_Unit>();
        
        foreach (Transform child in Units.transform)
        {
            list.Add(CreateUnit(child.gameObject.GetComponent<Data_Unit>(), child.gameObject));
        }

        return list;
    }

    public Save_Unit CreateUnit(Data_Unit data_unit, GameObject unitPrefab)
    {
        Save_Unit unit = new Save_Unit();
        Data_Unit.Unit_Info info = data_unit.SaveData();

        if(unitPrefab != null)
        {
            unit.unitType = convertUnitToType(unitPrefab.gameObject);

            unit.nextWander = info.nextWander;
            unit.impassable = info.impassable; ;
            unit.currentTileOn = SaveTile(info.currentTileOn.GetComponent<Data_Tile>(), info.currentTileOn);//null?
            unit.reachedTownHall = info.reachedTownHall; ;
            unit.detectedImpassable = SaveTile(info.detectedImpassable.GetComponent<Data_Tile>(), info.detectedImpassable);//null?;
            unit.townHall = SaveBuilding(info.townHall);
            //public Data_Card card; // Need to serialize
            unit.busy = info.busy; ;
            unit.toTownHall = info.toTownHall;
            unit.cardToDeliver = info.cardToDeliver;
            unit.buildingInteraction = info.buildingInteraction;
            unit.hurt = info.hurt;
            unit.durability = info.durability;
            unit.health = info.health;
            unit.regenCD = info.regenCD;
            unit.nextRegen = info.nextRegen;
            //unit.workCard;
            unit.workPlace = SaveTile(info.workPlace.GetComponent<Data_Tile>(), info.workPlace);//null?
            unit.working = info.working;
            unit.workBegun = info.workBegun;
            unit.workIndex = info.workIndex;
            unit.workTime = info.workTime;
            unit.workDone = info.workDone;
            unit.workExtra = info.workExtra;
            unit.buildPlace = SaveTile(info.buildPlace.GetComponent<Data_Tile>(), info.buildPlace);//null?
            unit.building = info.building;
            unit.buildBegun = info.buildBegun;
            unit.patroling = info.patroling;
            unit.patrolPlace = SaveTile(info.patrolPlace.GetComponent<Data_Tile>(), info.patrolPlace);//null?;
            unit.target = SaveTile(info.target.GetComponent<Data_Tile>(), info.target);//null?;;
            //public Save_Tile spottedAbyss; // Not finalized
            //unit.spottedEnemy = Save_Enemy();
            unit.nextAttack = info.nextAttack;
            unit.workInMemory = info.workInMemory;
            unit.rmbTileData = SaveTileData(info.rmbTileData.SaveData());
            //public Data_Card rmbWorkCard; // Need to serialize
            unit.rmbWorkPlace = SaveTile(info.rmbWorkPlace.GetComponent<Data_Tile>(), info.rmbWorkPlace);//null?;;;
            unit.rmbWorkIndex = info.rmbWorkIndex;
            unit.rmbWorkTime = info.rmbWorkTime;
            unit.rmbWorkExtra = info.rmbWorkExtra;
            unit.tileData = SaveTileData(info.tileData.SaveData());
        }
        return unit;
    }
    
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
    //Convertion UnitType >>> Unit
    public Enemy_List.Enemies convertTypeToEnemy(int type)
    {
        Enemy_List.Enemies[] enemyTypes = enemy_list.enemies;
        return enemyTypes[type];
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
    //Convertion UnitType >>> Unit
    public Unit_List.Units convertTypeToUnit(int type)
    {
        Unit_List.Units[] unitTypes = unit_list.units;
        return unitTypes[type];
    }
    //Convertion Building >>> BuildingType
    public int convertBuildingToType(GameObject building)
    {
        for (int i = 0; i < SpawnBuilding.BuildingList.Count; i++)
            if (SpawnBuilding.BuildingList[i].name == building.name)
                return i;
        Debug.LogError("Type not found");
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

    public class Save_Tile
    {
        public int posX;
        public int posY;

        public int tileType;
        public Save_TileData save_tileData;

        //Children//
        public Save_Building building;
    }

    public class Save_TileData
    {
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
        //public List<Save_Unit> builders;
        public int progress;
        public bool buildDone;
        public bool canProgress;
        public float nextProgress;

        // [Abyss]
        //public List<Data_Card> cards; //List<Data_Card> cards???
        public List<Save_Enemy> enemies;
        public bool abyssSetup;
        public bool canSpawn;
        public bool hasAbyss;
    }

    public class Save_Extra
    {
        public int typeExtra;
    }


    public class Save_Building
    {
        public int buildingType;
        public int parentPosX;
        public int parentPosY;

        public Color transparency;
        public Color originalColor;
        public bool night;
        public bool inProgress;
        public int health;
    }

    //public class Save_BuildingData //Useless?
    //{
    //    public Color transparency;
    //    public Color originalColor;
    //    public bool night;
    //    public bool inProgress;
    //    public int health;
    //}

    public class Save_Unit
    {
        public int unitType; //Type identifier

        public float nextWander;
        public bool impassable;
        public Save_Tile currentTileOn;
        public bool reachedTownHall;
        public Save_Tile detectedImpassable;

        // [Unit Control]
        public Save_Building townHall;
        //public Data_Card card; // Need to serialize
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
        //public Data_Card workCard; // Need to serialize
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
        //public Data_Card rmbWorkCard; // Need to serialize
        public Save_Tile rmbWorkPlace;
        public int rmbWorkIndex;
        public float rmbWorkTime;
        public bool rmbWorkExtra;

        // [Recieved from PlayerControl]
        public Save_TileData tileData; // Tile Data that is recieved from the player
    }

    public class Save_Enemy
    {
        public int enemyType; //Type identifier

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
        //public Data_Card card; // Need to serialize
        public bool busy;

        // [Enemy Health]
        public int health;
    }

    public class Save_Player
    {

    }
}
