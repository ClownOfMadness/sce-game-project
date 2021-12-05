using System.Collections.Generic;
using UnityEngine;
using System.Linq;  //needed for ToList function

//responsible for creating and storing Storage zone
public class Zone_Storage : MonoBehaviour
{
    public GameObject PagePrefab;       //type of prefab for Page (attached via Inspector)
    public GameObject CardPrefab;       //type of prefab for Card (attached via Inspector)
    //public GameObject exitButton;     //removed for now
    [HideInInspector] private GameObject Page;
    [HideInInspector] public int Size;  //Page size
    [HideInInspector] private List<Data_Card> slots;  //what is in the storage
    [HideInInspector] public int count;  //amount of full slots in the storage

    void Start()
    {
        Size = 8;           //max Zone size
        InstantiateZone();
        slots = new List<Data_Card>();
        count = 0;
    }
    private void InstantiateZone()
    {
        Page = Instantiate(PagePrefab, this.transform);           //create and instantiate Page objects in scene
        Page.name = string.Format("Cards");              //new Page name (for displaying in Scene)
        for (int i = count; i < this.Size; i++) 
        {
            GameObject newCard = Instantiate(CardPrefab, Page.transform);   //create and instantiate objects in scene
            newCard.GetComponent<Card_Display>().ClearCard();
            newCard.name = string.Format("Slot {0}", i);                    //updates name in scene
            newCard.transform.localScale -= new Vector3((CardPrefab.transform.localScale.x) / 3, (CardPrefab.transform.localScale.y) / 3, 0);
            newCard.GetComponent<CanvasGroup>().alpha = .6f;
        }
        //exitButton.transform.SetAsLastSibling();             //move button to the far right
    }
    private void RefreshZone()
    {
        slots = slots.OrderBy(Card => Card.code).ToList();
        Card_Display[] cards = Page.gameObject.transform.GetComponentsInChildren<Card_Display>();
        for (int i = 0; i < count; i++)
        {
            cards[i].AddCard(slots[i]);
            cards[i].gameObject.name = string.Format("{0} (Card)", slots[i].name);                    //updates name in scene
            cards[i].gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        }
        for (int i = count; i < this.Size; i++)
        {
            cards[i].ClearCard();
        }
    }
    public void AddToStorage(Data_Card newCard)
    {
        slots.Add(newCard);
        slots.OrderBy(Card=>Card.code);
        count++;
        RefreshZone();
    }
    public void RemoveFromStorage(Card_Display pickedCard)
    {
        slots.Remove(pickedCard.card);
        count--;
        RefreshZone();
    }
}


