using UnityEngine;

//responsible for creating Destroy zone and deleting cards on contact, extension of ZoneBehaviour
public class ZoneDestroy : ZoneBehaviour
{
    private static GameObject placeholder;
    public void Awake()
    {
        Size = 2;               //max Zone size
    }

    public void Update()
    {
        if (this.transform.childCount == this.Size)     //runs when card gets dropped in Zone
        {
            foreach (Transform child in this.transform) //finds the card
            {
                if (child.GetComponent<CardDrag>())
                {
                    Destroy(child.gameObject);          //deletes the card
                }
            }
        }
    }
}
