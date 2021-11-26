using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for creating and storing Hand zone, extension of ZoneBehaviour
public class ZoneHand : ZoneBehaviour
{
    [Header("Fill Hand:")]
    public bool Fill = false;

    public void Awake()
    {
        Size = 8;               //max Zone size
        InstantiateZone(this);  //create and instantiate objects in scene in runtime
    }
    public void Update()
    {
        if (Fill)
        {
            InstantiateZone(this);   //add objects to hand up to 8 in runtime
            Fill = false;
        }
    }

}


