using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    // Tile Description
    public new string name;
    public bool revealed = false;
    // Work data
    //public Dictionary<UnitData, ScriptableObject> work;
    [Serializable]
    public struct Work
    {
        public UnitData job;
        public ScriptableObject card;
        public float workTime;
    }

    public Work[] works;

    public GameObject unit = null;

    public void AttachWork(GameObject _unit)
    {
        unit = _unit;
    }

    public void DetachWork()
    {
        unit = null;
    }

    public bool GetData()
    {
        return unit;
    }

    public GameObject GetObject()
    {
        return unit;
    }
    
    // Card data

    // Pathfinding data
}
