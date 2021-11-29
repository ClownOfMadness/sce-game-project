using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MapDisplay : MonoBehaviour
{
    public Camera MainCamera;
    public AstarPath path;

    // Start is called before the first frame update
    void Start()
    {
        MapGen Map = FindObjectOfType<MapGen>();
        int size = Map.mapSize;
        FogOfWar Fog = FindObjectOfType<FogOfWar>();
        Fog.Createfog(size);
        MainCamera.transform.position = Map.generateMap();
        path.Scan();
    }

    private void Update()
    {
        
    }
}
