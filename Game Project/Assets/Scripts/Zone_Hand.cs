using UnityEngine;

//responsible for creating and storing Hand zone, extension of ZoneBehaviour
public class Zone_Hand : Zone_Behaviour
{
    [Header("Fill Hand:")]
    public bool Fill = false;
    private Card_Pool Pool;

    void Awake()
    {
        Size = 8;               //max Zone size
        Pool = ScriptableObject.CreateInstance<Card_Pool>();        //open Card_Pool connection to use its functions
        InstantiateZone();      //create and instantiate objects in scene in runtime

    }
    void Update()
    {
        if (Fill)
        {
            InstantiateZone();   //add objects to hand up to 8 in runtime
            Fill = false;
        }
    }
    public void InstantiateZone()
    {
        for (int i = this.transform.childCount; i < this.Size; i++)
        {
            GameObject newCard = Instantiate(CardPrefab, this.transform);   //create and instantiate objects in scene
            string newName = Pool.FillObject(newCard);                      //add cards to objects + save the new card name (for displaying in Scene)
            newCard.name = string.Format("{0} (Card)", newName);            //updates name in scene
        }
    }
    public override void EventDrop(Card_Drag cardObject)             //failsafe for when card gets dropped back in hand
    {
    }
}


