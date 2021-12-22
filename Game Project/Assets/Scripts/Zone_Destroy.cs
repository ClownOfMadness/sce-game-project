using UnityEngine;

//responsible for creating Destroy zone and deleting cards on contact, extension of ZoneBehaviour
public class Zone_Destroy : Zone_Behaviour
{
    private Card_Pool Pool;
    void Awake()
    {
        Size = 1;       //max Zone size
        Pool = ScriptableObject.CreateInstance<Card_Pool>();    //open Card_Pool connection to use its functions;
    }
    public override void EventDrop(Card_Drag cardObject)
    {
        if (this.transform.childCount >= this.Size)     //failsafe - verifying that there's an object in the Zone
        {
            if (cardObject.card == Pool.GetCard("Creation"))
            {
                Debug.Log("Game lost.");
            }
            cardObject.screen.draggedBuilding = false;  //close selectedTile updates
            cardObject.screen.draggedUnit = false;      //close selectedTile updates
            GameObject placeholder = cardObject.placeholder;    //get the card's placeholder in the Hand to delete it
            Destroy(placeholder);
            Destroy(cardObject.gameObject);     
        }
    }
}
