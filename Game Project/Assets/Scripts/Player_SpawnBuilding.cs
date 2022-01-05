using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Player_SpawnBuilding : MonoBehaviour
{
    //--------------------------------------[To-Do List]-----------------------------------------------

    // to add:
    // - option to remove buildings (with getting building card back)
    public BuildingDataBase DB;
    public AstarPath path;
    private Vector3 buildingPosition = new Vector3(0, 1, 0);
    Map_SpawnControl spawnControl;
    public List<GameObject> BuildingList;


    private void Awake()
    {
        spawnControl = FindObjectOfType<Map_SpawnControl>();
        CreateBuildingsList();

        if (!(path = GameObject.Find("Pathfinder Grid").GetComponent<AstarPath>()))
        {
            Debug.LogError("Pathfinder Grid gameobject is not found for the SpawnBuilding script");
        }
    }

    //Create a building on the map.
    public bool Spawn(GameObject building, GameObject Tile)
    {
         
        Data_Building dataBuilding = building.GetComponent<Data_Building>();
        Data_Tile dataTile = Tile.GetComponent<Data_Tile>();

        if (dataTile.hasTownHall)
        {
            return false;
        }
        if ((dataTile.revealed && dataTile.canBuild) || dataBuilding.buildingName == "TownHall")
        {
            if (dataTile.hasBuilding)
            {
                Destroy(dataTile.building);
            }
            GameObject NewBuilding = Instantiate(building, Tile.transform.position + buildingPosition, Quaternion.Euler(0, 0, 0));
            NewBuilding.name = building.transform.name;
            NewBuilding.transform.parent = Tile.transform;
            UpdateCapacity(NewBuilding.name);//add to the buildings total capacity

            if (dataBuilding.buildingName == "TownHall")
            {
                dataTile.PlaceTownHall(NewBuilding);
            }
            else
            {
                dataTile.PlaceBuilding(NewBuilding);
            }
            path.Scan();
            return true;
        }
        return false;
    }

    public bool SpawnReady(GameObject building, GameObject Tile)
    {
        Data_Building dataBuilding = building.GetComponent<Data_Building>();
        Data_Tile dataTile = Tile.GetComponent<Data_Tile>();

        if (dataTile.hasTownHall)
        {
            return false;
        }
        if ((dataTile.revealed && dataTile.canBuild) || dataBuilding.buildingName == "TownHall")
        {
            if (dataTile.hasBuilding)
            {
                Destroy(dataTile.building);
            }
            GameObject NewBuilding = Instantiate(building, Tile.transform.position + buildingPosition, Quaternion.Euler(0, 0, 0));
            NewBuilding.name = building.transform.name;
            NewBuilding.transform.parent = Tile.transform;
            UpdateCapacity(NewBuilding.name);//add to the buildings total capacity

            if (dataBuilding.buildingName == "TownHall")
            {
                dataTile.PlaceTownHall(NewBuilding);
            }
            else
            {
                dataTile.PlaceReadyBuilding(NewBuilding);
            }
            path.Scan();
            return true;
        }
        return false;
    }

    //Create a list of all the prefabs in the buildings folder.
    public List<GameObject> CreateBuildingsList()
    {
        BuildingList = new List<GameObject>();
        object[] files = Resources.LoadAll("Buildings", typeof(GameObject));
        foreach (object file in files)
        {
            BuildingList.Add((GameObject)file);
        }
        return BuildingList;
    }

    //Find the building capacity in the buildings database.
    //private void NewUpdateCapacity(Data_Tile dataTile)
    //{
    //    spawnControl.BuildingCapacity += dataTile.capacity;
    //}


    //Find the building capacity in the buildings database.
    private void UpdateCapacity(string name)
    {

        foreach (Building building in DB.BuildingList)
        {
            if (building.BuildingName == name)
            {
                if (building.isLively)
                    spawnControl.BuildingCapacity += building.capacity;
                break;
            }
        }
    }
}
