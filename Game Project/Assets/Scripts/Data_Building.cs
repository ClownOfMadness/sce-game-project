using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Building : MonoBehaviour
{
    // To add: building effects
    
    // Building Description
    public string buildingName;//name
    public Sprite buildingStart;
    public bool isLively;//determine if villagers can live in this building (add the max capacity of villagers or not)
    public int capacity;//the capacity of villagers in this building
}
