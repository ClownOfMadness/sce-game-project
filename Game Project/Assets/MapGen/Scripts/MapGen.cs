using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public GameObject tile;
    }
    public GameObject Abyss;
    public int abyss;
    public GameObject Ruins;
    public int ruins;

    public int mapSize;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    //Control the falloff map.
    public int falloffA;
    public float falloffB;

    public Vector2 offset;

    public TerrainType[] regions;

    public bool autoUpdate;
    public bool useFalloff;


    float[,] falloffMap;
    GameObject[,] TileArray;

    void Awake()
    {
        falloffMap = FalloffGen.generateFalloffMap(mapSize, falloffA, falloffB);
    }

    //Generating the map.
    public Vector3 generateMap()
    {
        int seed = Random.Range(int.MinValue, int.MaxValue);//Seed get a random value.
        int randP1, randP2;
        TileArray = new GameObject[mapSize, mapSize];

        //Create a noise map.
        float[,] noiseMap = Noise.noiseMapGen(mapSize, mapSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
        //Create a dictionary of possible possitions for the Town Hall.
        Dictionary<int, Vector2Int> PosiblePos = new Dictionary<int, Vector2Int>();
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
                        if (regions[i].height >= 0.3 && regions[i].height <= 0.55)
                            PosiblePos.Add(count++, new Vector2Int(x, y));

                        randP1 = Random.Range(1, abyss);
                        randP2 = Random.Range(1, ruins);

                        if (regions[i].name == "P1" && randP1 == 1)
                            TileArray[x, y] = Instantiate(Abyss, new Vector3(10 * x, 1, 10 * y), Quaternion.Euler(0, 180, 0));
                        else if (regions[i].name == "P2" && randP2 == 1)
                            TileArray[x, y] = Instantiate(Ruins, new Vector3(10 * x, 1, 10 * y), Quaternion.Euler(0, 180, 0));
                        else
                            TileArray[x, y] = Instantiate(regions[i].tile, new Vector3(10 * x, 1, 10 * y), Quaternion.Euler(0, 180, 0));

                        TileArray[x, y].transform.parent = this.transform;
                        TileArray[x, y].name = string.Format("tile_x{0}_y{1}", x, y);
                        break;
                    }
                }
            }
        }
        return PlaceStartPos(PosiblePos);
    }

    //Choosing randomly the player's starting point and adding the Town Hall to the map.
    public Vector3 PlaceStartPos(Dictionary<int, Vector2Int> PosiblePos)
    {
        int size = PosiblePos.Count, k = 0;
        int randNum = Random.Range(0, size - 1);

        int x = PosiblePos[randNum].x;
        int y = PosiblePos[randNum].y;

        while (regions[k].name != "P1") { k++; }
        
        for (int i = y - 1; i <= y + 1; i++)
        {
            for (int j = x - 1; j <= x + 1; j++)
            {
                TileArray[x, y] = GameObject.Find(string.Format("tile_x{0}_y{1}", j, i));
                if (TileArray[x, y].name != "P1")
                { 
                    Vector3 pos = TileArray[x, y].transform.position;
                    GameObject.DestroyImmediate(TileArray[x, y]);
                    TileArray[x, y] = Instantiate(regions[k].tile, new Vector3(j * 10, 1, i * 10), Quaternion.Euler(0, 180, 0));
                    TileArray[x, y].transform.parent = this.transform;
                    TileArray[x, y].name = string.Format("tile_x{0}_y{1}", j, i);
                }
            }
        }
        SpawnBuilding TownHall = FindObjectOfType<SpawnBuilding>();
        TownHall.Spawn(new Vector3(x * 10 ,2, y * 10), "BU0001", TileArray[x,y]);

        return new Vector3(x * 10, 150, y * 10);
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
        if (abyss < 1)
            abyss = 1;
        if (ruins < 1)
            ruins = 1;

        falloffMap = FalloffGen.generateFalloffMap(mapSize, falloffA, falloffB);
    }
}
