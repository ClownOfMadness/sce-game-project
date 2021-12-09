using UnityEngine;

//responsible for creating Destroy zone and deleting cards on contact, extension of ZoneBehaviour
public class Zone_Destroy : Zone_Behaviour
{
    void Awake()
    {
        Size = 1;       //max Zone size
    }
    public override void EventDrop(Card_Drag cardObject)
    {
        if (this.transform.childCount == this.Size)     //failsafe - verifying that there's an object in the Zone
        {
            Debug.Log(cardObject.nameText);
            GameObject placeholder = cardObject.placeholder;      //get the card's placeholder in the Hand to delete it
            Destroy(placeholder);
            Destroy(cardObject.gameObject);     
        }
    }
}
