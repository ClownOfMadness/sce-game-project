using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Building : MonoBehaviour
{
    //--------------------------------------[To-Do List]-----------------------------------------------

    // To add:
    // - building effects
    // - process of building the building

    //-------------------------------------[Configuration]---------------------------------------------

    // [Building Description]
    public string buildingName = "None"; // Name of the building
    public bool isTownHall = false; // Is this a townhall?
    public bool hasLight = false; // Does the building have light
    public SpriteRenderer spriteRenderer; // Building sprite renderer
    public SpriteRenderer secondaryRenderer; // Additional sprite renderer to render things like lighting and blueprint
    public Sprite buildingBlueprint; // Building ground sprite
    public Sprite buildingComplete; // Building complete sprite
    public Sprite buildingLight; // Building light shine at night

    //---------------------------------------[Automatic]-----------------------------------------------

    // [Building Appearance]
    private System_DayNight dayNight; // System DayNight script
    private Color transparency; // Secondary sprite transparancy
    private bool night; // Checks if its night and updates the bool

    // [Find Once Function]
    private int loadCount = 0; // Amount of times to search before declaring a fail

    private void Awake()
    {
        if (!spriteRenderer)
        {
            Debug.LogError("Sprite Renderer is missing from the " + buildingName + " Data_Building");
        }
        if (!secondaryRenderer)
        {
            Debug.LogError("Secondary Sprite Renderer is missing from the " + buildingName + " Data_Building");
        }
        if (!buildingBlueprint && !isTownHall)
        {
            Debug.LogError("Building blueprint sprite is missing from the " + buildingName + " Data_Building");
        }
        if (!buildingComplete)
        {
            Debug.LogError("Building complete sprite is missing from the " + buildingName + " Data_building");
        }
        if (!buildingLight && hasLight)
        {
            Debug.LogError("Building light sprite is missing from the " + buildingName + " Data_building");
        }
    }

    private void Start()
    {
        transparency = secondaryRenderer.color;
        if (!isTownHall)
        {
            spriteRenderer.sprite = buildingBlueprint;
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
        spriteRenderer.sprite = buildingComplete;
        if (hasLight)
            secondaryRenderer.sprite = buildingLight;
        transparency.a = 0f;
        secondaryRenderer.color = transparency;
    }

    private void NightMode()
    {
        if (hasLight)
        {
            if (!dayNight.isDay && !night)
            {
                night = true;
                transparency.a = 1f;
                secondaryRenderer.color = transparency;
            }
            else if (dayNight.isDay && night)
            {
                night = false;
                transparency.a = 0f;
                secondaryRenderer.color = transparency;
            }
        }
    }
}
