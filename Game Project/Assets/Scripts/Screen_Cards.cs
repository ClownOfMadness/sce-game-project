using UnityEngine;
using UnityEngine.UI;

//responsible for switching panels and button related functions of Cards
public class Screen_Cards : MonoBehaviour
{
    public Text Message;
    public GameObject Hand;
    public GameObject Craft;
    public GameObject Storage;
    public GameObject Book;
    public GameObject Unit;
    public GameObject destroyButton;
    public GameObject creativeButton;
    public GameObject storageButton;
    public GameObject cardsButton;

    [Header("Game State:")]
    public bool SkipLogin;  //for development testing

    private Zone_Hand zHand;
    private Zone_Craft zCraft;
    private Zone_Storage zStorage;
    private Zone_Book zBook;
    private Zone_Unit zUnit;

    private Card_Pool Pool;

    [HideInInspector] public bool canCraft;
    [HideInInspector] public bool UIDown;

    [HideInInspector] public GameObject selectedTile;   //updated by Player_Control
    [HideInInspector] public bool draggedBuilding;      //updated by Card_Drag
    [HideInInspector] public bool draggedUnit;          //updated by Card_Drag
    [HideInInspector] public Sprite draggedSprite;      //updated by Card_Drag

    void Start()    //initilizing in case something was off
    {
        zHand = Hand.transform.GetComponent<Zone_Hand>();
        zCraft = Craft.transform.GetComponent<Zone_Craft>();
        zStorage = Storage.transform.GetComponent<Zone_Storage>();
        zBook = Book.transform.GetComponent<Zone_Book>();
        zUnit = Unit.transform.GetComponent<Zone_Unit>();

        Pool = ScriptableObject.CreateInstance<Card_Pool>();        //open Card_Pool connection to use its functions;

        for (int i = 0; i < Card_Pool.count; i++)                   //returning all fields in cards back to default 
            Card_Pool.cards[i].neverDiscovered = true;              //done here instead of Card_Pool to ensure that it only happens once

        Unit.SetActive(false);

        CloseUI();

        if (SkipLogin || Screen_Login.IsLogin)
        {
            creativeButton.SetActive(true);
        }
        else
        {
            creativeButton.SetActive(false);
        }
        storageButton.SetActive(true);
        cardsButton.SetActive(true);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) //can close and open "Hand" with keyboard as well
        {
            SwitchCards();
        }
        if (Input.GetKeyDown(KeyCode.C)) //can close and open "Creative" with keyboard as well
        {
            SwitchCreative();
        }
        if (Input.GetKeyDown(KeyCode.S)) //can close and open "Storage" with keyboard as well
        {
            SwitchStorage();
        }
    }
    public void TopMessage(string text)
    {
        Message.gameObject.SetActive(true);
        Message.text = text;
    }
    public void SwitchCards()   //close/open all card related stuff
    {
        if (UIDown)
        {
            OpenUI();
        }
        else
        {
            CloseUI();
        }
    }
    private void OpenUI()
    {

        Hand.SetActive(true);
        destroyButton.SetActive(true);
        canCraft = true;
        UIDown = false;
    }
    private void CloseUI()
    {
        Message.gameObject.SetActive(false);

        Hand.SetActive(false);
        Craft.SetActive(false);
        Storage.SetActive(false);
        Book.SetActive(false);
        destroyButton.SetActive(false);
        canCraft = false;
        UIDown = true;
    }
    public void SwitchCreative()//close/open Book
    {

        if (Book.activeSelf)
        {
            CloseBook();
        }
        else
        {
            OpenBook();
        }
    }
    private void OpenBook()
    {
        TopMessage("Creative game mode, click cards to add to your deck");
        canCraft = false;
        Storage.SetActive(false);
        Book.SetActive(true);
        Hand.SetActive(true);
        destroyButton.SetActive(true);

        UIDown = false;
        Time.timeScale = 0f;    //pause game
    }
    private void CloseBook()
    {
        Message.gameObject.SetActive(false);

        zBook.FirstPage();
        Book.SetActive(false);
        canCraft = true;
        Time.timeScale = 1f;    //unpause game
    }
    public void SwitchStorage() //close/open Storage
    {
        if (Storage.activeSelf)
        {
            CloseStorage();
        }
        else
        {
            OpenStorage();
        }
    }
    public void OpenStorage()
    {
        Storage.SetActive(true);
        zStorage.RefreshZone();
        if (zStorage.night.isDay)
            TopMessage("Town Storage currently closed! Come back at night");
        else
            TopMessage("Town Storage, choose your deck for the next day");
        canCraft = false;
        Book.SetActive(false);
        Hand.SetActive(true);
        destroyButton.SetActive(true);

        UIDown = false;
        Time.timeScale = 0f;    //pause game
    }
    public void CloseStorage()
    {
        Message.gameObject.SetActive(false);

        Storage.SetActive(false);
        canCraft = true;
        Time.timeScale = 1f;    //unpause game
    }
    public void CardClick(Card_Drag pickedCard)     //move card between Hand zone and Craft on click
    {
        if (canCraft)
        {
            if (pickedCard.transform.parent == Hand.transform)  //if card is in hand - move to craft
            {
                Craft.SetActive(true);
                if (Craft.transform.childCount < zCraft.Size)
                {
                    TopMessage("Attempting to craft! Pick another card to combine");
                    pickedCard.transform.SetParent(Craft.transform);
                    if (Craft.transform.childCount == zCraft.Size)
                    {
                        zCraft.EventClick();
                    }
                }
            }
            else if (Hand.transform.childCount < zHand.Size)    //make sure there's space in Hand
            {

                if (pickedCard.transform.parent == Craft.transform)    //if card is in craft - move to hand
                {
                    CraftToHand(pickedCard);
                }
                else if (pickedCard.transform.parent == Unit.transform)    //if card is in Unit - move to hand
                {
                    UnitToHand(pickedCard);
                }
            }
        }
        else if (Storage.activeSelf)
        {
            if (pickedCard.transform.parent == Hand.transform)  //if card is in hand - move to storage
            {
                if (zStorage.AddToStorage(pickedCard.card, false))
                {
                    Destroy(pickedCard.gameObject);
                }
            }
            else    //if card is in storage - move to hand
            {
                ClickToHand(pickedCard);
            }
        }
    }
    public void CraftToHand(Card_Drag card)         //move card from Craft zone to Hand on click
    {
        if (zCraft.success)
        {
            TopMessage(string.Format("Craft succeseful! Created {0}", card.card.name));
        }
        else
        {
            TopMessage("Uh oh, craft didn't succeed");
        }
        //card.gameObject.transform.SetParent(Craft.transform);
        //[insert timer]
        card.gameObject.transform.SetParent(Hand.transform);
        Craft.SetActive(false);
    }
    public void UnitToHand(Card_Drag card)          //move card from Craft zone to Hand on click
    {
        zUnit.FreeUnit();
        card.gameObject.transform.SetParent(Hand.transform);
        Message.gameObject.SetActive(false);
    }
    public void ClickToHand(Card_Display pickedCard)//adds card to hand based on what was picked in Book/Storage
    {
        if (Book.activeSelf)    //only run if book is open
        {
            CreateInHand(pickedCard.card);
        }
        else if (Storage.activeSelf && zStorage.night.isDay == false) //only run if storage is open at night
        {
            if (CreateInHand(pickedCard.card))
            {
                zStorage.RemoveFromStorage(pickedCard);
            }
        }
    }
    private bool CreateInHand(Data_Card pickedCard)
    {
        if (Hand.transform.childCount < zHand.Size)
        {
            if (pickedCard.neverDiscovered)
            {
                pickedCard.neverDiscovered = false;
                //Debug.Log("New card found: " + pickedCard.name);
                Pool.discoveredTotal++;
            }
            GameObject newCard = Instantiate(zHand.CardPrefab, Hand.transform);     //create and instantiate object in scene
            newCard.GetComponent<Card_Drag>().AddCard(pickedCard);                  //add cards to objects 
            newCard.name = string.Format("{0} (Card)", pickedCard.name);                   //update new card name (for displaying in Scene)
            zHand.NotEmpty();   //if Creation was in hand, remove it
            return true;
        }
        return false;
    }
    public bool AddGathered(Data_Unit unit, bool gathered)   //add card from Unit to Hand (if there's space)
    {
        Data_Card pickedCard;
        Data_Unit waitingUnit;
        if (gathered)
        {
            pickedCard = unit.card;
            waitingUnit = unit;
        }
        else
        {
            pickedCard = unit.unitCard;
            waitingUnit = null;
        }
        if (CreateInHand(pickedCard))
        {
            return true;    //unit doesn't need to wait
        }
        else    //no space in Hand
        {
            OpenUI();
            TopMessage("New card arrived arrived at the Town Hall! Choose what to do with it next");
            Unit.SetActive(true);
            zUnit.CardAdded(pickedCard, waitingUnit);
            return false; //unit needs to wait
        }
    }
    public void CheckEmpty()     //add Creation to hand if empty
    {
        if (Hand.transform.childCount <= 1)
        {
            zHand.EmptyHand();
        }
    }
}
