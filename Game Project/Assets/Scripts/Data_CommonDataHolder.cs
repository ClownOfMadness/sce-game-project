using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_CommonDataHolder : MonoBehaviour
{
    public Sprite workSiteSprite;
    public Sprite buildLocationTrue;
    public Sprite buildLocationFalse;
    public Sprite workPlace;
    public Sprite pointer;
    private void Awake()
    {
        if (!workSiteSprite)
        {
            Debug.LogError("Worksite sprite is missing from the Data_CommonDataHolder");
        }
        if (!buildLocationTrue)
        {
            Debug.LogError("BuildLocation True sprite is missing from the Data_CommonDataHolder");
        }
        if (!buildLocationFalse)
        {
            Debug.LogError("BuildLocation False sprite is missing from the Data_CommonDataHolder");
        }
        if (!workPlace)
        {
            Debug.LogError("Workplace False sprite is missing from the Data_CommonDataHolder");
        }
        if (!pointer)
        {
            Debug.LogError("Pointer sprite is missing from the Data_CommonDataHolder");
        }
    }
}
