using UnityEngine;

//responsible for creating and storing Craft zone, will need to be split into 2 zones at a later stage?
public class ZoneCraft : ZoneBehaviour
{
    public void Awake()
    {
        Size = 2;               //max Zone size
    }

    public void Update()
    {
        if (this.transform.GetComponentsInChildren<CardDrag>().Length == this.Size)             //runs when two cards get dropped in Zone
        {
            CardPool pool = ScriptableObject.CreateInstance<CardPool>();        //open CardPool connection to use its functions
            CardDrag[] cardObjects = this.gameObject.transform.GetComponentsInChildren<CardDrag>();
            Card first = cardObjects[0].card;
            Card second = cardObjects[1].card;
            Debug.Log("Card 1 is " + first.name);
            Debug.Log("Card 2 is " + second.name);
            Card combination = pool.FindCombo(first, second);   //find fitting combination in deck pool
            if (combination)
            {
                Debug.Log("New card is " + combination.name);
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
    public void CreativeButton()
    {
        if (!this.gameObject.activeSelf)         //if inactive turn on
            this.gameObject.SetActive(true);
        else
        {
            this.gameObject.SetActive(false);  //if active turn off
        }
    }
}

