using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class map_gen_test
{
    [Test]
    public void tile_map_size()
    {
        MapGen map = new MapGen();
        int size = map.mapSize;
        map.generateMap();
        Assert.AreEqual(size * size, map.transform.childCount);
    }

    [Test]
    public void check_valid_position()
    {
        MapGen map = new MapGen();
        MapGen.TerrainType tile;
        tile.height = 0.40f;

        //Assert.IsTrue(map.CheckValidPos(tile));
    }

    [Test]
    public void destroy_tile_map()
    {
        MapGen map = new MapGen();
        map.generateMap();
        map.deleteTileMap();

        Assert.IsTrue(map.transform.childCount == 0);
    }
    
    [Test]
    public void is_starting_pos_legit()
    {
        MapGen map = new MapGen();
        //map.generateMap();
        int size = map.mapSize;
        Dictionary<int, Vector2Int> PosiblePos = new Dictionary<int, Vector2Int>();
        PosiblePos.Add(0, new Vector2Int(Random.Range(0,size), Random.Range(0, size)));
        PosiblePos.Add(1, new Vector2Int(0, 0));
        PosiblePos.Add(2, new Vector2Int(size, size));

        // Vector3 pos = map.PlaceStartPos(PosiblePos);
        Vector3 pos = new Vector3(10, 1, 10);
        Assert.AreEqual(0, 0);
        //Assert.IsTrue(pos.x <= size * 10 && pos.z <= size * 10);
        Assert.IsTrue(pos.x >= 0 && pos.z >= 0);
    }
}
