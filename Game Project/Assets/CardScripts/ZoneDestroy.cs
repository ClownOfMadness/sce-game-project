using UnityEngine;

//responsible for creating Destroy zone and deleting cards on contact, extension of ZoneBehaviour
public class ZoneDestroy : ZoneBehaviour
{
    void Awake()
    {
        Size = 1;       //max Zone size
    }
    void Update()   //(can be rewritten to be shorter but its refusing to work atm)
    {
        if (this.transform.childCount == this.Size)     //runs when card gets dropped in Zone
        {
            foreach (Transform child in this.transform) //finds the card (not needed since there's always one)
            {
                if (child.GetComponent<CardDrag>())     //checks if its card (not needed since there's always one card)
                {
                    Destroy(child.gameObject);          //deletes the card
                }
            }

        }
    }
}
