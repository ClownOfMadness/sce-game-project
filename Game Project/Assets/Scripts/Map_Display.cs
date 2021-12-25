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

    Map_Gen Map;
    int size;


    //Save System//
    public Save_Tile tile;

    private void Awake()
    {
        if (!(unitlist = GameObject.Find("Units").GetComponent<Unit_List>()))
        {
            Debug.LogError("Units gameobject or Unit_List is not found for Map_Display");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Map = FindObjectOfType<Map_Gen>();
        size = Map.mapSize;
        TownHall = Map.generateMap();
        MainCamera.transform.position = TownHall.transform.position + new Vector3(0, 150, 0);
        unitlist.townhall = TownHall;
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
}
