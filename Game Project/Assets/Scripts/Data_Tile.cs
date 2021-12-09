using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Tile : MonoBehaviour
{
    // To add: display work tile, display selected tile
    
    // [Tile Description - To configure]
    public string tileName; // Tile name
    public bool canBuild = false; // Determines if in this tile buildings can be build or rebuild
    public bool canBuildAtDefault = false; // Determines if in its default tile buildings can be build or rebuild
    public SpriteRenderer spriteRenderer; // Displays the sprite
    public Sprite regularSprite; // Sprite that the tile starts with
    public Sprite defaultSprite; // Sprite to change to once there are no more resources

    // [Tile Resources - To configure]
    [Serializable]
    public struct Work // Resource work struct
    {
        public Data_Unit job; // Units that can work for the resource
        public Data_Card card; // Resource the unit will recieve
        public float workTime; // Time needed to recieve the card
    }
    public Work[] works; // The works
    public int maxDurability; // Maximum allowed durabilty
    public int durability; // Current durability
    public int recharge; // Amount to recharge

    // [Tile Resources - Automated]
    private bool canRecharge = false; // Checks if its daytime and can recharge

    // [Tile Fog - Automated]
    [HideInInspector] public bool revealed = false; // Checks it the tile has been discovered
    
    // [Tile Work - Automated]
    private GameObject unit = null; // holds the unit that works here
    [HideInInspector] public GameObject building = null; // holds the building that is on it
    [HideInInspector] public bool hasTownHall = false; // True if it has townhall
    [HideInInspector] public bool hasBuilding = false; // True if it has building
    [HideInInspector] public bool hasResources = false; // True if it has resources

    // [Find Once Function]
    private System_DayNight systemDayNight; // Script for day night cycle
    private int loadCount = 0; // Amount of times to search before declaring a fail

    private void Awake()
    {
        Setup(); // Setups the tile
    }

    private void Update()
    {
        FindOnce(); // Searches for the needed components
        OnUpdate(); // Functions that work on update
    }

    private void Setup() // Setups the tile
    {
        if (!spriteRenderer)
        {
            Debug.LogError("Sprite Renderer component is missing from the Data_Tile");
        }
        if (!defaultSprite)
        {
            Debug.LogError("Default sprite is missing from the Data_Tile");
        }

        //spriteRenderer.sprite = regularSprite;

        if (works.Length > 0) // If the tile has no resources to gather
        {
            hasResources = true;
        }
        else // If the tile has resources to gather
        {
            hasResources = false;
        }
    }

    private void FindOnce() // Searches for the needed components
    {
        if (loadCount > 60)
        {
            Debug.LogError("Failed to find needed parameters in FindOnce() in the Data_Unit script");
        }
        else
        {
            if (!systemDayNight)
            {
                if (!(systemDayNight = GameObject.Find("Day/Night Cycle").GetComponent<System_DayNight>()))
                {
                    loadCount++;
                }
            }
        }
    }

    private void OnUpdate() // Functions that work on update
    {
        if (!hasBuilding)
        {
            // If the tile has no building
            if (durability <= 0 && hasResources)
            {
                // If the tile had resources and now they're gone
                ReturnToDefault();
            }

            if (hasResources)
            {
                // If the tile still has resources
                Recharge();
            }
        }
    }

    private void Recharge() // Recharge the tile's resources
    {
        if (systemDayNight.isDay)
        {
            // If it is daytime
            if (canRecharge)
            {
                // If it can recharge
                canRecharge = false;
                if (durability < maxDurability)
                {
                    // If the durability is below max durability
                    durability += recharge;
                }
            }
        }
        else
        {
            // if it is nighttime
            if (!canRecharge)
            {
                // if it cannot recharge
                canRecharge = true;
            }
        }
    }

    public void ReturnToDefault() // Returns the tile to dafault state, removing its resources
    {
        works = null;
        maxDurability = 0;
        durability = 0;
        recharge = 0;
        hasResources = false;
        canBuild = canBuildAtDefault;
        spriteRenderer.sprite = defaultSprite;
    }

    public int CanWork(Data_Unit _unit) // Sends what work can unit work in, otherwise sends that it cannot
    {
        if (hasResources)
        {
            for (int i = 0; i < works.Length; i++)
            {
                if (works[i].job.unitJob == _unit.unitJob)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    public void AttachWork(GameObject _unit) // Adds unit to the work place
    {
        if (hasResources)
        {
            unit = _unit;
        }
    }

    public void DetachWork() // Removes unit from the work place
    {
        unit = null;
    }

    public bool GetData() // Returns the unit that works here
    {
        return unit;
    }

    public GameObject GetObject() // Returns the object of the unit that works here
    {
        return unit;
    }
    
    // Card data

    // Pathfinding data
}
