using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_List : MonoBehaviour
{
    [Serializable]
    public struct Enemies // This is a struct for connecting prefab to its holder
    {
        public GameObject enemyPrefab;
        public GameObject enemyGroup;
    }

    public Enemies[] enemies;
    private Quaternion rotation = Quaternion.Euler(Vector3.zero);

    public GameObject CreateEnemy(int enemy, GameObject abyssTile)
    {
        if (enemy < 0 || enemy > enemies.Length)
        {
            Debug.LogError("You have given wrong int to the CreateEnemy function that is in Enemy_List");
            return null;
        }
        else
        {
            Data_Enemy enemyData;
            Data_Tile dataTile = abyssTile.GetComponent<Data_Tile>();
            if (!dataTile.hasBuilding && dataTile.gameObject.layer != 7)
            {
                GameObject newEnemy = Instantiate(enemies[enemy].enemyPrefab, abyssTile.transform.position, rotation, enemies[enemy].enemyGroup.transform);
                if (!(enemyData = newEnemy.GetComponent<Data_Enemy>()))
                {
                    Debug.LogError("Cannot find Data_Enemy when creating new enemy in Enemy_List");
                    return null;
                }
                else
                {
                    enemyData.abyss = abyssTile;
                    enemyData.abyssData = dataTile;
                }
                return newEnemy;
            }
            return null;
        }
    }
}
