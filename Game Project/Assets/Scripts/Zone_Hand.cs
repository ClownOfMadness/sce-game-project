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
    public enum decksList       //enum for preset menu
    {         
        CraftMenuPrompt,
        Units,
        Buildings,
        EmptyHand,
    }
    [Header("Starting Deck Preset:")]
    public decksList Preset;    //enables picking a deck preset via the inspector

    private List<string> deck = new List<string>();   //The cards that are in hand in the beginning

    private List<string> emptyHand = new List<string>();    //empty Hand

    private List<string> craftMenuPrompt = new List<string> //craft menu preset
    { "Flint","Stick","Flint Tool","Iron Tool","Wood","Stick","Steel Tool","Plank" };

    private List<string> units = new List<string> //craft menu preset
    { "Peasant","Peasant","Woodcutter (Flint)","Woodcutter (Flint)" };

    private List<string> buildings = new List<string>       //building testing preset
    { "Town Hall","Town Hall","House","Hut","Cabin","Bakery","Wood","Stick" };

    void Awake()
    {
        Size = 8;               //max Zone size
        Pool = ScriptableObject.CreateInstance<Card_Pool>();        //open Card_Pool connection to use its functions;
        switch (Preset)
        {
            case decksList.EmptyHand: deck = emptyHand; break;
            case decksList.CraftMenuPrompt: deck = craftMenuPrompt; break;
            case decksList.Units: deck = units; break;
            case decksList.Buildings: deck = buildings; break;
            default: deck = emptyHand; break;
        }
        InstantiateZone();      //create and instantiate objects in scene in runtime
    }
    void Update()
    {
        if (Fill)
        {
            NotEmpty();
            switch (Preset)
            {
                case decksList.EmptyHand: deck = emptyHand; break;
                case decksList.CraftMenuPrompt: deck = craftMenuPrompt; break;
                case decksList.Units: deck = units; break;
                case decksList.Buildings: deck = buildings; break;
                default: deck = emptyHand; break;
            }
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
        for (int i = this.transform.childCount; i < this.Size && i < deck.Count; i++) 
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
        if (this.transform.childCount == 0) //creates Creation if deck was empty
            EmptyHand();
    }
    public override void EventDrop(Card_Drag cardObject)
    {
        if (cardObject.card.neverDiscovered)
        {
            cardObject.card.neverDiscovered = false;
            //Debug.Log("New card found: " + combination.name);
            Pool.discoveredTotal++;
        }
        if(cardObject.card != Pool.GetCard("Creation"))
            NotEmpty();
    }
    public void DamageDeck()    //lose a random card from hand, if Creation is lost game is lost
    {
        Card_Drag[] cardObjects = this.GetComponentsInChildren<Card_Drag>();
        int count = cardObjects.Length;
        if (count > 0)
        {
            int random = Random.Range(0, count);
            if (cardObjects[random].card == Pool.GetCard("Creation"))
            {
                Debug.Log("Game lost.");
            }
            else
            {
                Debug.Log("Cardmaster took damage! Card " + cardObjects[random].card.name + " was lost.");
                Destroy(cardObjects[random].gameObject);
                if (count <= 1)
                    EmptyHand();    //spawn Creation if took too much damage
            }
        }
    }
    public void EmptyHand()     //add Creation to hand
    {
        GameObject newCard = Instantiate(CardPrefab, this.transform);   //create and instantiate objects in scene
        string newName = Pool.FillObject(newCard, "Creation");          //add cards to objects + save the new card name (for displaying in Scene)
        newCard.name = string.Format("{0} (Card)", newName);            //updates name in scene
    }
    public void NotEmpty()      //remove Creation
    {
        bool hidden = false;
        Card_Drag[] cardObjects = this.GetComponentsInChildren<Card_Drag>();
        foreach (Card_Drag card in cardObjects)
            if (card.card == Pool.GetCard("Creation"))
            {
                hidden = true;
                Destroy(card.gameObject);   //if Creation used to be in hand and card was added, remove Creation
            }
        if (hidden) //if Creation was found and removed
            Debug.Log("Creation wasd hidden, you're safe for now.");
    }
}


