using UnityEngine;

public class SpawnBuilding : MonoBehaviour
{

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

    public void Spawn(GameObject building, GameObject Tile)
    {
        //BuildingDataBase buildings = ScriptableObject.CreateInstance<BuildingDataBase>();   //open BuildingDataBase connection to use its functions
        //Building BuildingType = FindBuilding(ID, buildings);
        GameObject NewBuilding = Instantiate(building, Tile.transform.position ,Quaternion.Euler(0, 180, 0));
        //FogOfWar Fog = FindObjectOfType<FogOfWar>();

        NewBuilding.name = building.transform.name;
        NewBuilding.transform.parent = Tile.transform;
    }

    //Find the building in the buildings database.
    public static Building FindBuilding(string ID, BuildingDataBase DB)
    {
        foreach (Building building in DB.BuildingList)
        {
            if (building.BuildingID == ID)
                return building;
        }
        return null;
    }
}
