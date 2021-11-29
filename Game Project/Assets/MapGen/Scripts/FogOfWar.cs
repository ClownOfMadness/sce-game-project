using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    public GameObject Fog;
    GameObject[,] FogMap;

    public void Createfog(int mapSize)
    {
        FogMap = new GameObject[mapSize, mapSize];
        deleteFogMap();
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                FogMap[x, y] = Instantiate(Fog, new Vector3(x * 10, 3, y * 10), Quaternion.identity);
                FogMap[x, y].transform.parent = this.transform;
                FogMap[x, y].name = string.Format("fog_x{0}_y{1}", x, y);
            }
        }
    }

    public void deleteFogMap()
    {
        while (this.transform.childCount != 0)
        {
            foreach (Transform child in this.transform)
            {
                GameObject.DestroyImmediate(child.gameObject);
            }
        }
    }
}
