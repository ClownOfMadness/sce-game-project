using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for creating and storing Craft zone, will need to be split into 2 zones at a later stage?
public class ZoneCraft : ZoneBehaviour
{
    public void Awake()
    {
        Size = 2;                   //max Zone size
        InstantiateZone(this);      //create and instantiate objects in scene in runtime
    }
}
