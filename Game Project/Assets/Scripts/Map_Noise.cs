using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Map_Noise
{
    //Create noise map using the given parameters.
    public static float[,] noiseMapGen(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];
        System.Random prng = new System.Random(seed);

        //Creating array of offsets of the size as the number of octaves.
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-10000, 10000) + offset.x;
            float offsetY = prng.Next(-10000, 10000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }
        
        //Limit the scale to prevent division by zero.
        if (scale <= 0)
            scale = 0.001f;

        //Set the max as smallest float value, min as the highes float value.
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        //Variables for centerlizing the map.
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        //Create the noise map using the perlin noise function.
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                //Updating the maximum and the minimum.
                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                noiseMap[x, y] = noiseHeight;
            }
        }

        return Normalize(maxNoiseHeight, minNoiseHeight, mapHeight, mapWidth, noiseMap);
    }

    //Normalize the map between the created minimum and maximum.
    public static float[,] Normalize(float high, float low, int height, int width, float[,] map)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, y] = Mathf.InverseLerp(low, high, map[x,y]);
            }
        }
        return map;
    }
}