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

    public Game_Master GameMaster;
    GameObject GameMap; //The map of the game
    Vector3 TownPos; //The position of the town hall
    private bool prevState = false; //previous day state
    private bool curState = false; //current day state
    [HideInInspector] public int DayCount; //still counting the days
    [HideInInspector] public int UnitTotal = 3; //the current amount of units - (need to be changed by unit list)
    [HideInInspector] public int BuildingCapacity; //the capacity of all the buildings on the map
    public int MaxPeasentSpawn; //the maximum spawning of peasents in one day

    //Enemy spawn constants
    public bool EnemySpawn; //spawning activation
    private int reAbyss = 7; //the power of repetition, determining the recurrence of the spawn
    private int numAbyss = 1; //abyss's per spawn 
    private int firstSpawn; //

   
    [System.Serializable]
    public struct ResourceType //Resource struct
    {
        public int ID; //Abyss ID: 0
        public string name; //resource name
        public GameObject prefab; //resource prefab
        public float maxHeight; //the maximum height of the tile
        public float minHeight; //the minimum height of the tile
        public bool isDistance; //is the location need to have distance from the town hall
    }
    public ResourceType[] resources; //An array of the possible resources

    private void Awake()
    {
        DayCount = 1;

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
                if (DayCount != 1) SpawnNewPeasents(); //spawn new villagers
            }
            else//Night time
            {
                Debug.Log(string.Format("Night {0}", DayCount));
                if (EnemySpawn) 
                {
                    if (DayCount == 2)
                    {
                        firstSpawn = GameMaster.difficulty * 2 + 1;
                        SpawnResources(0, firstSpawn); //spawn firstSpawn Abysses at random places on the map
                        Debug.Log(firstSpawn);
                    }
                    else if (DayCount > 2 && (DayCount + reAbyss - 2) % reAbyss == 0)
                        SpawnResources(0, numAbyss); //spawn Abyss at random place on the map
                }
                DayCount++;
            }
        }
    }

    //Create n random tiles of the chosen resource
    private void SpawnResources(int index, int n)
    {
        Dictionary<int, Vector2Int> PosDic;
        float min = resources[index].minHeight, max = resources[index].maxHeight;
        PosDic = Map.FindPosByRange(min, max, resources[index].isDistance);

        for (int i = 0; i < n; i++)
        {
            if (PosDic.Count != 0)
            {
                int randPos = Random.Range(0, PosDic.Count); //get a random index of the dictionary
                int x = PosDic[randPos].x; //get the x value in the index
                int y = PosDic[randPos].y; //get the y value in the index

                //Update the dictionary
                if (randPos != PosDic.Count - 1)
                {
                    PosDic[randPos] = PosDic[PosDic.Count - 1];
                    PosDic.Remove(PosDic.Count - 1);
                }
                else
                    PosDic.Remove(randPos);

                float currentHeight = Map.TileArray[x, y].GetComponent<Data_Tile>().height; //get the height from the old tile
                GameObject fog = Map.TileArray[x, y].transform.Find("Fog(Clone)").gameObject; //get the fog from the old tile

                GameObject.Destroy(Map.TileArray[x, y]); //destroy the old tile
                Map.TileArray[x, y] = Map.InstantiateTile(resources[index].prefab, new Vector3(x * 10, 1, y * 10), Map, currentHeight);
                fog.transform.parent = Map.TileArray[x, y].transform;
                Debug.Log("Spawned " + resources[index].name + " at: " + Map.TileArray[x, y]);
            }
            else
            {
                Debug.LogError("No empty positions available");
                break;
            }
        }
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
                Map.RandomPeasents(PosList, peasentsCount, false);
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
        for(int i = 0; i < resources.Length; i++)
        {
            if (resources[i].maxHeight < 0)
                resources[i].maxHeight = 0;
            else if (resources[i].maxHeight > 1)
                resources[i].maxHeight = 1;

            if (resources[i].minHeight < 0)
                resources[i].minHeight = 0;
            else if (resources[i].minHeight > 1)
                resources[i].minHeight = 1;

            if (resources[i].maxHeight < resources[i].minHeight)
                resources[i].minHeight = resources[i].maxHeight - 0.1f;
        }
    } 
}
