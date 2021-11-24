using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vector3 worldPos; //Its position in the world
    public Vector2Int gridIndex; //Represents its x y position
    public byte cost;
    public ushort bestCost;
    //private GridDirection bestDirection;

    public Cell(Vector3 _worldPos, Vector2Int _gridIndex)
    {
        //Cell constructor
        worldPos = _worldPos;
        gridIndex = _gridIndex;
        cost = 1;
        bestCost = ushort.MaxValue;
        //bestDirection = GridDirection.None;
    }
}
