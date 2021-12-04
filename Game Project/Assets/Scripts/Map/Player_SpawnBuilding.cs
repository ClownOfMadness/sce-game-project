using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Player_SpawnBuilding : MonoBehaviour
{
    public AstarPath path;
    private Vector3 buildingPosition = new Vector3(0, 1, 0);
    //Create a building on the map.
    //public void Spawn(Vector3 SpawnPoint, string ID, GameObject Tile)
    //{
    //    BuildingDataBase buildings = ScriptableObject.CreateInstance<BuildingDataBase>();   //open BuildingDataBase connection to use its functions
    //    Building BuildingType = FindBuilding(ID, buildings);
    //    GameObject NewBuilding = Instantiate(BuildingType.Prefab, SpawnPoint, Quaternion.Euler(0, 180, 0));
    //    //FogOfWar Fog = FindObjectOfType<FogOfWar>();

    //    NewBuilding.name = BuildingType.BuildingName;
    //    NewBuilding.transform.parent = Tile.transform;
    //}

    private void Awake()
    {
        if (!(path = GameObject.Find("Pathfinder Grid").GetComponent<AstarPath>()))
        {
            Debug.LogError("Pathfinder Grid gameobject is not found for the SpawnBuilding script");
        }
    }

    public void Spawn(GameObject building, GameObject Tile)
    {
        
        GameObject NewBuilding = Instantiate(building, Tile.transform.position + buildingPosition, Quaternion.Euler(0, 180, 0));
        NewBuilding.name = building.transform.name;
        NewBuilding.transform.parent = Tile.transform;
        
        path.Scan();
        //Debug.Log("Building placed in " + Tile);
    }

    //Find the building in the buildings database.
    //public static Building FindBuilding(string ID, BuildingDataBase DB)
    //{
    //    foreach (Building building in DB.BuildingList)
    //    {
    //        if (building.BuildingID == ID)
    //            return building;
    //    }
    //    return null;
    //}
}
