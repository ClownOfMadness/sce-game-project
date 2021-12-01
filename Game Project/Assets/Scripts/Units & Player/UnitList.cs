using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitList : MonoBehaviour
{
    [Serializable]
    public struct Units
    {
        public GameObject unitPrefab;
        public GameObject unitGroup;
    }

    public Units[] units;

    private Quaternion rotation = Quaternion.Euler(Vector3.zero);

    public void AddUnit(Vector3 location, int unit)
    {
        Instantiate(units[0].unitPrefab, location, rotation, units[0].unitGroup.transform);
    }
}
