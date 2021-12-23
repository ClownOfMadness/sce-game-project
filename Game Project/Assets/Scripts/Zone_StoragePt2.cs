using UnityEngine;

//responsible for creating Storage pt2 zone (the button) and adding dragged cards to storage, extension of ZoneBehaviour
public class Zone_StoragePt2 : Zone_Behaviour
{ 
    //internal fields:
    private Zone_Storage zStorage;

    //external access:
    [HideInInspector] public Screen_Cards screen;

    public void InstantiateZone()
    {
        Size = 1; //max Zone size

        zStorage = screen.Storage.GetComponent<Zone_Storage>();
    }
    public override void EventDrop(Card_Drag cardObject)    //card was dropped into Zone
    {
        zStorage.EventDrop(cardObject);
    }
}

