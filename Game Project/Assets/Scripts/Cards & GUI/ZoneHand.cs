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
    public void InstantiateZone(ZoneBehaviour Zone)
    {
        CardPool pool = ScriptableObject.CreateInstance<CardPool>();        //open CardPool connection to use its functions
        for (int i = Zone.transform.childCount; i < Zone.Size; i++)
        {
            GameObject newCard = Instantiate(CardPrefab, Zone.transform);   //create and instantiate objects in scene
            string newName = pool.FillObject(newCard);                      //add cards to objects + save the new card name (for displaying in Scene)
            newCard.name = string.Format("{0} (Card)", newName);            //updates name in scene
        }
    }
    public void EventDrop()
    {
    }
}


