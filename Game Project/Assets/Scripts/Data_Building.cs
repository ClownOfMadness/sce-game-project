using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Building : MonoBehaviour
{
    //-------------------------------------[Configuration]---------------------------------------------

    // [Building Description]
    public string buildingName = "None"; // Name of the building
    public Data_Card buildingCard = null;
    public int buildingHealth = 20;
    public int buildTime = 0;
    public bool isTownHall = false; // Is this a townhall?
    public bool hasLight = false; // Does the building have light
    public Data_Unit canBuild; // Who can build this?
    public SpriteRenderer spriteRenderer; // Building sprite renderer
    public SpriteRenderer secondaryRenderer; // Additional sprite renderer to render things like lighting and blueprint
    public Sprite buildingComplete; // Building complete sprite
    public Sprite buildingLight; // Building light shine at night
    public Sprite townHallGround; // Only for townhall

    //---------------------------------------[Automatic]-----------------------------------------------

    // [Building Appearance]
    private System_DayNight dayNight; // System DayNight script
    private Color transparency; // Secondary sprite transparancy
    private Color originalColor; // Save of the original color
    private bool night; // Checks if its night and updates the bool
    private bool inProgress = false;

    // [Building Health]
    private int health;

    // [Find Once Function]
    private int loadCount = 0; // Amount of times to search before declaring a fail

    private void Awake()
    {
        if (!canBuild && !isTownHall)
        {
            Debug.LogError("Data Unit is missing from the " + buildingName + " Data_Building");
        }
        if (!spriteRenderer)
        {
            Debug.LogError("Sprite Renderer is missing from the " + buildingName + " Data_Building");
        }
        if (!secondaryRenderer)
        {
            Debug.LogError("Secondary Sprite Renderer is missing from the " + buildingName + " Data_Building");
        }
        if (!buildingComplete)
        {
            Debug.LogError("Building complete sprite is missing from the " + buildingName + " Data_Building");
        }
        if (!buildingLight && hasLight)
        {
            Debug.LogError("Building light sprite is missing from the " + buildingName + " Data_Building");
        }
        if (!townHallGround && isTownHall)
        {
            Debug.LogError("TownHall ground sprite is missing from the " + buildingName + " Data_Building");
        }
        if (!isTownHall && !buildingCard)
        {
            Debug.LogError("BuildingCard is missing from the " + buildingName + " Data_Building");
        }

        health = buildingHealth;
    }

    private void Start()
    {
        spriteRenderer.sortingOrder = -(int)Mathf.Abs(this.transform.position.z);
        secondaryRenderer.sortingOrder = -(int)Mathf.Abs(this.transform.position.z) + 1;

        transparency = secondaryRenderer.color;
        originalColor = transparency;
        if (!isTownHall)
        {
            spriteRenderer.sprite = buildingComplete;
            spriteRenderer.enabled = false;
            secondaryRenderer.sprite = buildingComplete;
            transparency.a = 0.5f;
            secondaryRenderer.color = transparency;
        }
        else
        {
            spriteRenderer.sprite = buildingComplete;
            if (hasLight)
                secondaryRenderer.sprite = buildingLight;
            transparency.a = 0f;
            secondaryRenderer.color = transparency;
        }
    }

    private void Update()
    {
        FindOnce();
        NightMode();
        Rebuild();
    }

    private void FindOnce()
    {
        if (loadCount > 60)
        {
            Debug.LogError("Failed to find needed parameters in FindOnce() in the Data_Unit script");
        }
        else
        {
            if (!dayNight)
            {
                if (!(dayNight = GameObject.Find("Day/Night Cycle").GetComponent<System_DayNight>()))
                {
                    loadCount++;
                }
            }
        }
    }

    public void Complete()
    {
        inProgress = true;
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = buildingComplete;
        if (hasLight)
            secondaryRenderer.sprite = buildingLight;
        transparency = originalColor;
        transparency.a = 0f;
        secondaryRenderer.color = transparency;
        night = false;
    }

    private void NightMode()
    {
        if (hasLight)
        {
            if (!dayNight.isDay && !night && secondaryRenderer.sprite != buildingComplete)
            {
                night = true;
                transparency.a = 1f;
                secondaryRenderer.color = transparency;
            }
            else if (dayNight.isDay && night && secondaryRenderer.sprite != buildingComplete)
            {
                night = false;
                transparency.a = 0f;
                secondaryRenderer.color = transparency;
            }
        }
    }

    private void Rebuild()
    {
        if (inProgress)
        {
            if (dayNight.isDay && health != buildingHealth)
            {
                health = buildingHealth;
            }
        }
    }

    public void Hurt(int damage, Data_Enemy enemy)
    {
        if (inProgress || isTownHall)
        {
            if (health - damage <= 0)
            {
                enemy.card = buildingCard;
                health -= damage;
                GetComponentInParent<Data_Tile>().DestroyBuilding();
            }
            else
            {
                health -= damage;
            }
        }
        else
        {
            enemy.card = buildingCard;
            GetComponentInParent<Data_Tile>().DestroyBuilding();
        }
    }

    public void LoadData(Building_Info data)
    {
        transparency = data.transparency;
        originalColor = data.originalColor;
        night = data.night;
        inProgress = data.inProgress;
        health = data.health;
    }

    public Building_Info SaveData()
    {
        Building_Info building_info = new Building_Info();

        building_info.transparency = transparency;
        building_info.originalColor = originalColor;
        building_info.night = night;
        building_info.inProgress = inProgress;
        building_info.health = health;

        return building_info;
    }

    public class Building_Info
    {
        public Color transparency;
        public Color originalColor;
        public bool night;
        public bool inProgress;
        public int health;
    }
}
