using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Camera MainCamera;
    // Start is called before the first frame update
    void Start()
    {
        
        MapGen Map = FindObjectOfType<MapGen>();
        MainCamera.transform.position =  Map.generateMap();
    }
}
