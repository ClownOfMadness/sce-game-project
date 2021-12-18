using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Building : MonoBehaviour
{
    //--------------------------------------[To-Do List]-----------------------------------------------

    // To add:
    // - building effects

    //-------------------------------------[Configuration]---------------------------------------------

    // [Building Description]
    public string buildingName = "None"; // Name of the building
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
            Debug.LogError("Building complete sprite is missing from the " + buildingName + " Data_building");
        }
        if (!buildingLight && hasLight)
        {
            Debug.LogError("Building light sprite is missing from the " + buildingName + " Data_building");
        }
        if (!townHallGround && isTownHall)
        {
            Debug.LogError("TownHall ground sprite is missing from the " + buildingName + " Data_building");
        }
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
            if (!dayNight.isDay && !night && spriteRenderer.sprite == buildingComplete)
            {
                night = true;
                transparency.a = 1f;
                secondaryRenderer.color = transparency;
            }
            else if (dayNight.isDay && night && spriteRenderer.sprite == buildingComplete)
            {
                night = false;
                transparency.a = 0f;
                secondaryRenderer.color = transparency;
            }
        }
    }
}
