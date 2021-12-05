using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Map_FalloffGen
{
    //Create falloff map.
    public static float[,] generateFalloffMap(int size, int A, float B)
    {
        float[,] map = new float[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i / (float)size * 2 - 1;
                float y = j / (float)size * 2 - 1;

                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                //Decrease the range of the falloff.
                map[i, j] = Mathf.Pow(value, A) / (Mathf.Pow(value, A) + Mathf.Pow(B - B * value, A));
            }
        }
        return map;
    }
}
