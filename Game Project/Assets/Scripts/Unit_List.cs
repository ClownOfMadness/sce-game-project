using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_List : MonoBehaviour
{
    [Serializable]
    public struct Units // This is a struct for connecting prefab to its holder
    {
        public GameObject unitPrefab;
        public GameObject unitGroup;
        public Data_Card unitCard;
    }

    public Units[] units;
    // [Int - Unit]
    // 0 - Peasant

    [HideInInspector] public GameObject townhall;
    private Quaternion rotation = Quaternion.Euler(Vector3.zero); // Default unit rotation

    public bool SummonUnit(int unit, GameObject tile, Data_Card card, bool gameStart)
    {
        if (unit < 0 || unit > units.Length)
        {
            Debug.LogError("You have given wrong int to the AddUnit function that is in Unit_List");
            return false;
        }
        else
        {
            Data_Unit unitData;
            Data_Tile dataTile = tile.GetComponent<Data_Tile>();
            if ((dataTile.revealed && !dataTile.hasBuilding && dataTile.gameObject.layer != 7) || gameStart)
            {
                GameObject newUnit = Instantiate(units[unit].unitPrefab, tile.transform.position, rotation, units[unit].unitGroup.transform);
                if (!(unitData = newUnit.GetComponent<Data_Unit>()))
                {
                    Debug.LogError("Cannot find Data_Unit when creating new unit in Unit_List");
                    return false;
                }
                else
                {
                    if (card)
                        unitData.card = card;
                }
                return true;
            }
            return false;
        }
    }
}
