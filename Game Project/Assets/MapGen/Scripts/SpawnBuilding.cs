using UnityEngine;

public class SpawnBuilding : MonoBehaviour
{
    public BuildingDataBase buildings;

    public void Spawn(Vector3 SpawnPoint, string ID, GameObject Tile)
    {
        Building BuildingType = FindBuilding(ID, buildings);
        GameObject NewBuilding = Instantiate(BuildingType.Prefab, SpawnPoint, Quaternion.Euler(0, 180, 0));
        NewBuilding.name = BuildingType.BuildingName;
        NewBuilding.transform.parent = Tile.transform;
    }

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
