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
    
    public int mapSize;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int falloffA;
    public float falloffB;

    public int seed;
    public Vector2 offset;

    public TerrainType[] regions;

    public bool autoUpdate;
    public bool useFalloff;


    float[,] falloffMap;

    void Awake()
    {
        falloffMap = FalloffGen.generateFalloffMap(mapSize, falloffA, falloffB);
    }

    public void generateMap(int seed)
    {
        float[,] noiseMap = Noise.noiseMapGen(mapSize, mapSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
        //System.Random StartPosition = new System.Random();

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
