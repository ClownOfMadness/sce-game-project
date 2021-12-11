using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Gen : MonoBehaviour
{
    [System.Serializable]
    public struct TerrainType // Terrain struct
    {
        public string name;
        public float height;
        public GameObject tile;
    }

    // Resource rarity - [[Later should be changed to a struct due to many resources]]
    public GameObject Ruins;
    public int ruins;

    // Prefabs
    public GameObject townHall;
    public GameObject fog;

    // Map settings
    public int mapSize;
    public float noiseScale;
    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public Vector2 offset;

    public int safeDistance; //distance from the town hall
    Vector2 hallCo; //town hall coortinates

    // Control the falloff map.
    public int falloffA;
    public float falloffB;

    public TerrainType[] regions;

    // Extra settings
    public bool autoUpdate;
    public bool useFalloff;
    public bool FogMap;

    float[,] falloffMap;
    public GameObject[,] TileArray;

    void Awake()
    {
        falloffMap = Map_FalloffGen.generateFalloffMap(mapSize, falloffA, falloffB);
    }

    //Generating the map.
    public GameObject generateMap()
    {
        int seed = Random.Range(int.MinValue, int.MaxValue);//Seed get a random value.
        int randP2;
        TileArray = new GameObject[mapSize, mapSize];

        //Create a noise map.
        float[,] noiseMap = Map_Noise.noiseMapGen(mapSize, mapSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
        //Create a dictionary of possible possitions for the Town Hall.
        Dictionary<int, Vector2Int> PosiblePos = new Dictionary<int, Vector2Int>();
        GameObject tileType;
        int count = 0;

        //Delete the map if such exist.
        deleteTileMap();

        for (int y = 0; y < mapSize; y++)
        {
            for( int x = 0; x < mapSize; x++)
            {
                if (useFalloff)
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);

                float currentHeight = noiseMap[x, y];

                for(int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height) 
                    {
                        if (CheckValidPos(regions[i]))
                            PosiblePos.Add(count++, new Vector2Int(x, y));

                        randP2 = Random.Range(1, ruins);

                        if (regions[i].name == "P2" && randP2 == 1)
                            tileType = Ruins;
                        else
                            tileType = regions[i].tile;

                        TileArray[x, y] = InstantiateTile(tileType, new Vector3(10 * x, 1, 10 * y), this, currentHeight);

                        if (FogMap)
                        {
                            GameObject thisFog = Instantiate(fog, new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 0), TileArray[x, y].transform);
                            thisFog.transform.localPosition = new Vector3(0, 1, 0);
                        }
                        break;
                    }
                }
            }
        }
        return PlaceStartPos(PosiblePos);
    }

    //Create a dictionary of the possiple spawn tiles in the height range.
    public Dictionary<int, Vector2Int> FindPosByRange(float minHeight, float maxHeight, bool isDistance)
    {
        Dictionary<int, Vector2Int> PosDic = new Dictionary<int, Vector2Int>();
        int count = 0;

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                if (isDistance) //if true eliminate from the list all the tiles in the radius of the town hall.
                    if ((x >= hallCo.x - safeDistance && x <= hallCo.x + safeDistance) && (y >= hallCo.y - safeDistance && y <= hallCo.y + safeDistance))
                        continue;

                float curHeight = TileArray[x, y].GetComponent<Data_Tile>().height;
                if ((curHeight >= minHeight && curHeight <= maxHeight) && (TileArray[x, y].GetComponent<Data_Tile>().tileName == "Plains"))
                    PosDic.Add(count++, new Vector2Int(x, y));
            }
        }
        return PosDic;
    }


    //Check if from this region there is access to the other tiles.
    public bool CheckValidPos(TerrainType tile)
    {
        if (tile.height > 0.60 && tile.height < 0.69)
            return true;
        return false;
    }

    //Choosing randomly the player's starting point and adding the Town Hall to the map.
    public GameObject PlaceStartPos(Dictionary<int, Vector2Int> PosiblePos)
    {
        int size = PosiblePos.Count, k = 0;
        int randNum = Random.Range(0, size - 1);

        int x = PosiblePos[randNum].x;
        int y = PosiblePos[randNum].y;

        while (regions[k].tile.name != "Plains") { k++; }
        List<Vector3> PeasentPos = new List<Vector3>();
        for (int i = y - 1; i <= y + 1; i++)
        {
            for (int j = x - 1; j <= x + 1; j++)
            {
                //Creating an list of the tiles surrounding the town hall.
                if (!(i == y && j == x))
                    PeasentPos.Add(new Vector3(j * 10, 1, i * 10));

                if (TileArray[j, i].GetComponent<Data_Tile>().tileName != "Plains")
                {
                    float currentHeight = TileArray[j, i].GetComponent<Data_Tile>().height;
                    GameObject.DestroyImmediate(TileArray[j, i]);
                    TileArray[j, i] = InstantiateTile(regions[k].tile, new Vector3(j * 10, 1, i * 10), this, currentHeight);

                    if (FogMap)//Create fog if it enabled.
                    {
                        GameObject thisFog = Instantiate(fog, new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 0), TileArray[j, i].transform);
                        thisFog.transform.localPosition = new Vector3(0, 1, 0);
                    }
                }
            }
        }
        Player_SpawnBuilding TownHall = FindObjectOfType<Player_SpawnBuilding>();
        TownHall.Spawn(townHall, TileArray[x, y]); //spawn the town hall
        RandomPeasents(PeasentPos, 3); //spawn 3 random peasents around the town hall
        hallCo = new Vector2(x, y); //save the coordenates of the town hall 

        return TileArray[x, y]; //return the position of the town hall
    }

    //Istantiate single tile by given parameters.
    public GameObject InstantiateTile(GameObject tileType, Vector3 pos, Map_Gen parent, float currentHeight)
    {
        GameObject tile = Instantiate(tileType, pos, Quaternion.Euler(0, 0, 0));
        tile.transform.parent = parent.transform;
        tile.name = string.Format("tile_x{0}_y{1}", pos.x / 10, pos.z / 10);
        tile.GetComponent<Data_Tile>().height = currentHeight;
        return tile;
    }

    //Create random peasents at n random places of the chose positions.
    public void RandomPeasents(List<Vector3> PeasentsPos, int n) 
    {
        Unit_List unitList = FindObjectOfType<Unit_List>();
        int size = PeasentsPos.Count;

        if (n <= size)
        {
            for (int i = 0; i < n; i++)
            {
                int rand = Random.Range(0, size);
                unitList.AddUnit(PeasentsPos[rand], 0);
                PeasentsPos.RemoveAt(rand);
                size--;
            }
        }
        else
            Debug.LogError("Insufficient amount of positions");
    }


    //Delete all the tiles on the map.
    public void deleteTileMap()
    {
        while (this.transform.childCount != 0)
        {
            foreach (Transform child in this.transform)
            {
                GameObject.DestroyImmediate(child.gameObject);
            }
        }
    }

    //Limits editing to valid values.
    private void OnValidate()
    {
        if (mapSize < 1)
            mapSize = 1;
        if (lacunarity < 1)
            lacunarity = 1;
        if (octaves < 0)
            octaves = 0;
        if (falloffA < 1)
            falloffA = 1;
        if (falloffB < 0.1f)
            falloffB = 0.1f;
        if (ruins < 1)
            ruins = 1;
        if (safeDistance < 1)
            safeDistance = 1;

            falloffMap = Map_FalloffGen.generateFalloffMap(mapSize, falloffA, falloffB);
    }
}
