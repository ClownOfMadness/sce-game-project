using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class noise_map_test
{
    // Test if the noise map size is correct.
    [Test]
    public void noise_map_size()
    {
        int size = 20, count =  0;
        float[,] noiseMap = Noise.noiseMapGen(size, size, 1, 1, 1, 3, 0.5f, new Vector2(0,0));
        for(int i = 0; i < size; i++)
            for(int j = 0; j < size; j++)
                count++;

        Assert.AreEqual(count, size * size);
    }

    //Test if the noise map between the minimus and maximum values.
    [Test]
    public void noise_map_normalize()
    {
        int size = 20;
        float low = 0.2f, high = 1000f;
        float[,] noiseMap = Noise.noiseMapGen(size, size, 1, 1, 1, 3, 0.5f, new Vector2(0, 0));
        noiseMap = Noise.Normalize(low, high, size, size, noiseMap);

        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                Assert.IsTrue(noiseMap[j, i] <= high && noiseMap[j, i] >= low);
    }

    // Test if the falloff map size is correct.
    [Test]
    public void falloff_map_size()
    {
        int size = 20, count = 0;
        float[,] noiseMap = FalloffGen.generateFalloffMap(size, 3, 0.2f);
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                count++;

        Assert.AreEqual(count, size * size);
    }
}
