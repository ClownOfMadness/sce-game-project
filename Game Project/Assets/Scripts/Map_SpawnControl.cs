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
    int UnitMax;//the maximum amount of units (dependent on the placed buildings on the map)

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
            if(curState == true)
            {
                //Map.RandomPeasents();
                Debug.Log(string.Format("Day {0}", DayCount));
                Debug.Log("Spawn new peasents");
            }
            //Night time
            else
            {
                Debug.Log(string.Format("Night {0}", DayCount));
                Debug.Log("Spawn new terrors");
                DayCount++;
            }
        }
    }

    //private static List<Vector3> PeasentPosList()
    //{
    //    List<Vector3> PosList = new List<Vector3>();

    //}

    private void SpawnNewPeasents()
    {
        //int peasentCount;

        if (UnitTotal < UnitMax)
        {
            //peasentCount = CaluculateNewPeasents();
        }
        else
            Debug.Log("The kingdom is full");
    }

    //private int CaluculateNewPeasents()
    //{

    //}
}
