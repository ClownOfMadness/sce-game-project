using UnityEngine;

//responsible for creating and storing Hand zone, extension of ZoneBehaviour
public class ZoneHand : ZoneBehaviour
{
    [Header("Fill Hand:")]
    public bool Fill = false;
    private CardPool Pool;

    void Awake()
    {
        Size = 8;               //max Zone size
        Pool = ScriptableObject.CreateInstance<CardPool>();        //open CardPool connection to use its functions
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
    public void InstantiateZone(ZoneBehaviour Zone)
    {
        for (int i = Zone.transform.childCount; i < Zone.Size; i++)
        {
            GameObject newCard = Instantiate(CardPrefab, Zone.transform);   //create and instantiate objects in scene
            string newName = Pool.FillObject(newCard);                      //add cards to objects + save the new card name (for displaying in Scene)
            newCard.name = string.Format("{0} (Card)", newName);            //updates name in scene
        }
    }
    public override void EventDrop(CardDrag cardObject)             //failsafe for when card gets dropped back in hand
    {
    }
}


