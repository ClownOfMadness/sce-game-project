using UnityEngine;

//responsible for creating Destroy zone and deleting cards on contact, extension of ZoneBehaviour
public class Zone_Destroy : Zone_Behaviour
{
    private Data_Card Creation;
    [HideInInspector] public Screen_Cards screen;

    public void Start()
    {
        Size = 1;       //max Zone size

        Creation = screen.Creation;
    }
    public override void EventDrop(Card_Drag cardObject)
    {
        if (this.transform.childCount >= this.Size)     //failsafe - verifying that there's an object in the Zone
        {
            if (cardObject.card == Creation)
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
