using UnityEngine;

//responsible for creating Storage pt2 zone (the button) and adding dragged cards to storage, extension of ZoneBehaviour
public class Zone_StoragePt2 : Zone_Behaviour
{
    [HideInInspector] public Zone_Storage Storage;
    void Awake()
    {
        Size = 0;       //max Zone size
    }
    public override void EventDrop(Card_Drag cardObject)
    {
        if (Storage.AddToStorage(cardObject.card))
        {
            GameObject placeholder = cardObject.placeholder;    //get the card's placeholder in the Hand to delete it
            Destroy(placeholder);
            cardObject.screen.CheckEmpty();     //create Creation if last card was destroyed
            Destroy(cardObject.gameObject);
        }
    }
}

