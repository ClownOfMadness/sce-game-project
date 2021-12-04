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
        MapGen map = FindObjectOfType<MapGen>();
        int size = map.mapSize;
        map.generateMap();
        
        Assert.AreEqual(size * size, map.transform.childCount);
    }

    //Test if the validity of the position.
    [Test]
    public void check_valid_position()
    {
        MapGen map = FindObjectOfType<MapGen>();
        MapGen.TerrainType tile = new MapGen.TerrainType();

        tile.height = 0.40f;// Valid height is between 0.35 and 0.6
        Assert.IsTrue(map.CheckValidPos(tile));
        tile.height = 0.7f;
        Assert.IsFalse(map.CheckValidPos(tile));
    }

    //Test if the map fully removed.
    [Test]
    public void destroy_tile_map()
    {
        MapGen map = FindObjectOfType<MapGen>();
        map.generateMap();
        map.deleteTileMap();

        Assert.IsTrue(map.transform.childCount == 0);
    }
    
    ////Test if the returned starting point is in the valid range.
    //[Test]
    //public void is_starting_pos_legit()
    //{
    //    MapGen map = FindObjectOfType<MapGen>();
    //    map.generateMap();
    //    int size = map.mapSize, count = 0;

    //    //Create a test dictionery.
    //    Dictionary<int, Vector2Int> PosiblePos = new Dictionary<int, Vector2Int>();
    //    for(int y = 0; y < size; y++)
    //        for(int x = 0; x < size; x++)
    //            PosiblePos.Add(count++, new Vector2Int(x, y));

    //    Vector3 pos = map.PlaceStartPos(PosiblePos);

    //    Assert.IsTrue(pos.x <= size * 10 && pos.z <= size * 10);
    //    Assert.IsTrue(pos.x >= 0 && pos.z >= 0);
    //}
}
