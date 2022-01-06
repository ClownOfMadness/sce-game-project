using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class map_gen_test: MonoBehaviour
{
    //Test is the size of the map is correct.
    [Test]
    public void tile_map_size()
    {
        Map_Gen map = FindObjectOfType<Map_Gen>();
        int size = map.mapSize;
        map.generateMap();

        Assert.AreEqual(size * size, map.transform.childCount);
    }

    //Test if the validity of the position.
    [Test]
    public void check_valid_position()
    {
        Map_Gen map = FindObjectOfType<Map_Gen>();
        Map_Gen.TerrainType tile = new Map_Gen.TerrainType();

        tile.height = 0.40f;// Valid height is between 0.35 and 0.6
        Assert.IsTrue(map.CheckValidPos(tile));
        tile.height = 0.7f;
        Assert.IsFalse(map.CheckValidPos(tile));
    }

    //Test if the map fully removed.
    [Test]
    public void destroy_tile_map()
    {
        Map_Gen map = FindObjectOfType<Map_Gen>();
        map.generateMap();
        map.deleteTileMap();

        Assert.IsTrue(map.transform.childCount == 0);
    }
}
