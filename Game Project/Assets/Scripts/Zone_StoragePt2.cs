using UnityEngine;

//responsible for creating Storage pt2 zone (the button) and adding dragged cards to storage, extension of ZoneBehaviour
public class Zone_StoragePt2 : Zone_Behaviour
{
    [HideInInspector] public Zone_Storage Storage;
    [HideInInspector] public bool ignoreTime;
    void Awake()
    {
        Size = 1;       //max Zone size
        ignoreTime = false;
    }
    public override void EventDrop(Card_Drag cardObject)
    {
        if (Storage.AddToStorage(cardObject.card, ignoreTime))
        {
            GameObject placeholder = cardObject.placeholder;    //get the card's placeholder in the Hand to delete it
            Destroy(placeholder);
            cardObject.screen.CheckEmpty();     //create Creation if last card was destroyed
            Destroy(cardObject.gameObject);
        }
        ignoreTime = false; //set back to default
    }
}

