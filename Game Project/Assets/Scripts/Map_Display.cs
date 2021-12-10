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
        Map_Gen Map = FindObjectOfType<Map_Gen>();
        int size = Map.mapSize;
        TownHall = Map.generateMap();
        MainCamera.transform.position = TownHall.transform.position + new Vector3(0, 150, 0);
        unitlist.townhall = TownHall;
        path.Scan();
    }

    private void Update()
    {
        
    }
}
