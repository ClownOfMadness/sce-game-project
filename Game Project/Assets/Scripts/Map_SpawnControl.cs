using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Map_SpawnControl : MonoBehaviour
{
    //Used objects
    System_DayNight DayCicle;
    Map_Gen Map;
    Map_Display display;

    GameObject GameMap;//The map of the game
    Vector3 TownPos;//The position of the town hall
    bool prevState = false;//previous day state
    bool curState = false;//current day state
    [HideInInspector] public int DayCount = 1;//still counting the days
    [HideInInspector] public int UnitTotal = 3;//the current amount of units
    [HideInInspector] public int BuildingCapacity;//the capacity of all the buildings on the map
    public int MaxPeasentSpawn;//the maximum spawning of peasents in one day

    [System.Serializable]
    public struct ResourceType
    {
        public int ID;
        public string name;
        public GameObject prefab;
        public float maxHeight;
        public float minHeight;
    }
    //Abyss ID:0
    public ResourceType[] resources;

    private void Awake()
    {
        DayCicle = FindObjectOfType<System_DayNight>();
        Map = FindObjectOfType<Map_Gen>();
        display = FindObjectOfType<Map_Display>();
    }


    // Update is called once per frame
    void Update()
    {
        //Changed from day to night or vice versa
        if (DayCicle.isDay != curState)
        {
            curState = DayCicle.isDay;
            prevState = curState;
            
            if (curState == true)//Day time
            {
                Debug.Log(string.Format("Day {0}", DayCount));
                if (DayCount != 0) SpawnNewPeasents();
                //Debug.Log("UnitTotal: " + UnitTotal);
                //Debug.Log("BuildingCapacity: " + BuildingCapacity);
                //Debug.Log("MaxPeasentSpawn: " + MaxPeasentSpawn);
            }
            else//Night time
            {
                Debug.Log(string.Format("Night {0}", DayCount));
                Debug.Log("Spawning new terrors");
                if (DayCount != 0) SpawnResources(0);
                DayCount++;
            }
        }
    }

    //[Temporary?] create 1 of the chosen resource
    private void SpawnResources(int n)
    {
        Dictionary<int, Vector2Int> PosDic;
        float min = resources[n].minHeight, max = resources[n].maxHeight;
        PosDic = Map.FindPosByRange(min, max);
        if (PosDic.Count != 0) 
        {
            int randPos = Random.Range(0, PosDic.Count);
            int x = PosDic[randPos].x;
            int y = PosDic[randPos].y;

            float currentHeight = Map.TileArray[x, y].GetComponent<Data_Tile>().height;
            GameObject.Destroy(Map.TileArray[x, y]);
            Map.TileArray[x, y] = Instantiate(resources[n].prefab, new Vector3(x * 10, 1, y * 10), Quaternion.Euler(0, 0, 0));
            Map.TileArray[x, y].transform.parent = Map.transform;
            Map.TileArray[x, y].name = string.Format("tile_x{0}_y{1}", x, y);
            Map.TileArray[x, y].GetComponent<Data_Tile>().height = currentHeight;
            Debug.Log("Spawned Abyss at: " + Map.TileArray[x, y]);
        }
        else Debug.LogError("List is empty");
    }


    //Spawn new peasents around the town hall.
    private void SpawnNewPeasents()
    {
        List<Vector3> PosList;
        int peasentsCount;

        if (UnitTotal < BuildingCapacity)
        {
            peasentsCount = CaluculateNewPeasents();
            if (peasentsCount != 0)
            {
                PosList = PeasentPosList();
                Map.RandomPeasents(PosList, peasentsCount);
                UnitTotal += peasentsCount;
            }
        }
        else
            Debug.Log("The kingdom is full");
    }

    //Calculate the number of new peasents that will be spawned.
    private int CaluculateNewPeasents()
    {
        int emptyPlaces = BuildingCapacity - UnitTotal;

        if (emptyPlaces <= MaxPeasentSpawn)
            return emptyPlaces;
        return MaxPeasentSpawn;
    }

    //Create list of all the positions surrounding the town hall.
    private List<Vector3> PeasentPosList()
    {
        List<Vector3> PosList = new List<Vector3>();
        Vector3 pos = display.TownHall.transform.position;

        int x = (int)pos.x / 10, y = (int)pos.z / 10;

        for (int i = y - 1; i <= y + 1; i++)
        {
            for (int j = x - 1; j <= x + 1; j++)
            {
                if (!(i == y && j == x))
                    PosList.Add(new Vector3(j * 10, 1, i * 10));
            }
        }
        return PosList;
    }

    //Limits editing to valid values.
    private void OnValidate()
    {
        if (MaxPeasentSpawn < 0)
            MaxPeasentSpawn = 0;
        else if (MaxPeasentSpawn > 8)
            MaxPeasentSpawn = 8;
    } 
}
