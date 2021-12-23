using UnityEngine;

//responsible for creating Storage pt2 zone (the button) and adding dragged cards to storage, extension of ZoneBehaviour
public class Zone_StoragePt2 : Zone_Behaviour
{
    [HideInInspector] public Zone_Storage Storage;

    public void Start()
    {
        Size = 0;       //max Zone size
    }
    public override void EventDrop(Card_Drag cardObject)
    {
        Storage.EventDrop(cardObject);
    }
}

