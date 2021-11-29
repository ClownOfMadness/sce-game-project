using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FogOfWar))]
public class MapFogEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FogOfWar FogGenerator = (FogOfWar)target;
        MapGen Map = FindObjectOfType<MapGen>();
        int size = Map.mapSize;

        if (GUILayout.Button("Generate"))
            FogGenerator.Createfog(size);

        if (GUILayout.Button("Delete Fog"))
            FogGenerator.deleteFogMap();
    }
}
