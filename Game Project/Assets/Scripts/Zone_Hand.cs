using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//responsible for creating and storing Hand zone, extension of ZoneBehaviour
public class Zone_Hand : Zone_Behaviour, IPointerEnterHandler, IPointerExitHandler
{
    //internal fields:
    private Data_Card Creation;     //card of Creation

    //external access:
    [HideInInspector] public Screen_Cards screen;
    private Card_Pool Pool; //open Card_Pool connection to use its functions

    //for development testing:
    [Header("- Fill Hand:")]
    public bool Fill = false;   //refill hand according to preset
    [Header("- Attack Cardmaster:")]
    public bool Damage = false; //simulate attacking the Cardmaster
    public enum decksList       //enum for preset menu
    {
        Presentation,
        CraftMenuPrompt,
        Units,
        Buildings,
        EmptyHand,
    }
    [Header("- Starting Deck Preset:")]
    public decksList Preset;    //enables picking a deck preset via the inspector

    //internal fields:
    private List<string> presenation = new List<string> //craft menu preset
    { "Flint","Stick","Clay","Meat" };
    private List<string> craftMenuPrompt = new List<string> //craft menu preset
    { "Flint","Stick","Flint","Stick","Iron","Stick","Steel","Stick" };
    private List<string> units = new List<string>           //unit placement preset
    { "Peasant","Peasant","Woodcutter (Flint)","Woodcutter (Flint)" };
    private List<string> buildings = new List<string>       //building placement preset
    { "Town Hall","Town Hall","House","Hut","Cabin","Bakery","Wood","Stick" };
    private List<string> emptyHand = new List<string>       //empty Hand preset
    {"Creation"};

    public void InstantiateZone()
    {
        Size = 8;               //max Zone size

        Pool = screen.Pool;
        Creation = screen.Creation;
        switch (Preset) //fill hand according to preset
        {
            case decksList.Presentation: AddCards(presenation); break;
            case decksList.EmptyHand: AddCards(emptyHand); break;
            case decksList.CraftMenuPrompt: AddCards(craftMenuPrompt); break;
            case decksList.Units: AddCards(units); break;
            case decksList.Buildings: AddCards(buildings); break;
            default: break;
        }
    }
    private void AddCards(List<string> deck)
    {
        for (int i = 0; i < this.Size && i < deck.Count; i++)
        {
            if (Pool.cards[Pool.FindCard(deck[i])].neverDiscovered)
            {
                Pool.cards[Pool.FindCard(deck[i])].neverDiscovered = false;
                //Debug.Log("New card found: " + deck[i]);
                screen.CardsDiscovered++;
            }
            screen.CreateObject(this.transform, deck[i]);
        }
    }
    public override void EventDrop(Card_Drag cardObject)    //card was dropped into Zone
    {
        if (cardObject.card.neverDiscovered)
        {
            cardObject.card.neverDiscovered = false;
            //Debug.Log("New card found: " + combination.name);
            screen.CardsDiscovered++;
        }
    }
    public Data_Card DamageDeck()    //lose a random card from hand, if Creation is lost game is lost
    {
        Card_Drag[] cardObjects = this.GetComponentsInChildren<Card_Drag>();
        Data_Card lostCard = null;
        int count = cardObjects.Length;
        if (count > 0)
        {
            int random = Random.Range(0, count);
            lostCard = cardObjects[random].card;

            Debug.Log("Cardmaster took damage! Card " + lostCard.name + " was lost.");
            Destroy(cardObjects[random].gameObject);
            if (lostCard != Creation && count <= 1)
                EmptyHand();
        }
        return lostCard;
    }
    public void RefreshZone() 
    {
        Card_Drag[] cardObjects = this.transform.GetComponentsInChildren<Card_Drag>(); //walkaround to ignore placeholders and only check cards
        if (cardObjects.Length < 1)
        {
            EmptyHand();
        }
        else if (cardObjects.Length > 1)
        {
            NotEmpty();
        }
    }
    public void EmptyHand()     //add Creation to hand
    {
        GameObject newCard = screen.CreateObject(this.transform, Creation); //create and instantiate object in zone
        newCard.GetComponent<LayoutElement>().ignoreLayout = true;
        Debug.Log("Creation was revealed.");
        screen.CreationRevealed = true;
    }
    public void NotEmpty()      //remove Creation
    {
        bool hidden = false;
        Card_Drag[] cardObjects = this.GetComponentsInChildren<Card_Drag>();
        if (cardObjects.Length > 1)
        {
            foreach (Card_Drag card in cardObjects)
                if (card.card == Creation)
                {
                    hidden = true;
                    GameObject placeholder = card.placeholder;    //get the card's placeholder in the Hand to delete it
                    Destroy(placeholder);
                    Destroy(card.gameObject);   //if Creation used to be in hand and card was added, remove Creation
                }
            if (hidden) //if Creation was found and removed
            {
                Debug.Log("Creation was hidden, you're safe for now.");
                screen.CreationRevealed = false;
            }
        }
    }
    public List<int> ExportDeck()           //used to save the game
    {
        List<int> export = new List<int>();
        Card_Drag[] cardObjects = this.GetComponentsInChildren<Card_Drag>();
        for (int i = 0; i < cardObjects.Length; i++)
            export.Add(cardObjects[i].card.code);
        return export;
    }
    public void ImportDeck(List<int> import)//used to load the game
    {
        List<string> importedDeck = new List<string>();
        for (int i = 0; i < import.Count && i < this.Size; i++) 
            importedDeck.Add(Pool.CodeToName(import[i]));
        if (importedDeck.Count > 0)
        {
            foreach (Transform cardObject in this.transform)    //clear whatever accidentally loaded by default into Hand
            {
                GameObject.Destroy(cardObject.gameObject);
            }
            AddCards(importedDeck);
        }
    }
}