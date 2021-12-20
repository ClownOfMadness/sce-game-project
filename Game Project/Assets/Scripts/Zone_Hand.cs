using System.Collections.Generic;
using UnityEngine;

//responsible for creating and storing Hand zone, extension of ZoneBehaviour
public class Zone_Hand : Zone_Behaviour
{
    [Header("Fill Hand:")]
    public bool Fill = false;
    [Header("Attack Cardmaster:")]
    public bool Damage = false;
    private Card_Pool Pool;
    private List<string> deck = new List<string>    //the starting cards that appear in Hand
    { "Town Hall","Town Hall","House","Hut","Cabin","Bakery","Wood","Stick" };

    void Awake()
    {
        Size = 8;               //max Zone size
        Pool = ScriptableObject.CreateInstance<Card_Pool>();        //open Card_Pool connection to use its functions;
        InstantiateZone();      //create and instantiate objects in scene in runtime
    }
    void Update()
    {
        if (Fill)
        {
            InstantiateZone();   //add objects to hand up to 8 in runtime
            Fill = false;
        }
        if (Damage)
        {
            DamageDeck();       //simulates the cardmaster being hit
            Damage = false;
        }
    }
    public void InstantiateZone()
    {
        for (int i = this.transform.childCount; i < this.Size; i++)
        {
            if (Card_Pool.cards[Pool.FindCard(deck[i])].neverDiscovered)
            {
                Card_Pool.cards[Pool.FindCard(deck[i])].neverDiscovered = false;
                //Debug.Log("New card found: " + deck[i]);
                Pool.discoveredTotal++;
            }
            GameObject newCard = Instantiate(CardPrefab, this.transform);   //create and instantiate objects in scene
            string newName = Pool.FillObject(newCard, deck[i]);             //add cards to objects + save the new card name (for displaying in Scene)
            newCard.name = string.Format("{0} (Card)", newName);            //updates name in scene
        }
    }
    public void DamageDeck()    //lose a random card from hand, if Creation is lost
    {
        int count = this.transform.childCount;
        if (count > 0)
        {
            int random = Random.Range(0, count);
            Card_Drag[] cardObjects = this.gameObject.transform.GetComponentsInChildren<Card_Drag>();
            Debug.Log("Cardmaster took damage! Card " + cardObjects[random].card.name + " was lost.");
            if (cardObjects[random].card == Pool.GetCard("Creation"))
            {
                Debug.Log("Game lost.");
            }
            else
            {
                Destroy(cardObjects[random].gameObject);
                if (count <= 1)
                    EmptyHand();    //spawn Creation if took too much damage
            }
        }
    }
    public void EmptyHand()     //add Creation to hand (not used yet)
    {
        GameObject newCard = Instantiate(CardPrefab, this.transform);   //create and instantiate objects in scene
        string newName = Pool.FillObject(newCard, "Creation");          //add cards to objects + save the new card name (for displaying in Scene)
        newCard.name = string.Format("{0} (Card)", newName);            //updates name in scene
    }
}


