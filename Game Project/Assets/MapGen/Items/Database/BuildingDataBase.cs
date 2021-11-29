using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new DataBase", menuName = "Assets/Databases/Building Database")]
public class BuildingDataBase : ScriptableObject
{
    public List<Building> BuildingList;
}
