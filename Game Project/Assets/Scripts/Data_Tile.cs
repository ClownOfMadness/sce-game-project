using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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
    public bool hasExtra = false;
    public bool thereIsRandom = false;
    public bool isAbyss = false;
    [Range(0.0f, 100.0f)]
    public float extraChance = 0f;
    public GameObject extraGameobject = null;
    public SpriteRenderer spriteRenderer = null; // Displays the sprite
    public SpriteRenderer workplaceRenderer = null;
    public SpriteRenderer gizmoRenderer = null;
    public SpriteRenderer pointerRenderer = null;
    public Sprite fullSprite = null; // Sprite that the tile starts with
    public Sprite halfSprite = null; // Sprite to change to once the resources are about to end
    public Sprite emptySprite = null; // Sprite to change to once there are no more resources
    public Sprite buildSprite = null; // Sprite to be shown under the building

    // [Tile Resources]
    [Serializable]
    public class Work // Resource work struct
    {
        public Data_Unit job; // Units that can work for the resource
        public Data_Card card; // Resource the unit will recieve
        public float workTime; // Time needed to recieve the card
        public bool extra;
        public bool random;
    }
    public Work[] works; // The works

    [Range(0, 50)]
    public int maxDurability = 0; // Maximum allowed durabilty
    [Range(0, 50)]
    public int durability = 0; // Current durability
    [Range(0, 50)]
    public int recharge = 0; // Amount to recharge

    //---------------------------------------[Automatic]-----------------------------------------------

    // [Tile Information]
    [HideInInspector] public float height; // Tile height
    private bool gizmoUse = false;

    // [Tile Resources]
    private bool canRecharge = false; // Checks if its daytime and can recharge
    private bool canRandom = true;
    private GameObject theExtra = null;
    [HideInInspector] public GameObject parentExtra = null;
    private bool extra = false;
    private bool check = false;

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
    private bool patrol = false;

    // [Tile Build]
    private List<GameObject> builders = new List<GameObject>();
    public int progress = 0;
    public bool buildDone = false;
    public float nextProgress = 0f;
    [HideInInspector] public AstarPath path;

    // [Abyss]
    [HideInInspector] public List<Data_Card> cards = new List<Data_Card>();
    private List<GameObject> enemies = new List<GameObject>();
    private bool abyssSetup = false;
    private bool canSpawn = true;
    private bool hasAbyss = false;

    // [Find Once Function]
    private Player_Control playerControl;
    private System_DayNight systemDayNight; // Script for day night cycle
    private Data_CommonDataHolder commonData;
    private Screen_Cards screenCards;
    private Enemy_List enemyList;
    private Game_Master game;
    private int loadCount = 0; // Amount of times to search before declaring a fail

    private void Awake()
    {
        Setup(); // Setups the tile
    }

    public void UpdateMe()
    {
        FindOnce(); // Searches for the needed components
        OnUpdate(); // Functions that work on update
        BuildProcess();
        Random();
    }

    private void Setup() // Setups the tile
    {
        if (isAbyss)
        {
            hasResources = false;
            maxDurability = 0;
            durability = 0;
            recharge = 0;
            hasAbyss = true;
        }

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
        if (!extraGameobject && hasExtra)
        {
            Debug.LogError("Extra GameObject component is missing from the " + tileName + " Data_Tile");
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

    private void Random()
    {
        if (!check && thereIsRandom && hasResources)
        {
            foreach (Work workRandom in works)
            {
                if (workRandom.random == true)
                {
                    workRandom.card = screenCards.Pool.GetLoot(3);
                }
            }
            check = true;
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

            if (!screenCards)
            {
                if (!(screenCards = GameObject.Find("CardsScreen").GetComponent<Screen_Cards>()))
                {
                    loadCount++;
                }
            }

            if (!enemyList)
            {
                if (!(enemyList = GameObject.Find("Enemies").GetComponent<Enemy_List>()))
                {
                    loadCount++;
                }
            }

            if (!path)
            {
                if (!(path = GameObject.Find("Pathfinder Grid").GetComponent<AstarPath>()))
                {
                    loadCount++;
                }
            }

            if (!game)
            {
                if (!(game = GameObject.Find("GameMaster").GetComponent<Game_Master>()))
                {
                    loadCount++;
                }
            }
        }
    }

    private void OnUpdate() // Functions that work on update
    {
        if (!hasBuilding && !hasTownHall)
        {
            if (durability > maxDurability)
                durability = maxDurability;

            // If the tile has no building
            if (durability <= 0 && hasResources)
            {
                // If the tile had resources and now they're gone
                if (!extra)
                    ReturnToDefault();
                else
                    NoExtra();
            }

            if (hasResources)
            {
                // If the tile still has resources
                Recharge();
            }

            if (hasExtra && hasResources)
                ExtraChance();
        }

        if (isAbyss && hasAbyss)
        {
            if (!abyssSetup)
            {
                while (cards.Count < 5)
                {
                    cards.Add(screenCards.Pool.GetLoot(3));
                }
                abyssSetup = true;
            }

            if (abyssSetup)
            {
                if (enemies.Count < cards.Count)
                {
                    Spawn();
                }
            }
        }

        if (playerControl.gizmoObject == this.gameObject && (playerControl.draggedType == "unit" || playerControl.draggedType == "building"))
        {
            ShowGizmo();
        }
        else
        {
            if (pointerRenderer.sprite != commonData.pointer)
                HideGizmo();
        }

        if (unit)
        {
            workplaceRenderer.enabled = true;
            if (patrol)
            {
                workplaceRenderer.sprite = commonData.patrolPlace;
            }
            else
            {
                workplaceRenderer.sprite = commonData.workPlace;
            }
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

    private void Spawn()
    {
        if (!systemDayNight.isDay)
        {
            // If it is daytime
            if (canSpawn)
            {
                // If it can recharge
                canSpawn = false;
                enemies.Add(enemyList.CreateEnemy(0, this.gameObject));
            }
        }
        else
        {
            // if it is nighttime
            if (!canSpawn)
            {
                // if it cannot recharge
                canSpawn = true;
            }
        }
    }

    public void TransferCard(Data_Card _card)
    {
        if (isAbyss && hasAbyss)
        {
            cards.Add(_card);
        }
    }

    public void DestroyBuilding()
    {
        StartCoroutine(RemoveBuilding());
        AstarPath.active.UpdateGraphs(GetComponent<BoxCollider>().bounds);
        hasAbyss = false;
        if (hasTownHall)
        {
            game.GameLost();
        }
        hasTownHall = false;
        hasBuilding = false;
        canBuild = true;
        canRecharge = false;
        building = null;
        dataBuilding = null;
        buildingComplete = false;
        works = null;
        maxDurability = 0;
        durability = 0;
        recharge = 0;
        hasResources = false;
        spriteRenderer.sprite = emptySprite;
    }

    private void ExtraChance()
    {
        if (systemDayNight.isDay)
        {
            if (canRandom)
            {
                canRandom = false;
                float result = UnityEngine.Random.Range(0f, 100f);
                if (result <= extraChance)
                {
                    HasExtra();
                }
                else
                {
                    extra = false;
                }
            }
        }
        else
        {
            if (!canRandom)
            {
                canRandom = true;
                NoExtra();
            }
        }
    }

    public void HasExtra()
    {
        extra = true;
        theExtra = Instantiate(extraGameobject, this.transform.position, Quaternion.Euler(90, 0, 0), this.gameObject.transform);
    }

    public void NoExtra()
    {
        extra = false;
        Destroy(theExtra);
        theExtra = null;
        durability += recharge;
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
                game.buildingsCount++;
                
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
        hasAbyss = false;
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
        if (theExtra)
            NoExtra();
    }

    public void PlaceReadyBuilding(GameObject _building)
    {
        hasTownHall = false;
        hasBuilding = true;
        canBuild = true;
        building = _building;
        dataBuilding = _building.GetComponent<Data_Building>();
        buildingComplete = true;
        works = null;
        maxDurability = 0;
        durability = 0;
        recharge = 0;
        hasResources = false;
        spriteRenderer.sprite = buildSprite;
        if (theExtra)
            NoExtra();
        dataBuilding.Complete();
    }

    public int CanWork(Data_Unit _unit) // Sends what work can unit work in, otherwise sends that it cannot
    {
        if (hasResources)
        {
            for (int i = 0; i < works.Length; i++)
            {
                if (works[i].job.unitJob == _unit.unitJob && works[i].extra == extra)
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
        if (hasResources && !hasBuilding && this.gameObject.layer != 7)
        {
            patrol = false;
            workplaceRenderer.transform.localPosition = new Vector3(0f, 0f, 0.625f);
            unit = _unit;
        }
    }

    public void AttachPatrol(GameObject _unit)
    {
        if (this.gameObject.layer != 7)
        {
            patrol = true;
            workplaceRenderer.transform.localPosition = new Vector3(0f, 0f, 0.155f);
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

    public bool GetExtra()
    {
        return extra;
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
        if (unit)
            return unit;
        return null;
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
            pointerRenderer.enabled = true;
            Color fade = pointerRenderer.color;
            fade.a = 1f;
            pointerRenderer.color = fade;
            StartCoroutine(Pointer());
        }
    }

    public IEnumerator Pointer()
    {
        pointerRenderer.sprite = commonData.pointer;
        yield return new WaitForSeconds(1);
        pointerRenderer.sprite = null;
        pointerRenderer.enabled = false;
        yield return null;
    }

    public IEnumerator RemoveBuilding()
    {
        building.transform.position = new Vector3(-500, -500, -500);
        yield return new WaitForSeconds(1);
        Destroy(building);
        yield return null;
    }


    public void LoadData(Tile_Info data)
    {
        height = data.height;
        gizmoUse = data.gizmoUse;
        canRecharge = data.canRecharge;
        canRandom = data.canRandom;
        //theExtra = data.theExtra;
        //parentExtra = data.parentExtra;
        extra = data.extra;
        check = data.check;
        revealed = data.revealed;
        unit = data.unit;
        building = data.building;
        dataBuilding = data.dataBuilding;
        hasTownHall = data.hasTownHall;
        hasBuilding = data.hasBuilding;
        buildingComplete = data.buildingComplete;
        hasResources = data.hasResources;
        patrol = data.patrol;
        builders = data.builders;
        progress = data.progress;
        buildDone = data.buildDone;
        nextProgress = data.nextProgress;
        cards = data.cards;
        enemies = data.enemies;
        abyssSetup = data.abyssSetup;
        canSpawn = data.canSpawn;
        hasAbyss = data.hasAbyss;
    }

    public Tile_Info SaveData()
    {
        Tile_Info tile_info = new Tile_Info();

        tile_info.height = height;
        tile_info.gizmoUse = gizmoUse;
        tile_info.canRecharge = canRecharge;
        tile_info.canRandom = canRandom;
        //tile_info.theExtra = theExtra;
        //tile_info.parentExtra = parentExtra;
        tile_info.extra = extra;
        tile_info.check = check;
        tile_info.revealed = revealed;
        tile_info.unit = unit;
        tile_info.building = building;
        tile_info.dataBuilding = dataBuilding;
        tile_info.hasTownHall = hasTownHall;
        tile_info.hasBuilding = hasBuilding;
        tile_info.buildingComplete = buildingComplete;
        tile_info.hasResources = hasResources;
        tile_info.patrol = patrol;
        tile_info.builders = builders;
        tile_info.progress =progress;
        tile_info.buildDone = buildDone;
        tile_info.nextProgress = 0;
        tile_info.cards = cards;
        tile_info. enemies = enemies;
        tile_info.abyssSetup = abyssSetup;
        tile_info.canSpawn = canSpawn;
        tile_info.hasAbyss = hasAbyss;

        return tile_info;
    }
    public class Tile_Info
    {
        public float height;
        public bool gizmoUse;
        public bool canRecharge;
        public bool canRandom;
        //public GameObject theExtra;
        //public GameObject parentExtra;
        public bool extra;
        public bool check;
        public bool revealed;
        public GameObject unit;
        public GameObject building;
        public Data_Building dataBuilding;
        public bool hasTownHall;
        public bool hasBuilding;
        public bool buildingComplete;
        public bool hasResources;
        public bool patrol;
        public List<GameObject> builders;
        public int progress;
        public bool buildDone;
        public float nextProgress;
        public List<Data_Card> cards;
        public List<GameObject> enemies;
        public bool abyssSetup;
        public bool canSpawn;
        public bool hasAbyss;
    }
}


