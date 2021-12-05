using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Tile : MonoBehaviour
{
    // Tile Description
    public string tileName;
    [HideInInspector] public bool revealed = false;
    // Work data
    //public Dictionary<Data_Unit, ScriptableObject> work;
    [Serializable]
    public struct Work
    {
        public Data_Unit job;
        public ScriptableObject card;
        public float workTime;
    }

    public Work[] works;

    private GameObject unit = null;

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
