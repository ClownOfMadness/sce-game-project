using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for creating and storing Zone2, will need to be split into 2 zones at a later stage?
public class DropZone2 : ZoneBehaviour
{
    public void Awake()
    {
        ZoneSize = 2;
        InstantiateZone(0, ZoneSize, Deck, this);     //create and instantiate objects in scene in runtime
    }
}
