using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    // Tile Description
    public new string name;

    // Card data

    // Pathfinding data
    public byte cost = 1;
    public ushort bestCost = ushort.MaxValue;
    //public GridDirection bestDirection;
}
