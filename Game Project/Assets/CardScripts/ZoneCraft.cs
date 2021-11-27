using UnityEngine;

//responsible for creating Craft zone
public class ZoneCraft : MonoBehaviour
{
    public GameObject CardPrefab;   //type of prefab for Card (attached via Inspector)
    [HideInInspector]
    private int Size;               //Zone size

    void Awake()
    {
        Size = 2;                   //max Zone size
    }

    public void Update()
    {
        if (this.transform.childCount == this.Size)             //runs when two cards get dropped in Zone
        {
            CardPool pool = ScriptableObject.CreateInstance<CardPool>();        //open CardPool connection to use its functions
            CardDrag[] cardObjects = this.gameObject.transform.GetComponentsInChildren<CardDrag>();
            Card first = cardObjects[0].card;
            Card second = cardObjects[1].card;
            Card combination = pool.FindCombo(first, second);   //find fitting combination in deck pool
            if (combination)
            {
                Destroy(cardObjects[1].gameObject);
                cardObjects[0].GetComponent<CardDisplay>().AddCard(combination);
                string newName = combination.name;                              //add cards to objects + save the new card name (for displaying in Scene)
                cardObjects[0].name = string.Format("{0} (Card)", newName);     //updates name in scene
            }
            else
            {
                cardObjects[1].ReturnToHand();
            }
            cardObjects[0].ReturnToHand();
        }
    }
}

