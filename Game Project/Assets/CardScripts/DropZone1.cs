using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for creating and storing Zone1
public class DropZone1 : ZoneBehaviour
{
    [Header("Fill Hand (bugged, reshuffles instead):")]
    public bool Fill = false;

    public void Awake()
    {
        ZoneSize = 8;
        InstantiateZone(0, ZoneSize, Deck, this);  //create and instantiate objects in scene in runtime
    }
    public void Update()
    {
        if (Fill)
        {
            //need to implement Deck updates in Zones first, currently reshuffles cards instead of adding
            InstantiateZone(Deck.Length, ZoneSize, Deck, this);  //add objects to hand in scene in runtime
            Fill = false;

        }
    }

}


