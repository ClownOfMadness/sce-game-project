using UnityEngine;

//responsible for creating Craft zone
public class Zone_Craft : MonoBehaviour
{
    public GameObject CardPrefab;           //type of prefab for Card (attached via Inspector)
    [HideInInspector] public int Size;      //Zone size
    [HideInInspector] public bool success;

    void Awake()
    {
        Size = 2;   //max Zone size
    }

    public void EventClick()
    {
        if (this.transform.childCount == this.Size)     //failsafe - verifying that there's two objects in the Zone
        {
            Card_Drag[] cardObjects = this.gameObject.transform.GetComponentsInChildren<Card_Drag>();
            Data_Card first = cardObjects[0].card;
            Data_Card second = cardObjects[1].card;
            Data_Card combination = Card_Pool.FindCombo(first, second);   //find fitting combination in deck pool
            if (combination)
            {
                Debug.Log("Card found " + combination.name);
                success = true;
                Destroy(cardObjects[1].gameObject);
                cardObjects[0].AddCard(combination);
                string newName = combination.name;                              //add cards to objects + save the new card name (for displaying in Scene)
                cardObjects[0].name = string.Format("{0} (Card)", newName);     //updates name in scene
            }
            else
            {
                success = false;
                cardObjects[1].ReturnToHand();
            }
            cardObjects[0].ReturnToHand();
        }
    }
}

