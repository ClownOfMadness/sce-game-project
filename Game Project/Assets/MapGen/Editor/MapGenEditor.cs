using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGen))]
public class MapGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGen MapGenerator = (MapGen)target;

        if (DrawDefaultInspector())
            if (MapGenerator.autoUpdate)
                MapGenerator.generateMap();

        if (GUILayout.Button("Generate"))
            MapGenerator.generateMap();
    }
}
