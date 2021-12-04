using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Map_Display : MonoBehaviour
{
    public Camera MainCamera;
    public AstarPath path;
    public GameObject TownHall;

    // Start is called before the first frame update
    void Start()
    {
        MapGen Map = FindObjectOfType<MapGen>();
        int size = Map.mapSize;
        //FogOfWar Fog = FindObjectOfType<FogOfWar>();
        //Fog.Createfog(size);
        //MainCamera.transform.position = Map.generateMap();
        TownHall = Map.generateMap();
        MainCamera.transform.position = TownHall.transform.position + new Vector3(0,150,0);
        path.Scan();
    }

    private void Update()
    {
        
    }
}
