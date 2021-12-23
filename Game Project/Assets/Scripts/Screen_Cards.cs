using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//responsible for switching panels and button related functions of Cards
public class Screen_Cards : MonoBehaviour
{
    //attached via Inspector:
    //[Header("Panels:")]
    public Text Message;
    public GameObject Hand;
    //public GameObject CraftMenu;
    public GameObject Craft;
    public GameObject Unit;
    public GameObject Storage;
    public GameObject Book;

    //[Header("Buttons:")]
    public GameObject destroyButton;
    public GameObject creativeButton;
    public GameObject storageButton;
    public GameObject cardsButton;

    //[Header("Prefabs:")]
    //public GameObject CardObject;    //type of prefab for draggable Cards
    //public GameObject CardDisplay;   //type of prefab for draggable Cards

    //blocking cards from acting weird:
    [HideInInspector] public bool visibleMap;
    [HideInInspector] public bool UIDown;

    //blocking zones from acting weird:
    [HideInInspector] public bool MenuUp;
    [HideInInspector] public bool CraftUp;
    [HideInInspector] public bool UnitUp;
    [HideInInspector] public bool StorageUp;
    [HideInInspector] public bool BookUp;

    //private Zones:
    private Zone_Hand zHand;
    private Zone_Craft zCraft;
    private Zone_Unit zUnit;
    private Zone_Storage zStorage;
    private Zone_Book zBook;
    private Zone_Destroy zDestroy;
    private Zone_StoragePt2 zStoragePt2;

    //notification messages:
    private string UnitMsg = "New card arrived arrived at the Town Hall! Choose what to do with it next";
    private string CraftMsg = "Attempting to craft! Pick another card to combine";
    private string CreativeMsg = "Creative game mode, click cards to add to your deck";
    private string StorageDayMsg = "Town Storage currently closed! You can only take cards out at night";
    private string StorageNightMsg = "Town Storage, choose your deck for the next day";

    //sharing with other scripts:
    [HideInInspector] public Card_Pool Pool;
    [HideInInspector] public GameObject CraftMenu; //remove when scene gets updated and enable
    [HideInInspector] public GameObject CardObject; //remove when scene gets updated and enable 
    [HideInInspector] public GameObject CardDisplay; //remove when scene gets updated and enable
    [HideInInspector] public Data_Card Creation;

    //placement on map:
    [HideInInspector] public GameObject selectedTile;   //updated by Player_Control
    [HideInInspector] public bool draggedBuilding;      //updated by Card_Drag
    [HideInInspector] public bool draggedUnit;          //updated by Card_Drag
    [HideInInspector] public Sprite draggedSprite;      //updated by Card_Drag

    //for development testing:
    [Header("Game State:")]
    public bool SkipLogin;
    [Header("Simulate night:")]
    public bool skipDay;

    void Awake()    //initilizing the entire cards' canvas
    {
        SetZones();

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

        MenuUp = false;
        CraftUp = false;
        UnitUp = false;
        StorageUp = false;
        BookUp = false;

        visibleMap = true;
        CloseUI();  //game starts with UI closed
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
    private void SetZones()     //handles everything to do with initiating zones and getting access to their scripts
    {
        Pool = ScriptableObject.CreateInstance<Card_Pool>();    //open Card_Pool connection to use its functions;
        Creation = Pool.GetCard("Creation");

        //finding all Zones
        zHand = Hand.transform.GetComponent<Zone_Hand>();
        zCraft = Craft.transform.GetComponent<Zone_Craft>();
        zUnit = Unit.transform.GetComponent<Zone_Unit>();
        zStorage = Storage.transform.GetComponent<Zone_Storage>();
        zBook = Book.transform.GetComponent<Zone_Book>();
        zDestroy = destroyButton.transform.GetComponent<Zone_Destroy>();
        zStoragePt2 = storageButton.transform.GetComponent<Zone_StoragePt2>();

        CraftMenu = zCraft.craftMenu; //[remove when scene gets updates]
        //zStoragePt2.Storage = zStorage;
        CardObject = zHand.CardPrefab; //[remove when scene gets updates]
        CardDisplay = zStorage.CardPrefab; //[remove when scene gets updates]

        //giving Screen_Cards to the Zones so they can access stuff
        zHand.screen = this;
        zCraft.screen = this;
        zUnit.screen = this;
        zStorage.screen = this;
        zBook.screen = this;
        zDestroy.screen = this;
        zStoragePt2.screen = this;

        //running Zones so they're ready to recieve input
        zHand.InstantiateZone();
        zCraft.InstantiateZone();
        zUnit.InstantiateZone();
        zStorage.InstantiateZone();
        zBook.InstantiateZone();
        zDestroy.InstantiateZone();
        zStoragePt2.InstantiateZone();
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
        if (MenuUp)
            CraftMenu.SetActive(true);
        else if (CraftUp)
            Craft.SetActive(true);
        else if (UnitUp)
            Unit.SetActive(true);
        else if (StorageUp)
            Storage.SetActive(true);
        else if (BookUp)
            Book.SetActive(true);
        if (MenuUp || CraftUp || UnitUp || StorageUp || BookUp)
        {
            Message.gameObject.SetActive(true);
            visibleMap = false; 
        }
        else
            visibleMap = true;
        destroyButton.SetActive(true);
        UIDown = false;
    }
    private void CloseUI()
    {
        Message.gameObject.SetActive(false);
        Hand.SetActive(false);
        CraftMenu.SetActive(false);
        Craft.SetActive(false);
        Unit.SetActive(false);
        Storage.SetActive(false);
        Book.SetActive(false);
        destroyButton.SetActive(false);
        UIDown = true;

        Time.timeScale = 1f;    //unpause game
    }
    public void SwitchCreative()//close/open Book
    {
        if (BookUp)
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
        if (visibleMap || StorageUp) 
        {
            TopMessage(CreativeMsg);
            visibleMap = false;
           
            Storage.SetActive(false);
            StorageUp = false;

            BookUp = true;
            OpenUI();

            Time.timeScale = 0f;    //pause game
        }
    }
    private void CloseBook()
    {
        Message.gameObject.SetActive(false);

        zBook.RefreshZone();
        Book.SetActive(false);
        visibleMap = true;
        BookUp = false;
        Time.timeScale = 1f;    //unpause game
        CheckUnit();
    }
    public void SwitchStorage() //close/open Storage
    {
        if (StorageUp)
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
        if (visibleMap || BookUp) 
        {
            if (zStorage.time.isDay)
                TopMessage(StorageDayMsg);
            else
                TopMessage(StorageNightMsg);
            visibleMap = false;

            Book.SetActive(false);
            BookUp = false;

            zStorage.RefreshZone();

            StorageUp = true;
            OpenUI();

            Time.timeScale = 0f;    //pause game
        }
    }
    public void CloseStorage()
    {
        Message.gameObject.SetActive(false);

        Storage.SetActive(false);
        visibleMap = true;
        StorageUp = false;
        Time.timeScale = 1f;    //unpause game
        CheckUnit();
    }
    public void CardClick(Card_Drag pickedCard) //move Card_Drag between zones on click
    {
        //if card is in hand - move it
        if (pickedCard.transform.parent == Hand.transform)  
        {
            if (pickedCard.card != Creation)
            {
                if (visibleMap || CraftUp) //move to Craft
                {
                    if (Craft.transform.childCount < zCraft.Size)
                    {
                        TopMessage(CraftMsg);
                        visibleMap = false;
                        Craft.SetActive(true);
                        CraftUp = true;
                        pickedCard.transform.SetParent(Craft.transform);
                        if (Craft.transform.childCount == zCraft.Size)
                        {
                            zCraft.RefreshZone();
                            if (CraftMenu.activeSelf)
                            {
                                CraftUp = false;
                                MenuUp = true;
                            }
                        }
                    }
                }
                else if (Storage.activeSelf)        //move to Storage
                {
                    if (zStorage.AddToStorage(pickedCard.card))
                    {
                        pickedCard.transform.SetParent(Storage.transform);
                        Destroy(pickedCard.gameObject);
                    }
                }
                zHand.RefreshZone();
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
                MenuUp = false;
                CheckUnit();
            }
            else if (pickedCard.transform.parent == Craft.transform)   //if card is in Craft - move to hand
            {
                Craft.SetActive(false);
                pickedCard.gameObject.transform.SetParent(Hand.transform);
                visibleMap = true;
                CraftUp = false;
                CheckUnit();
            }
            else if (pickedCard.transform.parent == Unit.transform)     //if card is in Unit prompt - move to hand
            {
                zUnit.FreeUnit();
                pickedCard.gameObject.transform.SetParent(Hand.transform);
                CheckUnit();
            }
            zHand.RefreshZone();
        }
    }
    public void DisplayCardClick(Card_Display pickedCard)   //add Card_Display to hand based on what was picked in Book/Storage
    {
        if (Book.activeSelf || (zStorage.time.isDay == false || skipDay)) 
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
        if (visibleMap) //nothing is open
        {
            if (CreateInHand(pickedCard))
            {
                return true;    //unit doesn't need to wait
            }
            else    //no space in Hand
            {
                zUnit.CardAdded(pickedCard, waitingUnit);
                CheckUnit();
                OpenUI();
                return false; //unit needs to wait
            }
        }
        else //something is open on screen
            zUnit.CardAdded(pickedCard, waitingUnit);
        return false;
    }
    public GameObject CreateObject(Transform Zone, Data_Card pickedCard)//creates draggable cards
    {
        GameObject newCard = Instantiate(CardObject, Zone); //create and instantiate objects in zone
        Card_Drag cardObject = newCard.GetComponent<Card_Drag>();
        cardObject.AddCard(pickedCard); //add info to display
        SetCardZones(cardObject);
        newCard.name = string.Format("{0} (Card)", pickedCard.name);//updates name in scene
        return newCard;
    }
    public void CreateObject(Transform Zone, string cardName)           //creates draggable cards
    {
        Data_Card pickedCard = Pool.GetCard(cardName);
        CreateObject(Zone, pickedCard);
    }
    public void CreateDisplay(Transform Zone, Data_Card pickedCard)     //create displayable cards
    {
        GameObject newCard = Instantiate(CardDisplay, Zone); //create and instantiate objects in zone
        Card_Display cardDisplay = newCard.GetComponent<Card_Display>();
        cardDisplay.AddCard(pickedCard); //add info to display;
        cardDisplay.screen = this;
        newCard.name = string.Format("{0} (Card)", pickedCard.name);//updates name in scene
        newCard.transform.localScale -= new Vector3((CardDisplay.transform.localScale.x) / 10, (CardDisplay.transform.localScale.y) / 10, 0);
    }
    public void CreateDisplay(Transform Zone, int index)                //create slots for the Storage
    {
        GameObject newCard = Instantiate(CardDisplay, Zone);   //create and instantiate objects in scene
        Card_Display cardDisplay = newCard.GetComponent<Card_Display>();
        cardDisplay.ClearCard();
        cardDisplay.screen = this;
        newCard.name = string.Format("Slot {0}", index);            //updates name in scene
        newCard.transform.localScale -= new Vector3((CardDisplay.transform.localScale.x) / 10, (CardDisplay.transform.localScale.y) / 10, 0);
        newCard.GetComponent<CanvasGroup>().alpha = .6f;
    }
    private void SetCardZones(Card_Drag card)       //give a draggable card all the needed Zones
    {
        card.canvas = this.GetComponent<Canvas>();
        card.screen = this;
        card.hand = Hand.transform;
        card.menu = CraftMenu.transform;
        card.craft = Craft.transform;
        card.unit = Unit.transform;
        card.storage = Storage.transform;
        card.storagept2 = storageButton.transform;
        card.destroy = destroyButton.transform;

        card.zHand = zHand;
        card.zCraft = zCraft;
        card.zUnit = zUnit;
        card.zStorage = zStorage;
    }
    private bool CreateInHand(Data_Card pickedCard) //try to create a card in Hand
    {
        if (Hand.transform.childCount < zHand.Size)
        {
            if (pickedCard.neverDiscovered)
            {
                pickedCard.neverDiscovered = false;
                //Debug.Log("New card found: " + pickedCard.name);
                Pool.discoveredTotal++;
            }
            CreateObject(Hand.transform, pickedCard);
            zHand.RefreshZone();
            return true;
        }
        return false;
    }
    public void CheckUnit() //open Unit Zone if it has cards
    {
        if (zUnit.hasBacklog)
        {
            TopMessage(UnitMsg);
            visibleMap = false;
            UnitUp = true;
            zUnit.RefreshZone();
        }
        else
        {
            Message.gameObject.SetActive(false);
            UnitUp = false;
        }
    }
    public Cards_Info ExportCards()             //will be used to save the game
    {
        Cards_Info export = new Cards_Info();
        export.Hand = zHand.ExportDeck();
        export.Storage = zStorage.ExportDeck();
        export.Discovered = Pool.discoveredTotal;
        export.Combos = zCraft.CombosTotal;
        List<bool> status = new List<bool>(0);
        for (int i = 0; i < Pool.count; i++)                   //returning all fields in cards back to default 
            status.Add(Pool.cards[i].neverDiscovered);         //done here instead of Card_Pool to ensure that it only happens once
        export.DisStatus = status;
        return export;
    }
    public void ImportCards(Cards_Info import)  //will be used to load the game
    {
        zHand.ImportDeck(import.Hand);
        zStorage.ImportDeck(import.Storage);
        Pool.discoveredTotal = import.Discovered;
        zCraft.CombosTotal = import.Combos;
        List<bool> status = new List<bool>(0);
        for (int i = 0; i < Pool.count; i++)
            Pool.cards[i].neverDiscovered = import.DisStatus[i];
    }
    public class Cards_Info //used for saves
    {
        public List<int> Hand;
        public List<int> Storage;
        public int Discovered;
        public int Combos;
        public List<bool> DisStatus;
    }
}
