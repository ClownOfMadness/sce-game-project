using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitList : MonoBehaviour
{
    [Serializable]
    public struct Units // This is a struct for connecting prefab to its holder
    {
        public GameObject unitPrefab;
        public GameObject unitGroup;
    }

    public Units[] units;
    // [Int - Unit]
    // 0 - Peasant

    private Quaternion rotation = Quaternion.Euler(Vector3.zero); // Default unit rotation

    public void AddUnit(Vector3 location, int unit) // Creates the unit in a given location
    {
        if (unit < 0 || unit > units.Length)
        {
            Debug.LogError("You have given wrong int to the AddUnit function");
        }
        else
        {
            Instantiate(units[unit].unitPrefab, location, rotation, units[unit].unitGroup.transform);
        }
    }
}
