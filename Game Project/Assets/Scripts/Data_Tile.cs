using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Tile : MonoBehaviour
{
    //--------------------------------------[To-Do List]-----------------------------------------------

    // To add:
    // - display additional sprite if resources are about to end

    //-------------------------------------[Configuration]---------------------------------------------

    // [Tile Description]
    public string tileName = "None"; // Tile name
    public bool canBuild = false; // Determines if in this tile buildings can be build or rebuild
    public bool canBuildAtDefault = false; // Determines if in its default tile buildings can be build or rebuild
    public SpriteRenderer spriteRenderer; // Displays the sprite
    public SpriteRenderer workplaceRenderer;
    public SpriteRenderer gizmoRenderer;
    public SpriteRenderer pointerRenderer;
    public Sprite fullSprite; // Sprite that the tile starts with
    public Sprite halfSprite; // Sprite to change to once the resources are about to end
    public Sprite emptySprite; // Sprite to change to once there are no more resources
    public Sprite buildSprite; // Sprite to be shown under the building

    // [Tile Resources]
    [Serializable]
    public struct Work // Resource work struct
    {
        public Data_Unit job; // Units that can work for the resource
        public Data_Card card; // Resource the unit will recieve
        public float workTime; // Time needed to recieve the card
    }
    public Work[] works; // The works
    [Range(0.0f, 50.0f)]
    public int maxDurability = 0; // Maximum allowed durabilty
    [Range(0.0f, 50.0f)]
    public int durability = 0; // Current durability
    [Range(0.0f, 50.0f)]
    public int recharge = 0; // Amount to recharge

    //---------------------------------------[Automatic]-----------------------------------------------

    // [Tile Information]
    [HideInInspector] public float height; // Tile height
    private bool gizmoUse = false;

    // [Tile Resources]
    private bool canRecharge = false; // Checks if its daytime and can recharge

    // [Tile Fog]
    [HideInInspector] public bool revealed = false; // Checks it the tile has been discovered
    
    // [Tile Work]
    private GameObject unit = null; // holds the unit that works here
    [HideInInspector] public GameObject building = null; // holds the building that is on it
    private Data_Building dataBuilding = null;
    [HideInInspector] public bool hasTownHall = false; // True if it has townhall
    [HideInInspector] public bool hasBuilding = false; // True if it has building
    [HideInInspector] public bool buildingComplete = false; // True if the building is complete
    [HideInInspector] public bool hasResources = false; // True if it has resources

    // [Tile Build]
    public List<GameObject> builders = null;
    private int progress = 0;
    private bool buildDone = false;
    private bool canProgress = true;
    private float nextProgress = 0f;

    // [Find Once Function]
    private Player_Control playerControl;
    private System_DayNight systemDayNight; // Script for day night cycle
    private Data_CommonDataHolder commonData;
    private int loadCount = 0; // Amount of times to search before declaring a fail

    private void Awake()
    {
        Setup(); // Setups the tile
    }

    private void Update()
    {
        FindOnce(); // Searches for the needed components
        OnUpdate(); // Functions that work on update
        BuildProcess();
    }

    private void Setup() // Setups the tile
    {
        if (works.Length > 0) // If the tile has no resources to gather
        {
            hasResources = true;
        }
        else // If the tile has resources to gather
        {
            hasResources = false;
            maxDurability = 0;
            durability = 0;
            recharge = 0;
        }

        if (!spriteRenderer)
        {
            Debug.LogError("Sprite Renderer component is missing from the " + tileName + " Data_Tile");
        }
        if (!workplaceRenderer && hasResources)
        {
            Debug.LogError("Workplace Renderer component is missing from the " + tileName + " Data_Tile");
        }
        if (!gizmoRenderer)
        {
            Debug.LogError("Gizmo Renderer component is missing from the " + tileName + " Data_Tile");
        }
        if (!pointerRenderer)
        {
            Debug.LogError("Pointer Renderer component is missing from the " + tileName + " Data_Tile");
        }
        if (!emptySprite && hasResources)
        {
            Debug.LogError("Empty sprite is missing from the " + tileName + " Data_Tile");
        }
        if (!halfSprite && hasResources)
        {
            Debug.LogError("Half sprite is missing from the " + tileName + " Data_Tile");
        }
        if (!fullSprite)
        {
            Debug.LogError("Full sprite is missing from the " + tileName + " Data_Tile");
        }
        if (!buildSprite && (canBuild || canBuildAtDefault))
        {
            Debug.LogError("Build sprite is missing from the " + tileName + " Data_Tile");
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

            if (!commonData)
            {
                if (!(commonData = GameObject.Find("Map Generator").GetComponent<Data_CommonDataHolder>()))
                {
                    loadCount++;
                }
            }

            if (!playerControl)
            {
                if (!(playerControl = GameObject.Find("PlayerControl").GetComponent<Player_Control>()))
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

        if (playerControl.gizmoObject == this.gameObject && (playerControl.draggedType == "unit" || playerControl.draggedType == "building"))
        {
            ShowGizmo();
        }
        else
        {
            HideGizmo();
        }

        if (unit)
        {
            workplaceRenderer.enabled = true;
            workplaceRenderer.sprite = commonData.workPlace;
        }
        else
        {
            workplaceRenderer.enabled = false;
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

    private void BuildProcess()
    {
        if (hasBuilding && !buildingComplete)
        {
            if (builders.Count > 0 && !buildDone)
            {
                if (Time.time > nextProgress)
                {
                    progress++;
                    nextProgress = Time.time + (1f / builders.Count);
                }
                if (progress >= dataBuilding.buildTime)
                {
                    buildDone = true;
                }
            }
            else if (buildDone)
            {
                buildingComplete = true;
                foreach (GameObject _unit in builders)
                {
                    DetachBuild(_unit);
                }
                spriteRenderer.sprite = buildSprite;
                dataBuilding.Complete();
            }
        }
        if (hasBuilding && buildingComplete && builders.Count > 0)
        {
            foreach (GameObject _unit in builders)
            {
                DetachBuild(_unit);
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
        spriteRenderer.sprite = emptySprite;
    }

    public void PlaceTownHall(GameObject _building)
    {
        hasTownHall = true;
        hasBuilding = false;
        canBuild = false;
        building = _building;
        dataBuilding = _building.GetComponent<Data_Building>();
        buildingComplete = true;
        works = null;
        maxDurability = 0;
        durability = 0;
        recharge = 0;
        hasResources = false;
        if (buildSprite)
            spriteRenderer.sprite = buildSprite;
        else
            spriteRenderer.sprite = _building.GetComponent<Data_Building>().townHallGround;
    }

    public void PlaceBuilding(GameObject _building)
    {
        hasTownHall = false;
        hasBuilding = true;
        canBuild = true;
        building = _building;
        dataBuilding = _building.GetComponent<Data_Building>();
        buildingComplete = false;
        works = null;
        maxDurability = 0;
        durability = 0;
        recharge = 0;
        hasResources = false;
        spriteRenderer.sprite = commonData.workSiteSprite;
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

    public bool CanBuild(Data_Unit _unit)
    {
        if (hasBuilding && !buildingComplete && dataBuilding)
        {
            if (_unit.unitJob == dataBuilding.canBuild.unitJob)
            {
                return true;
            }
        }
        return false;
    }

    public void AttachWork(GameObject _unit) // Adds unit to the work place
    {
        if (hasResources && !hasBuilding)
        {
            unit = _unit;
        }
    }

    public void AttachBuild(GameObject _unit)
    {
        if (hasBuilding && !buildingComplete)
        {
            builders.Add(_unit);
        }
    }

    public void DetachWork() // Removes unit from the work place
    {
        unit = null;
    }

    public void DetachBuild(GameObject _unit)
    {
        if (hasBuilding && !buildingComplete && builders.Count > 0)
        {
            for (int i = 0; i < builders.Count; i++)
            {
                if (builders[i] == _unit)
                {
                    builders.RemoveAt(i);
                }
            }
        }
    }    

    public bool GetData() // Returns the unit that works here
    {
        return unit;
    }

    public bool GetBuildData()
    {
        if (builders.Count > 0)
            return true;
        return false;
    }

    public GameObject GetObject() // Returns the object of the unit that works here
    {
        if (hasBuilding && !buildingComplete)
        {
            if (builders.Count > 0)
            {
                return builders[UnityEngine.Random.Range(0, (builders.Count - 1))];
            }
        }
            return unit;
    }

    private void ShowGizmo()
    {
        gizmoUse = true;
        gizmoRenderer.enabled = true;
        pointerRenderer.enabled = true;
        Color fade = pointerRenderer.color;
        fade.a = 0.5f;
        pointerRenderer.color = fade;
        pointerRenderer.sprite = playerControl.draggedSprite;
        if (playerControl.draggedType == "building")
        {
            if (revealed && canBuild)
            {
                gizmoRenderer.sprite = commonData.buildLocationTrue;
            }
            else
            {
                gizmoRenderer.sprite = commonData.buildLocationFalse;
            }
        }
        else if (playerControl.draggedType == "unit")
        {
            if (revealed && this.gameObject.layer == 6)
            {
                gizmoRenderer.sprite = commonData.buildLocationTrue;
            }
            else
            {
                gizmoRenderer.sprite = commonData.buildLocationFalse;
            }
        }
    }

    private void HideGizmo()
    {
        gizmoUse = false;
        gizmoRenderer.enabled = false;
        pointerRenderer.enabled = false;
    }

    public void DrawPointer()
    {
        if (!gizmoUse)
        {
            StartCoroutine(Pointer());
        }
    }

    public IEnumerator Pointer()
    {
        pointerRenderer.enabled = true;
        Color fade = pointerRenderer.color;
        fade.a = 1f;
        pointerRenderer.color = fade;
        pointerRenderer.sprite = commonData.pointer;
        yield return new WaitForSeconds(1);
        pointerRenderer.enabled = false;
        yield return null;
    }
}
