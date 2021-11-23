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
        public Color colour;
    }
    public enum MapType {noiseMap, colorMap, falloffMap, tileMap}
    public MapType mapType;

    public int mapSize;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int falloutA;
    public float falloutB;

    public int seed;
    public Vector2 offset;

    public TerrainType[] regions;

    public bool autoUpdate;
    public bool useFalloff;


    float[,] falloffMap;

    void Awake()
    {
        falloffMap = FalloffGen.generateFalloffMap(mapSize, falloutA, falloutB);
    }

    public void generateMap()
    {
        float[,] noiseMap = Noise.noiseMapGen(mapSize, mapSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
        Color[] colorMap = new Color[mapSize * mapSize];

        if (GameObject.Find(string.Format("tile_x{0}_y{1}", 0, 0)))
            deleteTileMap();

        for (int y = 0; y < mapSize; y++)
        {
            for( int x = 0; x < mapSize; x++)
            {
                if (useFalloff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }

                float currentHeight = noiseMap[x, y];

                for(int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height) 
                    {
                        if (mapType == MapType.tileMap)
                        {
                            GameObject tile = Instantiate(regions[i].tile);
                            tile.transform.position = new Vector3(10 * x , 1, 10 * y);
                            tile.name = string.Format("tile_x{0}_y{1}", x, y);
                        }
                        colorMap[y * mapSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (mapType == MapType.noiseMap)
            display.drawTexture(TextureGen.textureFromHeightMap(noiseMap));
        else if (mapType == MapType.colorMap)
            display.drawTexture(TextureGen.textureFromColorMap(colorMap, mapSize, mapSize));
        else if (mapType == MapType.falloffMap)
            display.drawTexture(TextureGen.textureFromHeightMap(FalloffGen.generateFalloffMap(mapSize, falloutA, falloutB)));
    }

    public void deleteTileMap()
    {
        for (int y = 0; y < 100; y++)
        {
            for (int x = 0; x < 100; x++)
            {
                GameObject tile = GameObject.Find(string.Format("tile_x{0}_y{1}", x, y));
                DestroyImmediate(tile);
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
        if (falloutA < 1)
            falloutA = 1;
        if (falloutB < 0.1f)
            falloutB = 0.1f;

        falloffMap = FalloffGen.generateFalloffMap(mapSize, falloutA, falloutB);
    }
}
