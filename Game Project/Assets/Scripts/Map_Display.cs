using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Map_Display : MonoBehaviour
{
    public GameObject GameMap;
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
        //Screen_Cards.Cards_Info cards_info = new Screen_Cards.Cards_Info();


        //cards_info.Hand = data.Hand;
        //cards_info.Storage = data.Storage;
        //cards_info.Discovered = data.Discovered;
        //cards_info.Combos = data.Combos;
        //cards_info.DisStatus = data.DisStatus;
        //screen_cards.ImportCards(cards_info);
    }

    //public Data_Player Save_Data()
    //{
    //    Data_Player data = new Data_Player();
    //    Screen_Cards.Cards_Info cards_info;

    //    //data.TileMap = SaveMap();
    //    //data.units = SaveUnits();
    //    //data.enemies = SaveEnemies();

    //    cards_info = screen_cards.ExportCards();
    //    data.Hand = cards_info.Hand;
    //    data.Storage = cards_info.Storage;
    //    data.Discovered = cards_info.Discovered;
    //    data.Combos = cards_info.Combos;
    //    data.DisStatus = cards_info.DisStatus;

    //    return data;
    //}

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
    //public Save_Building SaveBuilding(GameObject building)
    //{
    //    Save_Building save_building = new Save_Building();
    //    Data_Building data_building = building.GetComponent<Data_Building>();

    //    save_building.buildingType = convertBuildingToType(building);
    //    //save_building.parentPosX = ;
    //    //save_building.parentPosY = ;

    //    //save_building.transparency = data_building.transparency;
    //    //save_building.originalColor = data_building.originalColor;
    //    //save_building.night = data_building.night;
    //    //save_building.inProgress = data_building.inProgress;
    //    //save_building.health = data_building.health;

    //    return save_building;
    //}

    //Create serializable tile.
    //public Save_Tile SaveTile(Data_Tile data_tile, GameObject tile)
    //{
    //    Save_Tile save_tile = new Save_Tile();
    //    Data_Tile.Tile_Info info = data_tile.CreateTile_Info();

    //    save_tile.posX = (int)tile.transform.position.x / 10;
    //    save_tile.posY = (int)tile.transform.position.z / 10;
    //    save_tile.tileType = convertTileToType(data_tile);

    //    //save_tile.height = data_tile.height;
    //    //save_tile.gizmoUse = info.gizmoUse;
    //    //save_tile.canRecharge = info.canRecharge;
    //    //save_tile.canRandom = info.canRandom;
    //    ////theExtra = info.theExtra; //GO
    //    ////parentExtra = data_tile.parentExtra; //GO
    //    //save_tile.extra = info.extra;
    //    //save_tile.check = info.check;
    //    //save_tile.revealed = data_tile.revealed;
    //    ////unit = info.unit; //GO
    //    //save_tile.building = convertBuildingToType(data_tile.building);
    //    ////dataBuilding = info.dataBuilding; //Class
    //    ////builders = info.builders; //list<Class>
    //    //save_tile.hasTownHall = data_tile.hasTownHall;
    //    //save_tile.hasBuilding = data_tile.hasBuilding;
    //    //save_tile.buildingComplete = data_tile.buildingComplete;
    //    //save_tile.hasResources = data_tile.hasResources;
    //    //save_tile.progress = info.progress;
    //    //save_tile.buildDone = info.buildDone;
    //    //save_tile.canProgress = info.canProgress;
    //    //save_tile.nextProgress = info.nextProgress;
    //    ////cards = data_tile.cards; //List<Class>
    //    ////enemies = info.enemies; //List<Class>
    //    //save_tile.abyssSetup = info.abyssSetup;
    //    //save_tile.canSpawn = info.canSpawn;
    //    //save_tile.hasAbyss = info.hasAbyss;

    //    return save_tile;
    //}

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

}
