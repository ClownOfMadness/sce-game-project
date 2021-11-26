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
    public GameObject temp1;//very very temporary

    public int mapSize;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int falloffA;
    public float falloffB;

    public Vector2 offset;

    public TerrainType[] regions;

    public bool autoUpdate;
    public bool useFalloff;


    float[,] falloffMap;

    void Awake()
    {
        falloffMap = FalloffGen.generateFalloffMap(mapSize, falloffA, falloffB);
    }

    public Vector3 generateMap()
    {
        int seed = Random.Range(int.MinValue, int.MaxValue);
        float[,] noiseMap = Noise.noiseMapGen(mapSize, mapSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
        Dictionary<int, Vector2Int> PosiblePos = new Dictionary<int, Vector2Int>();
        int count = 0;

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

                        GameObject tile = Instantiate(regions[i].tile);
                        tile.transform.position = new Vector3(10 * x , 1, 10 * y);
                        tile.transform.rotation = Quaternion.Euler(0, 180, 0);
                        tile.transform.parent = this.transform;
                        tile.name = string.Format("tile_x{0}_y{1}", x, y);
                        break;
                    }
                }
            }
        }
        return PlaceStartPos(PosiblePos);
    }

    public Vector3 PlaceStartPos(Dictionary<int, Vector2Int> PosiblePos)
    {
        int size = PosiblePos.Count;
        int randNum = Random.Range(0, size - 1);
        int k = 0;

        int x = PosiblePos[randNum].x;
        int y = PosiblePos[randNum].y;

        while (regions[k].name != "P1")
        {
            k++;
        }

        for (int i = y - 1; i <= y + 1; i++)
        {
            for (int j = x - 1; j <= x + 1; j++)
            {
                GameObject tile;
                        
                tile = GameObject.Find(string.Format("tile_x{0}_y{1}", j, i));
                if (tile.name != "P1")
                { 
                    Vector3 pos = tile.transform.position;
                    GameObject.DestroyImmediate(tile);
                    tile = Instantiate(regions[k].tile);
                    tile.transform.position = new Vector3(j * 10, 1, i * 10);
                    tile.transform.rotation = Quaternion.Euler(0, 180, 0);
                    tile.transform.parent = this.transform;
                    tile.name = string.Format("tile_x{0}_y{1}", j, i);
                }
                if (j == x && i == y) {
                    GameObject temp2;
                    temp2 = Instantiate(temp1);
                    temp2.transform.position = new Vector3(tile.transform.position.x, 2, tile.transform.position.z);
                    temp2.transform.rotation = tile.transform.rotation;
                    temp2.transform.parent = tile.transform;
                }
            }
        }
        //SpawnBuilding.Spawn();
        return new Vector3(x * 10, 100, y * 10);
    }

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

        falloffMap = FalloffGen.generateFalloffMap(mapSize, falloffA, falloffB);
    }
}
