using UnityEngine;
using UnityEngine.UI;

//responsible for switching panels and button related functions of Cards
public class ScreenCards : MonoBehaviour
{
    public Text Message;
    public GameObject Hand;
    public GameObject Craft;
    public GameObject Book;
    public GameObject Storage;
    public GameObject Map;
    public GameObject creativeButton;
    public GameObject destroyButton;
    [Header("Game State:")]
    public bool SkipLogin;
    private ZoneHand zHand;
    private ZoneCraft zCraft;
    private ZoneStorage zStorage;
    private ZoneBook zBook;
    [HideInInspector] public bool Placeable;

    void Start()    //initilizing in case something was off
    {
        zHand = Hand.transform.GetComponent<ZoneHand>();
        zCraft = Craft.transform.GetComponent<ZoneCraft>();
        zStorage = Storage.transform.GetComponent<ZoneStorage>();
        zBook = Book.transform.GetComponent<ZoneBook>();
        Message.gameObject.SetActive(false);

        Hand.SetActive(true);
        Craft.SetActive(true);
        Storage.SetActive(false);
        Book.SetActive(false);
        Map.SetActive(false);
        destroyButton.SetActive(true);
        Placeable = true;
        if (SkipLogin)  //for development testing
        {
            creativeButton.SetActive(true);
        }
        else
        {
            creativeButton.SetActive(false);
        }

    }
    public void SwitchCreative()
    {
        if (Storage.activeSelf == false)    //only try to open/close Book if Storage is closed
        {
            if (Craft.activeSelf)           //open book, close craft
            {
                Message.gameObject.SetActive(true);
                Message.text = "Creative game mode, click cards to add to your deck";
                Placeable = false;
                Craft.SetActive(false);
                Book.SetActive(true);
            }
            else
            {
                Message.gameObject.SetActive(false);
                Placeable = true;
                zBook.FirstPage();
                Book.SetActive(false);      //open craft, close book
                Craft.SetActive(true);
            }
        }
    }
    public void SwitchStorage()       //used when clicked on TownHall
    {
        if (Book.activeSelf == false)       //only try to open/close Storage if Book is closed
        {
            if (Storage.activeSelf)
            {
                CloseStorage();
            }
            else
            {
                Message.gameObject.SetActive(true);
                Message.text = "Town Storage";
                Placeable = false;
                Craft.SetActive(false);
                Storage.SetActive(true);
            }
        }
    }
    public void CloseStorage()      //used when clicked on Exit
    {
        Message.gameObject.SetActive(false);
        Placeable = true;
        Craft.SetActive(true);
        Storage.SetActive(false);
    }
    public void MoveFromHand(CardDrag pickedCard)  //move card from Hand zone to Hand on click
    {
        if (Craft.activeSelf)
        {
            if (pickedCard.transform.parent == Hand.transform)    //if card is in hand - move to craft
            {
                if (Craft.transform.childCount < zCraft.Size)
                {
                    Message.gameObject.SetActive(true);
                    Message.text = "Attempting to craft! Pick another card to combine";
                    pickedCard.transform.SetParent(Craft.transform);
                    if (Craft.transform.childCount == zCraft.Size)
                    {
                        zCraft.EventClick();
                    }
                }
            }
            else
            {
                CraftToHand(pickedCard);
            }
        }
        else if (Storage.activeSelf)
        {
            if (pickedCard.transform.parent == Hand.transform)    //if card is in hand - move to storage
            {
                if (zStorage.count < zStorage.Size)
                {
                    zStorage.AddToStorage(pickedCard.card);
                    Destroy(pickedCard.gameObject);
                }
            }
            else
            {
                ClickToHand(pickedCard);
            }
        }
    }
    public void CraftToHand(CardDrag card)  //move card from Craft zone to Hand on click
    {
        if (zCraft.success)
        {
            Message.gameObject.SetActive(true);
            Message.text = string.Format("Craft succeseful! Created {0}", card.card.name);
        }
        else
        {
            Message.gameObject.SetActive(true);
            Message.text = "Uh oh, craft didn't succeed";
        }
        card.gameObject.transform.SetParent(Craft.transform);
        //[insert timer]
        //Message.gameObject.SetActive(false); //happens after a timer
        card.gameObject.transform.SetParent(Hand.transform);
    }
    public void ClickToHand(CardDisplay pickedCard)  //adds card to hand based on what was picked in Book/Storage
    {
        if (Hand.transform.childCount < zHand.Size)
        {
            GameObject newCard = Instantiate(zHand.CardPrefab, Hand.transform);     //create and instantiate object in scene
            newCard.GetComponent<CardDrag>().AddCard(pickedCard.card);              //add cards to objects 
            newCard.name = string.Format("{0}", pickedCard.name);                   //update new card name (for displaying in Scene)
            if (Storage.activeSelf)
                zStorage.RemoveFromStorage(pickedCard);
        }
    }
}
