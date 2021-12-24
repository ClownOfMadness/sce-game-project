using System.Collections.Generic;
using UnityEngine;
using System.Linq;  //needed for ToList function

//responsible for creating and storing Storage zone, extension of ZoneBehaviour
public class Zone_Storage : Zone_Behaviour
{
    //attached via Inspector:
    public GameObject PagePrefab;       //type of prefab for Page (attached via Inspector)

    //public fields:
    [HideInInspector] public int pageSize;  //amount of cards allowed per page
    [HideInInspector] public int count;     //amount of full slots in the storage (serves as this zone's childCount)

    //internal fields:
    private GameObject Page;
    private List<Data_Card> slots;    //what is in the storage

    //external access:
    [HideInInspector] public Screen_Cards screen;
    [HideInInspector] public System_DayNight time;
    private Card_Pool Pool; //open Card_Pool connection to use its functions

    //for development testing:
    private bool skipDay = false;     
    
    public void InstantiateZone()
    {
        Size = 8;
        pageSize = 8;   //max Zone size

        time = FindObjectOfType<System_DayNight>();
        Pool = screen.Pool;

        skipDay = screen.TestSkipDay; //for development testing

        slots = new List<Data_Card>();
        count = 0;

        Page = Instantiate(PagePrefab, this.transform); //create and instantiate Page objects in zone
        Page.name = string.Format("Cards");             //new Page name (for displaying in Scene)
        for (int i = 0; i < pageSize; i++)
        {
            screen.CreateDisplay(Page.transform, i);
        }
    }
    public override void EventDrop(Card_Drag cardObject)    //card was dropped into Zone
    {
        if (AddToStorage(cardObject.card))
        {
            GameObject placeholder = cardObject.placeholder;    //get the card's placeholder in the Hand to delete it
            Destroy(placeholder);
            Destroy(cardObject.gameObject);
        }
    }
    public void RefreshZone()   //clear/add cards to zone visually
    {
        Size = pageSize-count;
        slots = slots.OrderBy(Card => Card.code).ToList();
        Card_Display[] cards = Page.gameObject.transform.GetComponentsInChildren<Card_Display>();
        for (int i = 0; i < count; i++)
        {
            cards[i].AddCard(slots[i]);
            cards[i].gameObject.name = string.Format("{0} (Card)", slots[i].name);  //updates name in scene
            if (time.isDay == false || skipDay) 
                cards[i].gameObject.GetComponent<CanvasGroup>().alpha = 1f;
            else
                cards[i].gameObject.GetComponent<CanvasGroup>().alpha = .6f;
        }
        for (int i = count; i < pageSize; i++)
        {
            cards[i].ClearCard();
            cards[i].gameObject.GetComponent<CanvasGroup>().alpha = .6f;
        }
    }
    public bool AddToStorage(Data_Card newCard)//adds card to zone at night or if given by Unit
    {
        if ((count < pageSize))
        {
            slots.Add(newCard);
            count++;
            RefreshZone();
            return true;
        }
        return false;
    }
    public void RemoveFromStorage(Card_Display pickedCard)  //removes card from storage on click
    {
        if (time.isDay == false || skipDay)
        {
            slots.Remove(pickedCard.card);
            count--;
            RefreshZone();
        }
    }
    public List<int> ExportDeck()           //will be used to save the game
    {
        List<int> export = new List<int>();
        for (int i = 0; i < count; i++)
            export.Add(slots[i].code);
        return export;
    }
    public void ImportDeck(List<int> import)//will be used to load the game
    {
        for (int i = 0; i < import.Count && i < pageSize; i++) 
            AddToStorage(Pool.GetCard(import[i]));
    }
}