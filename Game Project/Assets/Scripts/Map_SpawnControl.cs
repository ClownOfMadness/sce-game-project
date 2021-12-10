using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            //Day time
            if (curState == true)
            {
                //Map.RandomPeasents();
                Debug.Log(string.Format("Day {0}", DayCount));
                Debug.Log("UnitTotal: " + UnitTotal);
                Debug.Log("BuildingCapacity: " + BuildingCapacity);
                Debug.Log("MaxPeasentSpawn: " + MaxPeasentSpawn);
                if(DayCount != 0)
                    SpawnNewPeasents();
            }
            //Night time
            else
            {
                Debug.Log(string.Format("Night {0}", DayCount));
                Debug.Log("Spawning new terrors");
                DayCount++;
            }
        }
    }

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

    private int CaluculateNewPeasents()
    {
        int emptyPlaces = BuildingCapacity - UnitTotal;

        if (emptyPlaces <= MaxPeasentSpawn)
            return emptyPlaces;
        return MaxPeasentSpawn;
    }

    private List<Vector3> PeasentPosList()
    {
        List<Vector3> PosList = new List<Vector3>();
        Vector3 pos = display.TownHall.transform.position;

        int x = (int)pos.x / 10, y = (int)pos.z / 10;

        for (int i = y - 1; i <= y + 1; i++)
        {
            for (int j = x - 1; j <= x + 1; j++)
            {
                //Creating an list of the tiles surrounding the town hall.
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
