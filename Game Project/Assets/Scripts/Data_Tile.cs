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
    public bool hasTownHall = false;
    public bool hasBuilding = false;

    public int CanWork(Data_Unit _unit)
    {
        for (int i = 0; i < works.Length; i++)
        {
            if (works[i].job.unitJob == _unit.unitJob)
            {
                return i;
            }
        }
        return -1;
    }

    public void AttachWork(GameObject _unit)
    {
        if (!hasBuilding)
        {
            unit = _unit;
        }
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
