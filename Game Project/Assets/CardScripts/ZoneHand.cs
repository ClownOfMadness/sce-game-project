using UnityEngine;

//responsible for creating and storing Hand zone, extension of ZoneBehaviour
public class ZoneHand : ZoneBehaviour
{
    [Header("Fill Hand:")]
    public bool Fill = false;

    void Awake()
    {
        Size = 8;               //max Zone size
        InstantiateZone(this);  //create and instantiate objects in scene in runtime
    }
    void Update()
    {
        if (Fill)
        {
            InstantiateZone(this);   //add objects to hand up to 8 in runtime
            Fill = false;
        }
    }

}


