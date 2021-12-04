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
        Map_Gen Map = FindObjectOfType<Map_Gen>();
        int size = Map.mapSize;
        TownHall = Map.generateMap();
        MainCamera.transform.position = TownHall.transform.position + new Vector3(0, 150, 0);
        path.Scan();
    }

    private void Update()
    {
        
    }
}
