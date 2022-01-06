using UnityEngine;

[CreateAssetMenu(fileName = "new Building", menuName = "Building")]
public class Building : ScriptableObject
{
    public string BuildingID;//"BU" + "(00 - 99)"
    public string BuildingName;//the name of the building
    [TextArea]
    public string Description;//description of the building
    public bool isLively;//determine if villagers can live in this building (add the max capacity of villagers or not)
    public int capacity;//the capacity of villagers in this building
}
