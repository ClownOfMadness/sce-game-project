using UnityEngine;

public class SpawnBuilding : MonoBehaviour
{
    public GameObject GameMap;
    public GameObject TileBuilding;
    public BuildingDataBase buildings;
    private static SpawnBuilding instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Spawn(Vector3 SpawnPoint, string ID)
    {
        GameObject NewBuilding = new GameObject();
        Building BuildingType = FindBuilding(ID);
        NewBuilding = Instantiate(TileBuilding, SpawnPoint, Quaternion.identity);
        NewBuilding.transform.parent = GameMap.transform;
    }

    public static Building FindBuilding(string ID)
    {
       foreach(Building building in instance.buildings.BuildingList)
        {
            if (building.BuildingID == ID)
                return building;
        }
        return null;
    }
}
