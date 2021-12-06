using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class System_DayNight : MonoBehaviour
{
    public int dayLength = 10; // minutes
    public int dayStart = 1;
    public int nightStart = 8;
    private int midNight;
    private int noon;
    private int nightStartJump;
    private int currentTime;
    public float cycleSpeed = 1f; // [[Add this option to parental control]]
    [HideInInspector] public bool isDay;
    private PostProcessVolume volume;
    private ColorGrading timeColor;
    private bool allGood = false;
    
    public float tempOffset = 20f;
    public float tempContrast = 90f;
    public float nightTempOffset = 30f;
    public float brightnessOffset = 10f;
    public float brightnessContrast = 20f;
    public float nightBrightnessOffset = 30f;
    private int timeJump;
    private int currentJump;

    private void Awake()
    {
        if(!(volume = GetComponent<PostProcessVolume>()))
        {
            Debug.LogError("PostProcessVolume component is not found in System_DayNight");
        }
        else
        {
            if(!volume.profile.TryGetSettings<ColorGrading>(out timeColor))
            {
                Debug.LogError("Cannot find ColorGrading setting from PostProcessingVolume in System_DayNight");
            }
            else
            {
                if (dayLength <= 0 || dayStart >= nightStart || nightStart >= dayLength || cycleSpeed <= 0)
                {
                    Debug.LogError("The values that are given to the System_DayNight are incorrect");
                }
                else
                {
                    allGood = true;
                }
            }
        }
    }

    private void Start()
    {
        currentTime = 0;
        midNight = (int)(((dayLength - (nightStart - dayStart)) / 2) + nightStart);
        noon = (int)(((nightStart - dayStart) / 2) + dayStart);
        timeJump = (int)(midNight - noon);
        nightStartJump = (dayLength - nightStart) - (dayLength - midNight);
        if (allGood)
            StartCoroutine(TimeOfDay());
    }

    private void Update()
    {
        if (currentTime >= 0 && currentTime < dayStart)
        {
            isDay = false;
            timeColor.temperature.value = (currentTime * 22f) - 18;
        }
        else if (currentTime >= dayStart && currentTime < nightStart)
        {
            isDay = true;
        }
        else if (currentTime >= nightStart && currentTime < dayLength)
        {
            isDay = false;
        }
        else if (currentTime >= dayLength)
        {
            currentTime = 0;
        }
        if (allGood)
        {
            if (currentTime >= 0 && currentTime < noon)
            {
                currentJump = currentTime + (dayLength - midNight);
            }
            else if (currentTime >= noon && currentTime < midNight || (currentTime >= midNight && currentTime < dayLength))
            {
                currentJump = (dayLength - currentTime) - (dayLength - midNight);
            }

            if (currentJump <= nightStartJump)
            {
                timeColor.temperature.value = (float)((nightStartJump * (tempContrast / (timeJump - nightStartJump))) - (tempOffset + nightTempOffset));
                timeColor.brightness.value = (float)((nightStartJump * (brightnessContrast / (timeJump - nightStartJump))) - (brightnessOffset + nightBrightnessOffset));
            }
            else
            {
                timeColor.temperature.value = (float)(((currentJump - nightStartJump) * (tempContrast / (timeJump - nightStartJump))) - tempOffset);
                timeColor.brightness.value = (float)(((currentJump - nightStartJump) * (brightnessContrast / (timeJump - nightStartJump))) - brightnessOffset);
            }
        }
    }

    IEnumerator TimeOfDay()
    {
        while (true)
        {
            currentTime += 1;
            yield return new WaitForSeconds((1f / cycleSpeed) * 60f);
        }
    }
}
