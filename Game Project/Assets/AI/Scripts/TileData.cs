using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    // Tile Description
    public new string name;

    // Work data
    //public Dictionary<UnitData, ScriptableObject> work;
    [Serializable]
    public struct Work
    {
        public UnitData job;
        public ScriptableObject card;
    }

    public Work[] works;

    // Card data

    // Pathfinding data
}
