using UnityEngine;
using UnityEngine.UI;

//responsible for switching panels and button related functions of Cards
public class Screen_Cards : MonoBehaviour
{
    public Text Message;
    public GameObject Hand;
    public GameObject Craft;
    public GameObject Unit;
    public GameObject Storage;
    public GameObject Book;

    public GameObject destroyButton;
    public GameObject creativeButton;
    public GameObject cardsButton;

    [Header("Game State:")]
    public bool SkipLogin;  //for development testing

    private Zone_Hand zHand;
    private Zone_Craft zCraft;
    private Zone_Unit zUnit;
    private Zone_Storage zStorage;
    private Zone_Book zBook;

    [HideInInspector] public Card_Pool Pool;
    [HideInInspector] public GameObject CraftMenu;
    [HideInInspector] public GameObject storageButton;

    [HideInInspector] public bool visibleMap;
    [HideInInspector] public bool UIDown;

    [HideInInspector] public GameObject selectedTile;   //updated by Player_Control
    [HideInInspector] public bool draggedBuilding;      //updated by Card_Drag
    [HideInInspector] public bool draggedUnit;          //updated by Card_Drag
    [HideInInspector] public Sprite draggedSprite;      //updated by Card_Drag

    void Start()    //initilizing in case something was off
    {
        zHand = Hand.transform.GetComponent<Zone_Hand>();
        zCraft = Craft.transform.GetComponent<Zone_Craft>();
        zUnit = Unit.transform.GetComponent<Zone_Unit>();
        zStorage = Storage.transform.GetComponent<Zone_Storage>();
        zBook = Book.transform.GetComponent<Zone_Book>();

        CraftMenu = zCraft.craftMenu;
        storageButton = zStorage.StorageButton;

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
        visibleMap = true;
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
        visibleMap = false;
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
        visibleMap = false;
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
        visibleMap = true;
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
        if (zStorage.time.isDay)
            TopMessage("Town Storage currently closed! Come back at night");
        else
            TopMessage("Town Storage, choose your deck for the next day");
        visibleMap = false;
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
        visibleMap = true;
        Time.timeScale = 1f;    //unpause game
    }
    public void CardClick(Card_Drag pickedCard)             //move Card_Drag between zones on click
    {
        //if card is in hand - move it
        if (pickedCard.transform.parent == Hand.transform)  
        {
            if (visibleMap)                   //move to Craft
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
            else if (Storage.activeSelf)    //move to Storage
            {
                if (zStorage.AddToStorage(pickedCard.card, false))
                {
                    Destroy(pickedCard.gameObject);
                    CheckEmpty();
                }
            }
        }
        //if card isn't in hand - try to move it to hand
        else if (Hand.transform.childCount < zHand.Size)    //if there's space in Hand
        {
            if (pickedCard.card.neverDiscovered)
            {
                pickedCard.card.neverDiscovered = false;
                //Debug.Log("New card found: " + combination.name);
                Pool.discoveredTotal++;
            }
            if (pickedCard.transform.parent == CraftMenu.transform)     //if card is in Craft Menu prompt - move to hand
            {
                pickedCard.gameObject.transform.SetParent(Hand.transform);
                zCraft.ClearMenu();
                visibleMap = true;
            }
            else if (pickedCard.transform.parent == Craft.transform)         //if card is in Craft - move to hand
            {
                Craft.SetActive(false);
                pickedCard.gameObject.transform.SetParent(Hand.transform);
            }
            else if (pickedCard.transform.parent == Unit.transform)     //if card is in Unit prompt - move to hand
            {
                zUnit.FreeUnit();
                pickedCard.gameObject.transform.SetParent(Hand.transform);
                zHand.NotEmpty();
            }
            Message.gameObject.SetActive(false);
        }
    }
    public void DisplayCardClick(Card_Display pickedCard)   //add Card_Display to hand based on what was picked in Book/Storage
    {
        if (CreateInHand(pickedCard.card))
        {
            if (Storage.activeSelf)
                zStorage.RemoveFromStorage(pickedCard);
        }
    }
    public bool AddGathered(Data_Unit unit, bool gathered)  //add card from Unit to Hand (if there's space)
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
            newCard.name = string.Format("{0} (Card)", pickedCard.name);            //update new card name (for displaying in Scene)
            zHand.NotEmpty();   //if Creation was in hand, remove it
            return true;
        }
        return false;
    }
    public void CheckEmpty()     //add Creation to hand if empty
    {
        if (Hand.transform.childCount <= 1)
        {
            zHand.EmptyHand();
        }
    }
}
