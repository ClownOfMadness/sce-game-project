using UnityEngine;
using UnityEngine.EventSystems;

//responsible for creating Destroy zone and deleting cards on contact, extension of ZoneBehaviour
public class Zone_Destroy : Zone_Behaviour
{
    //internal fields:
    private Data_Card Creation;

    //external access:
    [HideInInspector] public Screen_Cards screen;

    public void InstantiateZone()
    {
        Size = 1;       //max Zone size

        Creation = screen.Creation;
    }
    public override void EventDrop(Card_Drag cardObject)    //card was dropped into Zone
    {
        if (this.transform.childCount >= this.Size)     //failsafe - verifying that there's an object in the Zone
        {
            if (cardObject.card == Creation)
            {
                screen.Game.gameLost = true;
                Debug.Log("Game lost.");
            }
            GameObject placeholder = cardObject.placeholder;    //get the card's placeholder in the Hand to delete it
            Destroy(placeholder);
            Destroy(cardObject.gameObject);     
        }
    }
}
