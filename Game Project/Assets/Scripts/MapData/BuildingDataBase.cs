using System.Collections.Generic;
using UnityEngine;
using System.Linq;  //needed for ToList function

//collection of all existing buildings
public class BuildingDataBase : ScriptableObject
{
    public List<Building> BuildingList;

    void Awake()
    {
        BuildingList = Resources.LoadAll<Building>("Buildings").ToList(); //fills list from Resources folder
    }
}