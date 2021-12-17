using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_CommonDataHolder : MonoBehaviour
{
    public Sprite workSiteSprite;
    public Sprite buildLocationTrue;
    public Sprite buildLocationFalse;
    public Sprite workPlace;

    private void Awake()
    {
        if (!workSiteSprite)
        {
            Debug.LogError("Worksite sprite is missing from the Data_Building_Father");
        }
        if (!buildLocationTrue)
        {
            Debug.LogError("BuildLocation True sprite is missing from the Data_Building_Father");
        }
        if (!buildLocationFalse)
        {
            Debug.LogError("BuildLocation False sprite is missing from the Data_Building_Father");
        }
        if (!workPlace)
        {
            Debug.LogError("Workplace False sprite is missing from the Data_Building_Father");
        }
    }
}
